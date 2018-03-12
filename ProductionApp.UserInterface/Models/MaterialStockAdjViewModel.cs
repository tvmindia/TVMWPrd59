using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductionApp.UserInterface.Models
{
    public class MaterialStockAdjViewModel
    {
        public Guid ID { get; set; }
        [Display(Name = "Adjusted By")]
        [Required(ErrorMessage ="Adjusted By is Required")]
        public Guid AdjustedBy { get; set; }
        public Guid? LatestApprovalID { get; set; }
        public DateTime AdjustmentDate { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name ="General Notes")]
        public string Remarks { get; set; }
        public int LatestApprovalStatus { get; set; }
        public bool IsFinalApproved { get; set; }
        public string ApprovalStatus { get; set; }
        public string ReferenceNo { get; set; }
        public CommonViewModel Common { get; set; }

        //Additional Fields Adjustment
        [Display(Name ="Adjustment Date")]
        [Required(ErrorMessage = "Adjustment Date is Required")]
        public string AdjustmentDateFormatted { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public string AdjustedByEmployeeName { get; set; }
        public MaterialViewModel Material { get; set; }
        public MaterialStockAdjDetailViewModel MaterialStockAdjDetail { get; set; }
        public List<MaterialStockAdjViewModel> MaterialStockAdjList { get; set; }
        public List<MaterialStockAdjDetailViewModel> MaterialStockAdjDetailList { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public Guid EmployeeID { get; set; }
        public bool IsUpdate { get; set; }
        public string DetailJSON { get; set; }
    }

    public class MaterialStockAdjDetailViewModel
    {
        public Guid ID { get; set; }
        public Guid AdjustmentID { get; set; }
        public Guid AdjustmentBy { get; set; }
        [Display(Name ="Material")]
        public Guid MaterialID { get; set; }
        [Display(Name ="Quantity")]
        public decimal Qty { get; set; }
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }
        public CommonViewModel Common { get; set; }
        public MaterialViewModel Material { get; set; }
    }

    public class MaterialStockAdjAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }

        [Display(Name = "FromDate")]
        public string FromDate { get; set; }
        [Display(Name = "ToDate")]
        public string ToDate { get; set; }
        [Display(Name ="Adjusted By")]
        public Guid AdjustedBy { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public string AdjustedByEmployeeName { get; set; }
        public string ApprovalStatus { get; set; }
    }
}