
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Models
{
    public class ApproverViewModel
    {
        public Guid ID { get; set; }
        [Display(Name = "Document Type")]
        [Required(ErrorMessage = "Document Type is missing")]
        public string DocumentTypeCode { get; set; }
        public int Level { get; set; }
        [Required(ErrorMessage = "User is missing")]
        [Display(Name = "User")]
        public Guid UserID { get; set; }
        [Display(Name = "Is Default Approver")]
        public bool IsDefault { get; set; }
        public string IsDefaultString { get; set; }
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        //additional fields 
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public CommonViewModel Common { get; set; }
        public UserViewModel User { get; set; }
        public DocumentTypeViewModel DocumentType { get; set; }
    }

    public class ApproverAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        public DocumentTypeViewModel DocumentType { get; set; }
        public ApproverViewModel Approver { get;set;}
    }
}