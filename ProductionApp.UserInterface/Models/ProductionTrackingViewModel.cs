using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ProductionApp.UserInterface.Models
{
    public class ProductionTrackingViewModel
    {
        public Guid ID { get; set; }
        [Display(Name = "Entry Date")]
        public DateTime EntryDate { get; set; }
        [Display(Name = "Production Reference No")]
        public string ProductionRefNo { get; set; }
        [Display(Name = "Product")]
        public Guid ProductID { get; set; }
        public Guid LineStageDetailID { get; set; }
        [Range(0, int.MaxValue)]
        [Display(Name = "Accepted Quantity")]
        public int? AcceptedQty { get; set; }
        [Display(Name = "Accepted Weight")]
        public decimal? AcceptedWt { get; set; }
        [Range(0, int.MaxValue)]
        [Display(Name = "Damaged Quantity")]
        public int? DamagedQty { get; set; }
        [Display(Name = "Damaged Weight")]
        public decimal? DamagedWt { get; set; }
        [Display(Name = "Employee")]
        [Required(ErrorMessage = "Employee required")]
        public Guid ForemanID { get; set; }
        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }
        public DateTime PostedDate { get; set; }
        public string PostedBy { get; set; }
        public CommonViewModel Common { get; set; }
        //Additional
        public bool IsUpdate { get; set; }
        [Required(ErrorMessage = "EntryDate required")]
        public string EntryDateFormatted { get; set; }
        public string SearchDetail { get; set; }
        public string PostedDateFormatted { get; set; }
        public int PreviousQty { get; set; }
        public int TotalQty { get; set; }
        public bool IsValid { get; set; }
        public int SlNo { get; set; }
        public string ErrorMessage { get; set; }

        [Display(Name = "Product")]
        public ProductViewModel Product { get; set; }
        [Display(Name = "Component")]
        public ProductViewModel Component { get; set; }
        [Display(Name = "Stage")]
        public StageViewModel Stage { get; set; }
        [Display(Name = "Output")]
        public ProductViewModel OutputComponent { get; set; }
        [Display(Name = "SubComponent")]
        public SubComponentViewModel SubComponent { get; set; }
        [Display(Name = "Done By")]
        public EmployeeViewModel Employee { get; set; }
        public BOMComponentLineStageDetailViewModel BOMComponentLineStageDetail { get; set; }

        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public List<ProductionTrackingViewModel> ProductionTrackingList { get; set; }
        public string BaseURL { get; set; }
    }

    public class ProductionTrackingAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "From Date")]
        public string FromDate { get; set; }
        [Display(Name = "To Date")]
        public string ToDate { get; set; }
        [Display(Name = "Product")]
        public Guid ProductID { get; set; }
        public ProductViewModel Product { get; set; }
        [Display(Name = "Done By")]
        public Guid EmployeeID { get; set; }
        public EmployeeViewModel Employee { get; set; }
        [Display(Name = "Stage")]
        public Guid StageID { get; set; }
        public StageViewModel Stage { get; set; }
        [Display(Name = "Transactions Upto")]
        public string PostDate { get; set; }
        [Display(Name = "Posted By")]
        public Guid? LineStageDetailID { get; set; }
        [Display(Name ="Posting Status")]
        public bool? Status { get; set; }
        [Display(Name = "Damaged Y/N")]
        public bool? IsDamaged { get; set; }

        //[Display(Name = "Output")]
        //public SubComponentViewModel SubComponent { get; set; }
        //public ProductViewModel OutputComponent { get; set; }
        //[Display(Name = "Component")]
        //public ProductViewModel Component { get; set; }
    }

}