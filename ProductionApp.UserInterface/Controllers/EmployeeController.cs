using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.UserInterface.Models;
using Newtonsoft.Json;
using ProductionApp.UserInterface.SecurityFilter;

namespace ProductionApp.UserInterface.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        AppConst _appConst = new AppConst();
        private Common _common = new Common();
        private IEmployeeBusiness _employeeBusiness;
        private IDepartmentBusiness _departmentBusiness;
        private IEmployeeCategoryBusiness _employeeCategoryBusiness;
        public EmployeeController(IEmployeeBusiness employeeBusiness, IDepartmentBusiness departmentBusiness, IEmployeeCategoryBusiness employeeCategoryBusiness)
        {
            _employeeBusiness = employeeBusiness;
            _departmentBusiness = departmentBusiness;
            _employeeCategoryBusiness = employeeCategoryBusiness;
        }
        [AuthSecurityFilter(ProjectObject = "Employee", Mode = "R")]
        public ActionResult Index(string code)
        {
            ViewBag.SysModuleCode = code;
            EmployeeAdvanceSearchViewModel employeeSearchVM = new EmployeeAdvanceSearchViewModel();
            employeeSearchVM.Department = new DepartmentViewModel();
            employeeSearchVM.Department.departmentSelectList = _departmentBusiness.GetDepartmentForSelectList();
            employeeSearchVM.EmployeeCategory = new EmployeeCategoryViewModel();
            employeeSearchVM.EmployeeCategory.employeeCategorySelectList = _employeeCategoryBusiness.GetEmployeeCategoryForSelectList();
            return View(employeeSearchVM);
        }
        #region GetAllEmployee
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "Employee", Mode = "R")]
        public JsonResult GetAllEmployee(DataTableAjaxPostModel model, EmployeeAdvanceSearchViewModel employeeAdvanceSearchVM)
        {
            try
            {
                //setting options to our model
                employeeAdvanceSearchVM.DataTablePaging.Start = model.start;
                employeeAdvanceSearchVM.DataTablePaging.Length = (employeeAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : employeeAdvanceSearchVM.DataTablePaging.Length;

                // action inside a standard controller
                List<EmployeeViewModel> employeeVMList = Mapper.Map<List<Employee>, List<EmployeeViewModel>>(_employeeBusiness.GetAllEmployee(Mapper.Map<EmployeeAdvanceSearchViewModel, EmployeeAdvanceSearch>(employeeAdvanceSearchVM)));
                if (employeeAdvanceSearchVM.DataTablePaging.Length == -1)
                {
                    int totalResult = employeeVMList.Count != 0 ? employeeVMList[0].TotalCount : 0;
                    int filteredResult = employeeVMList.Count != 0 ? employeeVMList[0].FilteredCount : 0;
                    employeeVMList = employeeVMList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
                }
                var settings = new JsonSerializerSettings
                {
                    //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Formatting = Formatting.None
                };
                return Json(new
                {
                    // this is what datatables wants sending back
                    draw = model.draw,
                    recordsTotal = employeeVMList.Count != 0 ? employeeVMList[0].TotalCount : 0,
                    recordsFiltered = employeeVMList.Count != 0 ? employeeVMList[0].FilteredCount : 0,
                    data = employeeVMList
                });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return Json(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion GetAllEmployee

        //#region CheckEmployeeCodeExist
        //public ActionResult CheckEmployeeCodeExist(EmployeeViewModel employeeVM)
        //{
        //    bool exists = _employeeBusiness.CheckEmployeeCodeExist(Mapper.Map<EmployeeViewModel, Employee>(employeeVM));
        //    if (exists)
        //    {
        //        return Json("<p><span style='vertical-align: 2px'>Employee code is in use </span> <i class='fa fa-close' style='font-size:19px; color: red'></i></p>", JsonRequestBehavior.AllowGet);
        //    }
        //    //var result = new { success = true, message = "Success" };
        //    return Json(true, JsonRequestBehavior.AllowGet);
        //}
        //#endregion CheckEmployeeCodeExist

        #region GetEmployee
        [AuthSecurityFilter(ProjectObject = "Employee", Mode = "R")]
        public string GetEmployee(string ID)
        {
            try
            {
                EmployeeViewModel employeeVM = new EmployeeViewModel();
                employeeVM = Mapper.Map<Employee, EmployeeViewModel>(_employeeBusiness.GetEmployee(Guid.Parse(ID)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = employeeVM });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex });
            }
        }

        #endregion GetEmployee

        #region MasterPartial
        [HttpGet]
        public ActionResult MasterPartial(string masterCode)
        {
            EmployeeViewModel employeeVM = string.IsNullOrEmpty(masterCode) ? new EmployeeViewModel() : Mapper.Map<Employee, EmployeeViewModel>(_employeeBusiness.GetEmployee(Guid.Parse(masterCode)));
            //EmployeeViewModel employeeVM = new EmployeeViewModel();
            employeeVM.IsUpdate = string.IsNullOrEmpty(masterCode) ? false : true;
            if (string.IsNullOrEmpty(masterCode))
            {
                employeeVM.IsActive = true;
            }
            employeeVM.Department = new DepartmentViewModel();
            employeeVM.Department.departmentSelectList = _departmentBusiness.GetDepartmentForSelectList();
            employeeVM.EmployeeCategory = new EmployeeCategoryViewModel();
            employeeVM.EmployeeCategory.employeeCategorySelectList = _employeeCategoryBusiness.GetEmployeeCategoryForSelectList();
            return PartialView("_AddEmployeePartial", employeeVM);
        }
        #endregion MasterPartial

        #region InsertUpdateEmployee
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "Employee", Mode = "W")]
        public string InsertUpdateEmployee(EmployeeViewModel employeeVM)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                employeeVM.Common = new CommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _common.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _common.GetCurrentDateTime(),
                };
                var result = _employeeBusiness.InsertUpdateEmployee(Mapper.Map<EmployeeViewModel, Employee>(employeeVM));
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion InsertUpdateEmployee

        #region DeleteEmployee
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Employee", Mode = "D")]
        public string DeleteEmployee(Guid id)
        {
            try
            {
                var result = _employeeBusiness.DeleteEmployee(id);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }

        }
        #endregion DeleteEmployee

        public ActionResult EmployeeDropdown()
        {
            EmployeeViewModel employeeVM = new EmployeeViewModel();
            employeeVM.SelectList = _employeeBusiness.GetEmployeeSelectList();
            return PartialView("_EmployeeDropdown", employeeVM);

        }
        public ActionResult AssembleDropdown()
        {
            EmployeeViewModel employeeVM = new EmployeeViewModel();
            employeeVM.SelectList = _employeeBusiness.GetEmployeeSelectList();
            return PartialView("_AssembleDropdown", employeeVM);

        }
        public ActionResult ReturnByDropdown(string required)
        {
            EmployeeViewModel employeeVM = new EmployeeViewModel();
            ViewBag.IsRequired = required;
            employeeVM.SelectList = _employeeBusiness.GetEmployeeSelectList();
            return PartialView("_ReturnByDropdown", employeeVM);
        }
        public ActionResult ReceivedByDropdown(string required)
        {
            EmployeeViewModel employeeVM = new EmployeeViewModel();
            ViewBag.IsRequired = required;
            employeeVM.SelectList = _employeeBusiness.GetEmployeeSelectList();
            return PartialView("_ReceivedByDropdown", employeeVM);
        }
        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Employee", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddEmployeeMaster('MSTR')";
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetEmployeeList();";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "ImportEmployeeData();";
                    //---------------------------------------
                    break;

                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion
    }
}