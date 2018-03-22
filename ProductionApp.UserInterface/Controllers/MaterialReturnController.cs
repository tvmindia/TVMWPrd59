using AutoMapper;
using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.UserInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Controllers
{
    public class MaterialReturnController : Controller
    {
        IMaterialReturnBusiness _materialReturnBusiness;
        private IEmployeeBusiness _employeeBusiness;
        AppConst _appConst = new AppConst();
        public MaterialReturnController(IMaterialReturnBusiness materialReturnBusiness, IEmployeeBusiness employeeBusiness)
        {
            _materialReturnBusiness = materialReturnBusiness;
            _employeeBusiness = employeeBusiness;
        }
        public ActionResult ViewMaterialReturn(string code)
        {
            ViewBag.SysModuleCode = code;
            MaterialReturnAdvanceSearchViewModel materialReturnAdvanceSearchVM = new MaterialReturnAdvanceSearchViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            materialReturnAdvanceSearchVM.Employee = new EmployeeViewModel();
            materialReturnAdvanceSearchVM.Employee.SelectList = new List<SelectListItem>();
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
            materialReturnAdvanceSearchVM.Employee.SelectList = selectListItem;
            return View(materialReturnAdvanceSearchVM);
        }

        public ActionResult NewMaterialReturn(string code)
        {
            ViewBag.SysModuleCode = code;
            MaterialReturnViewModel materialReturnVM = new MaterialReturnViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            materialReturnVM.Employee = new EmployeeViewModel();
            materialReturnVM.Employee.SelectList = new List<SelectListItem>();
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
            materialReturnVM.Employee.SelectList = selectListItem;
            return View(materialReturnVM);
        }

        #region GetAllReturnToSupplier
        
        public JsonResult GetAllReturnToSupplier(DataTableAjaxPostModel model, MaterialReturnAdvanceSearchViewModel materialReturnAdvanceSearchVM)
        {
            materialReturnAdvanceSearchVM.DataTablePaging.Start = model.start;
            materialReturnAdvanceSearchVM.DataTablePaging.Length = (materialReturnAdvanceSearchVM.DataTablePaging.Length == 0 ? model.length : materialReturnAdvanceSearchVM.DataTablePaging.Length);
            List<MaterialReturnViewModel> materialIssueOrderList = Mapper.Map<List<MaterialReturn>, List<MaterialReturnViewModel>>(_materialReturnBusiness.GetAllReturnToSupplier(Mapper.Map<MaterialReturnAdvanceSearchViewModel, MaterialReturnAdvanceSearch>(materialReturnAdvanceSearchVM)));
            if (materialReturnAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = materialIssueOrderList.Count != 0 ? materialIssueOrderList[0].TotalCount : 0;
                int filteredResult = materialIssueOrderList.Count != 0 ? materialIssueOrderList[0].FilteredCount : 0;
                materialIssueOrderList = materialIssueOrderList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }

            return Json(new
            {
                draw = model.draw,
                recordsTotal = materialIssueOrderList.Count != 0 ? materialIssueOrderList[0].TotalCount : 0,
                recordsFiltered = materialIssueOrderList.Count != 0 ? materialIssueOrderList[0].FilteredCount : 0,
                data = materialIssueOrderList
            });
        }

        #endregion GetAllReturnToSupplier

        #region ButtonStyling
        [HttpGet]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewMaterialReturn", "MaterialReturn", new { code = "STR" });
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetPackingSlipList();";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "ImportPackingSlipData();";
                    //---------------------------------------

                    break;
                case "Edit":

                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "New";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewMaterialReturn", "MaterialReturn", new { code = "STR" });

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

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Event = "";
                    toolboxVM.ListBtn.Href = Url.Action("ViewMaterialReturn", "MaterialReturn", new { Code = "STR" });


                    break;
                case "Add":

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "Save();";

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Event = "";
                    toolboxVM.ListBtn.Href = Url.Action("ViewMaterialReturn", "MaterialReturn", new { Code = "STR" });


                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion
    }
}