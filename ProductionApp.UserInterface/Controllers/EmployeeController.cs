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
        public EmployeeController(IEmployeeBusiness employeeBusiness)
        {
            _employeeBusiness = employeeBusiness;
        }
        public ActionResult Employee()
        {
            return View();
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
    }
}