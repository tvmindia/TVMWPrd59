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
        public string InvoiceNo { get; set; }
        public Guid CustomerID  { get; set; }
        public string PaymentTerm  { get; set; }
        public DateTime InvoiceDate  { get; set; }
        public DateTime PaymentDueDate  { get; set; }
        public string BillingAddress  { get; set; }
        public decimal Discount  { get; set; }
        public string GeneralNotes  { get; set; }

        //additional properties
        public bool IsUpdate { get; set; }
        public Guid hdnFileID { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public string InvoiceDateFormatted { get; set; }
        public string PaymentDueDateFormatted { get; set; }
        public CommonViewModel Common { get; set; }
        public CustomerInvoiceDetailViewModel CustomerInvoiceDetail { get; set; }


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
        public decimal TaxPercApplied { get; set; }
        public decimal TaxAmount { get; set; }
        public bool IsInvoiceInKG { get; set; }
        //additional properties
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