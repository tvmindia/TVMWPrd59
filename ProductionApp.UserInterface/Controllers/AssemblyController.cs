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
        private Common _common = new Common();
        AppConst _appConst = new AppConst();
        Settings settings = new Settings();
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
            assemblyVM.IsUpdate = id == null ? false : true;
            assemblyVM.AssemblyDateFormatted = _common.GetCurrentDateTime().ToString(settings.DateFormat);
            assemblyVM.Product = new ProductViewModel();
            assemblyVM.Product.ProductSelectList = _productBusiness.GetProductForSelectList("PRO");
            return View(assemblyVM);
        }
        [AuthSecurityFilter(ProjectObject = "Assembly", Mode = "R")]
        public ActionResult ViewAssembly(string code)
        {
            ViewBag.SysModuleCode = code;
            AssemblyAdvanceSearchViewModel AssemblyAdvanceSearchVM = new AssemblyAdvanceSearchViewModel();
            AssemblyAdvanceSearchVM.Product = new ProductViewModel();
            AssemblyAdvanceSearchVM.Product.ProductSelectList = _productBusiness.GetProductForSelectList("PRO");
            AssemblyAdvanceSearchVM.Employee = new EmployeeViewModel();
            AssemblyAdvanceSearchVM.Employee.SelectList = _employeeBusiness.GetEmployeeSelectList();
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

        #region GetProductComponentList
        [AuthSecurityFilter(ProjectObject = "Assembly", Mode = "R")]
        public string GetProductComponentList(string id,decimal qty, string assemblyId)
        {
            try
            {
                List<AssemblyViewModel> assemblyList = Mapper.Map<List<Assembly>, List<AssemblyViewModel>>(_assemblyBusiness.GetProductComponentList(Guid.Parse(id),qty,Guid.Parse(assemblyId)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = assemblyList, Message = "Success" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = ex });
            }
        }
        #endregion GetProductComponentList

        #region InsertUpdateAssembly
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "Assembly", Mode = "W")]
        public string InsertUpdateAssembly(AssemblyViewModel assemblyVM)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                assemblyVM.Common = new CommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _common.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _common.GetCurrentDateTime(),
                };
                var result = _assemblyBusiness.InsertUpdateAssembly(Mapper.Map<AssemblyViewModel, Assembly>(assemblyVM));
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion InsertUpdateAssembly

        #region GetAssembly
        [AuthSecurityFilter(ProjectObject = "Assembly", Mode = "R")]
        public string GetAssembly(string id)
        {
            try
            {
                AssemblyViewModel assemblyVM = new AssemblyViewModel();
                assemblyVM = Mapper.Map<Assembly, AssemblyViewModel>(_assemblyBusiness.GetAssembly(Guid.Parse(id)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = assemblyVM });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex });
            }
        }

        #endregion GetAssembly

        #region DeleteAssembly
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Assembly", Mode = "D")]
        public string DeleteAssembly(Guid id)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                var result = _assemblyBusiness.DeleteAssembly(id, appUA.UserName);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }

        }
        #endregion DeleteAssembly

        #region GetPossibleItemQuantityForAssembly
        [AuthSecurityFilter(ProjectObject = "Assembly", Mode = "R")]
        public string GetPossibleItemQuantityForAssembly(string id)
        {
            try
            {
                List<AssemblyViewModel> assemblyList = Mapper.Map<List<Assembly>, List<AssemblyViewModel>>(_assemblyBusiness.GetPossibleItemQuantityForAssembly(Guid.Parse(id)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = assemblyList, Message = "Success" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = ex });
            }
        }
        #endregion GetPossibleItemQuantityForAssembly


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
                    toolboxVM.addbtn.Href = Url.Action("NewAssembly", "Assembly", new { code = "PROD" });
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "BindOrReloadAssembleTable('Reset');";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "BindOrReloadAssembleTable('Export');";
                    //---------------------------------------

                    break;
                case "Edit":

                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "New";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewAssembly", "Assembly", new { code = "PROD" });

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
                    toolboxVM.ListBtn.Href = Url.Action("ViewAssembly", "Assembly", new { code = "PROD" });

                    break;

                case "Disable":

                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "New";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewAssembly", "Assembly", new { code = "PROD" });

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Href = Url.Action("ViewAssembly", "Assembly", new { code = "PROD" });

                    break;
                case "Add":

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "Save();";

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Href = Url.Action("ViewAssembly", "Assembly", new { code = "PROD" });
                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion
    }
}