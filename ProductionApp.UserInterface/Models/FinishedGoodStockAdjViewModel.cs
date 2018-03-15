using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductionApp.UserInterface.Models
{
    public class FinishedGoodStockAdjViewModel
    {
        public Guid ID { get; set; }
        [Display(Name = "Adjusted By")]
        [Required(ErrorMessage = "Adjusted By is Required")]
        public Guid AdjustedBy { get; set; }
        public Guid? LatestApprovalID { get; set; }
        public DateTime Date { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "General Notes")]
        public string Remarks { get; set; }
        public int LatestApprovalStatus { get; set; }
        public bool IsFinalApproved { get; set; }
        public string ApprovalStatus { get; set; }
        public string ReferenceNo { get; set; }
        public CommonViewModel Common { get; set; }

        //Additional Fields Adjustment
        [Display(Name = "Adjustment Date")]
        [Required(ErrorMessage = "Adjustment Date is Required")]
        public string AdjustmentDateFormatted { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public string AdjustedByEmployeeName { get; set; }
        public Guid EmployeeID { get; set; }    
        public List<FinishedGoodStockAdjDetailViewModel> FinishedGoodStockAdjDetailList { get; set; }
        public FinishedGoodStockAdjDetailViewModel FinishedGoodStockAdjDetail { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public string DetailJSON { get; set; }
        public bool IsUpdate { get; set; }

    }

    public class FinishedGoodStockAdjDetailViewModel
    {
        public Guid ID { get; set; }
        public Guid AdjustmentID { get; set; }
        [Display(Name ="Product")]
        public Guid ProductID { get; set; }
        [Display(Name = "Quantity")]
        public decimal Qty { get; set; }
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }
        public CommonViewModel Common { get; set; }
        public ProductViewModel Product { get; set; }
    }

    public class FinishedGoodStockAdjAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }

        [Display(Name = "FromDate")]
        public string FromDate { get; set; }
        [Display(Name = "ToDate")]
        public string ToDate { get; set; }
        [Display(Name = "Adjusted By")]
        public Guid AdjustedBy { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public string AdjustedByEmployeeName { get; set; }
        public string ApprovalStatus { get; set; }
    }
}