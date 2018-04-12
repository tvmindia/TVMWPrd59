using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.UserInterface.Models;

namespace ProductionApp.UserInterface.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        private IEmployeeBusiness _employeeBusiness;
        private IDepartmentBusiness _departmentBusiness;
        public EmployeeController(IEmployeeBusiness employeeBusiness, IDepartmentBusiness departmentBusiness)
        {
            _employeeBusiness = employeeBusiness;
            _departmentBusiness = departmentBusiness;
        }
        public ActionResult Index(string code)
        {
            ViewBag.SysModuleCode = code;
            EmployeeAdvanceSearchViewModel employeeSearchVM = new EmployeeAdvanceSearchViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            employeeSearchVM.Department = new DepartmentViewModel();
            employeeSearchVM.Department.SelectList = new List<SelectListItem>();
            List<DepartmentViewModel> departmentList= Mapper.Map<List<Department>, List<DepartmentViewModel>>(_departmentBusiness.GetDepartmentForSelectList());
            if(departmentList != null)
                foreach(DepartmentViewModel department in departmentList)
                {
                    selectListItem.Add(new SelectListItem
                    { 
                        Text=department.Name,
                        Value=department.Code,
                        Selected = false
                    });
                }
            employeeSearchVM.Department.SelectList = selectListItem;
            return View(employeeSearchVM);
        }
        public ActionResult EmployeeDropdown()
        {
            EmployeeViewModel employeeVM = new EmployeeViewModel();
            employeeVM.SelectList = new List<SelectListItem>();
            List<EmployeeViewModel> employeeList = Mapper.Map<List<Employee>, List<EmployeeViewModel>>(_employeeBusiness.GetEmployeeForSelectList());
            if (employeeList != null)
                foreach (EmployeeViewModel employee in employeeList)
                {
                    employeeVM.SelectList.Add(new SelectListItem
                    {
                        Text = employee.Name,
                        Value = employee.ID.ToString(),
                        Selected = false
                    });
                }
            return PartialView("_EmployeeDropdown", employeeVM);

        }
        #region ButtonStyling
        [HttpGet]
        //[AuthSecurityFilter(ProjectObject = "Employee", Mode = "R")]
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