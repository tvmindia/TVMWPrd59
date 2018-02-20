using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductionApp.UserInterface.Models
{
    public class ApproverViewModel
    {
        public Guid ID { get; set; }
        public string DocType { get; set; }
        public int Level { get; set; }
        public Guid UserID { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        //additional fields 
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public CommonViewModel Common { get; set; }
        public UserViewModel User { get; set; }
    }

    public class ApproverAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }
}