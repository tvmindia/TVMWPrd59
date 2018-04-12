using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Models
{
    public class EmployeeViewModel
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
        public DepartmentViewModel Department { get; set; }
        public EmployeeCategoryViewModel EmployeeCategory { get; set; }
        public CommonViewModel Common { get; set; }
        public List<SelectListItem> SelectList { get; set; }

    }
    public class EmployeeAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "Department")]
        public string DepartmentCode { get; set; }
        public DepartmentViewModel Department { get; set; }
    }
}