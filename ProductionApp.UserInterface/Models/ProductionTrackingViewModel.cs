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
        [Display(Name = "Production Referance No")]
        public string ProductionRefNo { get; set; }
        [Display(Name = "Product")]
        public Guid ProductID { get; set; }
        public Guid LineStageDetailID { get; set; }
        [Display(Name = "Accepted Quantity")]
        public int? AcceptedQty { get; set; }
        [Display(Name = "Accepted Weight")]
        public decimal? AcceptedWt { get; set; }
        [Display(Name = "Damaged Quantity")]
        public int? DamagedQty { get; set; }
        [Display(Name = "Damaged Weight")]
        public decimal? DamagedWt { get; set; }
        [Display(Name = "Employee")]
        public Guid ForemanID { get; set; }
        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }
        public CommonViewModel Common { get; set; }
        //Additional
        public bool IsUpdate { get; set; }
        public string EntryDateFormatted { get; set; }
        public string SearchDetail { get; set; }

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
        public ProductViewModel Product { get; set; }
        [Display(Name = "Done By")]
        public EmployeeViewModel Employee { get; set; }

        //[Display(Name = "Stage")]
        //public StageViewModel Stage { get; set; }
        //[Display(Name = "Output")]
        //public SubComponentViewModel SubComponent { get; set; }
        //public ProductViewModel OutputComponent { get; set; }

        //[Display(Name = "Component")]
        //public ProductViewModel Component { get; set; }
    }

}