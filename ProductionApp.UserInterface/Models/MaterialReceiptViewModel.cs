using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ProductionApp.DataAccessObject.DTO;

namespace ProductionApp.UserInterface.Models
{
    public class MaterialReceiptViewModel
    {
        public Guid ID { get; set; }
        [Required(ErrorMessage = "Please select a Supplier")]
        public Guid SupplierID { get; set; }
        public Guid PurchaseOrderID { get; set; }
        [Required(ErrorMessage = "Purchase Order is required")]
        [Display(Name = "Purchase Order")]
        public string PurchaseOrderNo { get; set; }
        [Required(ErrorMessage = "Receipt No is required")]
        [Display(Name = "Receipt No")]
        public string ReceiptNo { get; set; }
        [Display(Name = "Receipt Date")]
        public DateTime ReceiptDate { get; set; }
        [Display(Name = "General Notes")]
        [DataType(DataType.MultilineText)]
        public string GeneralNotes { get; set; }
        public CommonViewModel Common { get; set; }

        //Additional Fields
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public string DetailJSON { get; set; }
        [Required(ErrorMessage = "Receipt Date is required")]
        public string ReceiptDateFormatted { get; set; }
        public MaterialReceiptDetailViewModel MaterialReceiptDetail { get; set; }
        [Display(Name ="Supplier")]
        public SupplierViewModel Supplier { get; set; }
        public bool IsUpdate { get; set; }

        public List<MaterialReceiptViewModel> MaterialReceiptList { get; set; }
        public List<MaterialReceiptDetailViewModel> MaterialReceiptDetailList { get; set; }
    }

    public class MaterialReceiptDetailViewModel
    {
        public Guid ID { get; set; }
        public Guid HeaderID { get; set; }
        [Display(Name = "Material")]
        public Guid MaterialID { get; set; }
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string MaterialDesc { get; set; }
        [Display(Name = "Unit")]
        public string UnitCode { get; set; }
        [Display(Name = "Quantity")]
        public decimal Qty { get; set; }
        public CommonViewModel Common { get; set; }

        //Additional Fields
        [Display(Name = "Raw Material")]
        public MaterialViewModel Material { get; set; }
    }

    public class MaterialReceiptAdvanceSearchViewModel
    {
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        [Display(Name = "From Date")]
        public string FromDate { get; set; }
        [Display(Name = "To Date")]
        public string ToDate { get; set; }
        [Display(Name = "Supplier")]
        public SupplierViewModel Supplier { get; set; }
        [Display(Name = "Purchase Order")]
        public PurchaseOrderViewModel PurchaseOrder { get; set; }
    }
}