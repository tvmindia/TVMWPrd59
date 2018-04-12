using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    public class Employee
    {
        public Guid ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public string EmpType { get; set; }
        public string ImageURL { get; set; }
        public string CompanyID { get; set; }
        public string GeneralNotes { get; set; }
        public string DepartmentCode { get; set; }
        public string EmployeeCategoryCode { get; set; }
        public bool IsActive { get; set; }
        //additional fields 
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public Common Common { get; set; }
        public Department Department { get; set; }
        public EmployeeCategory EmployeeCategory { get; set; }
    }
    public class EmployeeAdvanceSearch
    {
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public string DepartmentCode { get; set; }
        public Department Department { get; set; }
    }
}
