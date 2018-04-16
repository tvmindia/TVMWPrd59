using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductionApp.UserInterface.Models
{
    public class SupplierInvoiceViewModel
    {
        public Guid ID { get; set; }
        [Display(Name = "Invoice No")]
        public string InvoiceNo { get; set; }
        [Display(Name = "Supplier")]
        public Guid SupplierID { get; set; }
        [Display(Name = "Payment Term")]
        public string PaymentTermCode { get; set; }
        [Display(Name = "Invoice Date")]
        public string InvoiceDateFormatted { get; set; }
        [Display(Name = "Payment Due Date")]
        public string PaymentDueDateFormatted { get; set; }
        [Display(Name = "Billing Address")]
        public string BillingAddress { get; set; }
        [Display(Name = "Shipping Address")]
        public string ShippingAddress { get; set; }
        public decimal Discount { get; set; }
        [Display(Name = "General Notes")]
        public string GeneralNotes { get; set; }
        [Display(Name = "Account Head")]
        public string AccountCode { get; set; }

        //Additional Reference
        public bool IsUpdate { get; set; }
        public Guid hdnFileID { get; set; }
        public int TotalCount { get; set; }
        public string DetailJSON { get; set; }
        public int FilteredCount { get; set; }

        public decimal TotalTaxableAmount { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public decimal InvoiceAmount { get; set; }
        public CommonViewModel Common { get; set; }
        public SupplierViewModel Supplier { get; set; }
    }
    public class SupplierInvoiceDetailViewModel
    {
        public Guid ID { get; set; }
        public Guid SupplierInvoiceID { get; set; }
        public Guid MaterialID { get; set; }
        public decimal Quantity { get; set; }
        public string UnitCode { get; set; }
        public decimal TradeDiscountPerc { get; set; }
        public decimal TradeDiscountAmount { get; set; }
        public string TaxTypeCode { get; set; }
        public decimal CGSTPerc { get; set; }
        public decimal SGSTPerc { get; set; }
        public decimal IGSTPerc { get; set; }
 
        //Additional Reference
        public CommonViewModel Common { get; set; }
    }

    public class SupplierInvoiceAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "From Date")]
        public string FromDate { get; set; }
        [Display(Name = "To Date")]
        public string ToDate { get; set; }

        [Display(Name = "Supplier")]
        public Guid SupplierID { get; set; }
        public SupplierViewModel Supplier { get; set; }

    }
}