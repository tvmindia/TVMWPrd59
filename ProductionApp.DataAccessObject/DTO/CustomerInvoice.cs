using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    public class CustomerInvoice
    {
        public Guid ID { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceType { get; set; }
        public Guid CustomerID { get; set; }
        public Guid? ReferenceCustomer { get; set; }
        public string PaymentTermCode { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime PaymentDueDate { get; set; }
        public string BillingAddress { get; set; }
        public decimal Discount { get; set; }
        public string GeneralNotes { get; set; }

        //additional properties
        public bool IsUpdate { get; set; }
        public Guid hdnFileID { get; set; }
        public bool hdnIsRegular { get; set; }
        public int TotalCount { get; set; }
        public string DetailJSON { get; set; }
        public string DetailXML { get; set; }
        public string Status { get; set; }
        public decimal BalanceDue { get; set; }
        public int FilteredCount { get; set; }
        public string InvoiceDateFormatted { get; set; }
        public string PaymentDueDateFormatted { get; set; }
        public string LastPaymentDateFormatted { get; set; }

        public decimal TotalTaxAmount { get; set; }
        public decimal TotalTaxableAmount { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal PaymentReceived { get; set; }
        public decimal Balance { get; set; }
        public Common Common { get; set; }
        public Customer Customer { get; set; }
        public CustomerInvoiceDetail CustomerInvoiceDetail { get; set; }
        public PackingSlip PackingSlip { get; set; }
        public List<CustomerInvoiceDetail> CustomerInvoiceDetailList { get; set; }
        public CustomerPayment CustomerPayment { get; set; }
        public List<CustomerInvoice> CustomerInvoiceList { get; set; }
        public CustomerInvoiceMailPreview CustomerInvoiceMailPreview { get; set; }
        public string BaseURL { get; set; }
        public string MailBodyHeader { get; set; }
        public string MailBodyFooter { get; set; }
        public string EmailSentYN { get; set; }
        public string SubscriberEmail { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyName { get; set; }
        public string LogoURL { get; set; }
        public string BankName{ get; set; }
        public string BankAccDetail { get; set; }
        public string InvoiceDeclaration { get; set; }
        public string InvoiceAmountWords { get; set; }
        public ServiceItem ServiceItems { get; set; }
        public TaxType TaxType { get; set; }
        public decimal TotalCGSTAmount { get; set; }
        public decimal TotalSGSTAmount { get; set; }
        public decimal TotalIGSTAmount { get; set; }
    }
    public class CustomerInvoiceDetail
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
        public decimal Total { get; set; }
        public decimal TaxableAmount { get; set; }
        public Guid PackingSlipDetailID { get; set; }
        public Guid CustomerInvoiceDetailLinkID { get; set; }
        public decimal QuantityCheck { get; set; }
        public decimal WeightCheck { get; set; }

        public Product Product { get; set; }
        public Guid ServiceItemID { get; set; }
        public string ServiceName { get; set; }

    }

    public class CustomerInvoiceDetailLink
    {
        public Guid ID { get; set; }
        public Guid PackingSlipDetailID { get; set; }
        public Guid CustomerInvoiceDetailID { get; set; }
        public decimal Quantity  { get; set; }
        public decimal Weight { get; set; }

    }

    public class CustomerInvoiceAdvanceSearch
    {
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public Guid CustomerID { get; set; }
        public string InvoiceType { get; set; }
    }

    public class CustomerInvoiceMailPreview
    { 
        public string SentToEmails { get; set; }
        public string MailBody { get; set; }
        public bool Flag { get; set; }
        public CustomerInvoice CustomerInvoice { get; set; }
    }
}
