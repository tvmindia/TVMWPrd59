using AutoMapper;
using Newtonsoft.Json;
using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.UserInterface.Models;
using ProductionApp.UserInterface.SecurityFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Controllers
{
    public class AssemblyController : Controller
    {
        private IEmployeeBusiness _employeeBusiness;
        private IProductBusiness _productBusiness;
        private IAssemblyBusiness _assemblyBusiness;
        // GET: Assembly
        public AssemblyController(IEmployeeBusiness employeeBusiness, IProductBusiness productBusiness, IAssemblyBusiness assemblyBusiness)
        {
            _employeeBusiness = employeeBusiness;
            _productBusiness = productBusiness;
            _assemblyBusiness = assemblyBusiness;
        }
        [AuthSecurityFilter(ProjectObject = "Assembly", Mode = "R")]
        public ActionResult NewAssembly(string code, Guid? id)
        {
            ViewBag.SysModuleCode = code;
            AssemblyViewModel assemblyVM = new AssemblyViewModel();
            assemblyVM.ID = id == null ? Guid.Empty : (Guid)id;
            return View(assemblyVM);
        }
        [AuthSecurityFilter(ProjectObject = "Assembly", Mode = "R")]
        public ActionResult ViewAssembly(string code)
        {
            ViewBag.SysModuleCode = code;
            AssemblyAdvanceSearchViewModel AssemblyAdvanceSearchVM = new AssemblyAdvanceSearchViewModel();
            AssemblyAdvanceSearchVM.Product = new ProductViewModel();
            AssemblyAdvanceSearchVM.Product.ProductSelectList = _productBusiness.GetProductForSelectList();
            AssemblyAdvanceSearchVM.Employee = new EmployeeViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            AssemblyAdvanceSearchVM.Employee.SelectList = new List<SelectListItem>();
            List<EmployeeViewModel> employeeList = Mapper.Map<List<Employee>, List<EmployeeViewModel>>(_employeeBusiness.GetEmployeeForSelectList());
            if (employeeList != null)
            {
                foreach (EmployeeViewModel Emp in employeeList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = Emp.Name,
                        Value = Emp.ID.ToString(),
                        Selected = false,
                    });

                }
            }
            AssemblyAdvanceSearchVM.Employee.SelectList = selectListItem;
            return View(AssemblyAdvanceSearchVM);
        }

        #region GetAllAssembly
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "Assembly", Mode = "R")]
        public JsonResult GetAllAssembly(DataTableAjaxPostModel model, AssemblyAdvanceSearchViewModel assemblyAdvanceSearchVM)
        {
            assemblyAdvanceSearchVM.DataTablePaging.Start = model.start;
            assemblyAdvanceSearchVM.DataTablePaging.Length = (assemblyAdvanceSearchVM.DataTablePaging.Length == 0 ? model.length : assemblyAdvanceSearchVM.DataTablePaging.Length);
            List<AssemblyViewModel> assemblyList = Mapper.Map<List<Assembly>, List<AssemblyViewModel>>(_assemblyBusiness.GetAllAssembly(Mapper.Map<AssemblyAdvanceSearchViewModel, AssemblyAdvanceSearch>(assemblyAdvanceSearchVM)));
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.None
            };
            return Json(new
            {
                draw = model.draw,
                recordsTotal = assemblyList.Count != 0 ? assemblyList[0].TotalCount : 0,
                recordsFiltered = assemblyList.Count != 0 ? assemblyList[0].FilteredCount : 0,
                data = assemblyList

            });
        }
        #endregion GetAllAssembly

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Assembly", Mode = "")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewAssembly", "Assembly", new { Code = "PROD" });
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "Reset();";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "Import();";
                    //---------------------------------------

                    break;
                case "Edit":

                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "New";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewAssembly", "Assembly", new { Code = "PROD" });

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "Save();";

                    toolboxVM.deletebtn.Visible = true;
                    toolboxVM.deletebtn.Text = "Delete";
                    toolboxVM.deletebtn.Title = "Delete";
                    toolboxVM.deletebtn.Event = "DeleteClick()";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "Reset();";

                    toolboxVM.SendForApprovalBtn.Visible = true;
                    toolboxVM.SendForApprovalBtn.Text = "Send";
                    toolboxVM.SendForApprovalBtn.Title = "Send For Approval";
                    toolboxVM.SendForApprovalBtn.Event = "ShowSendForApproval('REQ');";

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Href = Url.Action("ViewAssembly", "Assembly", new { Code = "PROD" });

                    break;

                case "Disable":

                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "New";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewAssembly", "Assembly", new { Code = "PROD" });

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Href = Url.Action("ViewAssembly", "Assembly", new { Code = "PROD" });

                    break;
                case "Add":

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "Save();";

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Href = Url.Action("ViewAssembly", "Assembly", new { Code = "PROD" });
                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion
    }
}