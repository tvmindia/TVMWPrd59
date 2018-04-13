using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductionApp.BusinessService.Contracts;
using ProductionApp.RepositoryServices.Contracts;
using ProductionApp.DataAccessObject.DTO;

namespace ProductionApp.BusinessService.Services
{
    public class EmployeeBusiness : IEmployeeBusiness
    {
        private IEmployeeRepository _employeeRepository;
        public EmployeeBusiness(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        public List<Employee> GetEmployeeForSelectList()
        {
            return _employeeRepository.GetEmployeeForSelectList();
        }
        public List<Employee> GetAllEmployee(EmployeeAdvanceSearch employeeAdvanceSearch)
        {
            return _employeeRepository.GetAllEmployee(employeeAdvanceSearch);
        }
        public object InsertUpdateEmployee(Employee employee)
        {
            return _employeeRepository.InsertUpdateEmployee(employee);
        }
        public Employee GetEmployee(Guid id)
        {
            return _employeeRepository.GetEmployee(id);
        }
        public object DeleteEmployee(Guid id)
        {
            return _employeeRepository.DeleteEmployee(id);
        }
        public bool CheckEmployeeCodeExist(Employee employee)
        {
            return _employeeRepository.CheckEmployeeCodeExist(employee);
        }
    }
}
