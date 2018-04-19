using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductionApp.DataAccessObject.DTO;
using System.Web.Mvc;

namespace ProductionApp.BusinessService.Contracts
{
    public interface IEmployeeBusiness
    {
        List<SelectListItem> GetEmployeeSelectList();
        List<Employee> GetEmployeeForSelectList();
        List<Employee> GetAllEmployee(EmployeeAdvanceSearch employeeAdvanceSearch);
        object InsertUpdateEmployee(Employee employee);
        Employee GetEmployee(Guid id);
        object DeleteEmployee(Guid id);
        //bool CheckEmployeeCodeExist(Employee employee);
    }
}
