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
        public string IssueDateFormatted { get; set; }
        public string IssueToEmployeeName { get; set; }
        public string IssuedByEmployeeName { get; set; }
        public MaterialIssueDetailViewModel MaterialIssueDetail { get; set; }
        public string DetailJSON { get; set; }
        public bool IsUpdate { get; set; }
        public List<MaterialIssueDetailViewModel> MaterialIssueDetailList { get; set; }
        public List<MaterialIssueViewModel> MaterialIssueList { get; set; }
        public EmployeeViewModel Employee { get; set; }         
    }
    public class MaterialIssueDetailViewModel
    {
        public Guid ID { get; set; }
        public Guid HeaderID { get; set; }
        public Guid MaterialID { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialDesc { get; set; }
        public string UnitCode { get; set; }
        public decimal Qty { get; set; }
        public string OtherUnit { get; set; }
        public decimal OtherQty { get; set; }
        public CommonViewModel Common { get; set; }       
    }

    public class MaterialIssueAdvanceSearchViewModel
    {

        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }

    }
}