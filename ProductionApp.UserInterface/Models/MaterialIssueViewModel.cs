using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Models
{
    public class MaterialIssueViewModel
    {
        public Guid ID { get; set; }
        [Display(Name = "Issue To")]
        public Guid IssueTo { get; set; }
        [Display(Name = "Issued By")]
        public Guid IssuedBy { get; set; }
        [Display(Name = "Issue No")]
        public string IssueNo { get; set; }
        [Display(Name = "Issue Date")]
        public DateTime IssueDate { get; set; }
        [Display(Name = "Issue Date")]
        [Required(ErrorMessage ="Issue Date Required")]
        public string IssueDateFormatted { get; set; }
        [DataType(DataType.MultilineText)]
        public string GeneralNotes { get; set; }
        public CommonViewModel Common { get; set; }

        //Additional Fields
        public string IssueToEmployeeName { get; set; }
        public string IssuedByEmployeeName { get; set; }
        public MaterialIssueDetailViewModel MaterialIssueDetail { get; set; }
        public string DetailJSON { get; set; }
        public bool IsUpdate { get; set; }
        public List<MaterialIssueDetailViewModel> MaterialIssueDetailList { get; set; }
        public List<MaterialIssueViewModel> MaterialIssueList { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
    }
    public class MaterialIssueDetailViewModel
    {
        public Guid ID { get; set; }
        public Guid HeaderID { get; set; }
        [Display(Name ="Material")]
        public Guid MaterialID { get; set; }
        [Display(Name ="Material Description")]
        public string MaterialDesc { get; set; }
        [Display(Name = "Unit Code")]
        public string UnitCode { get; set; }
        [Display(Name = "Quantity")]
        public decimal Qty { get; set; }
        public string OtherUnit { get; set; }
        public decimal OtherQty { get; set; }
        public CommonViewModel Common { get; set; }   
        public MaterialIssueViewModel MaterialIssue { get; set; }
        public MaterialViewModel Material { get; set; }
      
    }

    public class MaterialIssueAdvanceSearchViewModel
    {

        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }

        [Display(Name = "FromDate")]
        public string FromDate { get; set; }
        [Display(Name = "ToDate")]
        public string ToDate { get; set; }
        [Display(Name ="Issued By")]
        public Guid IssuedBy { get; set; }
        [Display(Name ="Issue To")]
        public Guid IssueTo { get; set; }
        public string IssuedByEmployeeName { get; set; }
        public string IssueToEmployeeName { get; set; }       
        public EmployeeViewModel Employee { get; set; }
    }
}