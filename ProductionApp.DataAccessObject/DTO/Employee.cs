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
        public string Department { get; set; }
        public string EmployeeCategory { get; set; }
        public bool IsActive { get; set; }
        public Common Common { get; set; }
    }
}
