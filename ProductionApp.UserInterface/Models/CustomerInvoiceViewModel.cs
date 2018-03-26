using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductionApp.UserInterface.Models
{
    public class CustomerInvoiceViewModel
    {
        public Guid ID { get; set; }
        [Display(Name = "Invoice No")]
        public string InvoiceNo { get; set; }
        [Required(ErrorMessage = "Customer is missing")]
        [Display(Name = "Customer")]
        public Guid CustomerID  { get; set; }
        [Display(Name = "Payment Term")]
        public string PaymentTerm  { get; set; }
        public DateTime InvoiceDate  { get; set; }
        public DateTime PaymentDueDate  { get; set; }
        [Display(Name = "Billing Address")]
        public string BillingAddress  { get; set; }
        public decimal Discount  { get; set; }
        [Display(Name = "General Notes")]
        public string GeneralNotes  { get; set; }

        //additional properties
        public bool IsUpdate { get; set; }
        public Guid hdnFileID { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        [Display(Name = " Invoice Date")]
        [Required(ErrorMessage = "Invoice Date is missing")]
        public string InvoiceDateFormatted { get; set; }
        [Required(ErrorMessage = "Payment Due Date is missing")]
        [Display(Name = "Payment Due Date")]
        public string PaymentDueDateFormatted { get; set; }
        public CommonViewModel Common { get; set; }
        public CustomerInvoiceDetailViewModel CustomerInvoiceDetail { get; set; }
        public PackingSlipViewModel PackingSlip { get; set; }

    }
    public class CustomerInvoiceDetailViewModel
    {
        public Guid ID { get; set; }
        public Guid CustomerInvoiceID { get; set; }
        public Guid ProductID { get; set; }
        public decimal Quantity { get; set; }
        public decimal Weight { get; set; }
        public decimal Rate { get; set; }
        public string TaxTypeCode { get; set; }
        public decimal IGSTPerc { get; set; }
        public decimal SGSTPerc { get; set; }
        public decimal CGSTPerc { get; set; }
        public decimal TradeDiscountPerc { get; set; }
        public decimal TradeDiscountAmount { get; set; }
        public bool IsInvoiceInKG { get; set; }
        //additional properties
        public string ProductName { get; set; }
        public decimal Total { get; set; }
        public Guid PackingSlipDetailID { get; set; }
        public decimal QuantityCheck { get; set; }
        public decimal WeightCheck { get; set; }

    }

    public class CustomerInvoiceDetailLinkViewModel
    {
        public Guid ID { get; set; }
    }

    public class CustomerInvoiceAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "FromDate")]
        public string FromDate { get; set; }
        [Display(Name = "ToDate")]
        public string ToDate { get; set; } 

    }
}