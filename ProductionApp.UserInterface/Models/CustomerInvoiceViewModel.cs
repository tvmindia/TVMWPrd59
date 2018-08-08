using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using UserInterface.Models;

namespace ProductionApp.UserInterface.Models
{
    public class CustomerInvoiceViewModel
    {
        public Guid ID { get; set; }
        [Display(Name = "Invoice No")]
        public string InvoiceNo { get; set; }
        [Display(Name = "Invoice Type")]
        public string InvoiceType { get; set; }
        [Required(ErrorMessage = "Customer is missing")]
        [Display(Name = "Customer")]
        public Guid CustomerID  { get; set; }
        [Display(Name = "Reference Customer")]
        public Guid? ReferenceCustomer { get; set; }
        [Display(Name = "Payment Term")]
        public string PaymentTermCode { get; set; }
        public DateTime InvoiceDate  { get; set; }
        public DateTime PaymentDueDate  { get; set; }
        [Display(Name = "Billing Address")]
        public string BillingAddress  { get; set; }
        [Display(Name = "Cash Discount")]
        public decimal Discount  { get; set; }
        [Display(Name = "General Notes")]
        public string GeneralNotes  { get; set; }

        //additional properties
        public bool IsUpdate { get; set; }
        public Guid hdnFileID { get; set; }
        public bool hdnIsRegular { get; set; }
        public int TotalCount { get; set; }
        public string DetailJSON { get; set; }
        public string Status { get; set; }
        public string LastPaymentDateFormatted { get; set; }
        public decimal BalanceDue { get; set; }
        public int FilteredCount { get; set; }
        [Display(Name = " Invoice Date")]
        [Required(ErrorMessage = "Invoice Date is missing")]
        public string InvoiceDateFormatted { get; set; }
        [Required(ErrorMessage = "Payment Due Date is missing")]
        [Display(Name = "Payment Due Date")]
        public string PaymentDueDateFormatted { get; set; }

        [Display(Name = "Total Tax Amount")]
        public decimal TotalTaxAmount { get; set; }
        [Display(Name = "Total Taxable Amount")]
        public decimal TotalTaxableAmount { get; set; }
        [Display(Name = "Invoice Amount")]
        public decimal InvoiceAmount { get; set; }
        public decimal PaymentReceived { get; set; }
        public decimal Balance { get; set; }
        public CommonViewModel Common { get; set; }
        public CustomerViewModel Customer { get; set; }
        public CustomerInvoiceDetailViewModel CustomerInvoiceDetail { get; set; }
        public PackingSlipViewModel PackingSlip { get; set; }
        public List<CustomerInvoiceDetailViewModel> CustomerInvoiceDetailList { get; set; }
        public CustomerPaymentViewModel CustomerPayment { get; set; }
        public List<CustomerInvoiceViewModel> CustomerInvoiceList { get; set; }
        public CustomerInvoiceMailPreviewViewModel CustomerInvoiceMailPreview { get; set; }
        public PDFTools PDFTools { get; set; }
        public string BaseURL { get; set; }
        //Mail and Print 
        [Display(Name = "Body Header")]
        [DataType(DataType.MultilineText)]
        public string MailBodyHeader { get; set; }
        [Display(Name = "Body Footer")]
        [DataType(DataType.MultilineText)]
        public string MailBodyFooter { get; set; }
        public string EmailSentYN { get; set; }
        public string SubscriberEmail { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyName { get; set; }
        public string LogoURL { get; set; }
        public string BankName { get; set; }
        public string BankAccDetail { get; set; }
        public string InvoiceDeclaration { get; set; }
        public string InvoiceAmountWords { get; set; }
        public ServiceItemsViewModel ServiceItems { get; set; }
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
        public Guid GroupID { get; set; }
        public string GroupName { get; set; }
        public string ProductName { get; set; }
        public string TaxTypeDescription { get; set; }
        public string SlipNo { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal Total { get; set; }
        public Guid PackingSlipDetailID { get; set; }
        public Guid CustomerInvoiceDetailLinkID { get; set; }
        public decimal QuantityCheck { get; set; }
        public decimal WeightCheck { get; set; }
        public ProductViewModel Product { get; set; }
        public Guid ServiceItemID { get; set; }
        public string ServiceName { get; set; }
    }

    public class CustomerInvoiceDetailLinkViewModel
    {
        public Guid ID { get; set; }
        public Guid PackingSlipDetailID { get; set; }
        public Guid CustomerInvoiceDetailID { get; set; }
        public decimal Quantity { get; set; }
        public decimal Weight { get; set; }
    }

    public class CustomerInvoiceAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "From Date")]
        public string FromDate { get; set; }
        [Display(Name = "To Date")]
        public string ToDate { get; set; }

        [Display(Name = "Customer")]
        public Guid CustomerID { get; set; }
        public CustomerViewModel Customer { get; set; }
        public ServiceItemsViewModel ServiceItems { get; set; }

    }
    public class CustomerInvoiceMailPreviewViewModel
    {
        [Display(Name = "Send To")]
        public string SentToEmails { get; set; }
        public string MailBody { get; set; }
        public bool Flag { get; set; }
        public CustomerInvoiceViewModel CustomerInvoice { get; set; }
    }
}