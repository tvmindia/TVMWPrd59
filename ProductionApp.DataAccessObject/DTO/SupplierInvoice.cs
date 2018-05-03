using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    
   public class SupplierInvoice
    {
        public Guid ID { get; set; }
        public string InvoiceNo { get; set; }
        public Guid SupplierID { get; set; }
        public string PaymentTermCode { get; set; }
        public string InvoiceDateFormatted { get; set; }
        public string PaymentDueDateFormatted { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public decimal Discount { get; set; }
        public string GeneralNotes { get; set; }
        public string AccountCode { get; set; }
        public Guid PurchaseOrderID { get; set; }
        public string PurchaseOrderNo { get; set; }

        //Additional Reference
        public bool IsUpdate { get; set; }
        public Guid hdnFileID { get; set; }
        public int TotalCount { get; set; }
        public string DetailJSON { get; set; }
        public string DetailXML { get; set; }
        public int FilteredCount { get; set; }
        public decimal TotalTaxableAmount { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal PaymentReceived { get; set; }
        public decimal Balance { get; set; }
        public Common Common { get; set; }
        public Supplier Supplier { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }
        public SupplierPayment SupplierPayment { get; set; }
        public SupplierInvoiceDetail SupplierInvoiceDetail { get; set; }
        public List<SupplierInvoiceDetail> SupplierInvoiceDetailList { get; set; }
    }
    public class SupplierInvoiceDetail 
    {
        public Guid ID { get; set; }
        public Guid SupplierInvoiceID { get; set; }
        public Guid MaterialID { get; set; }
        public decimal Rate { get; set; }
        public decimal Quantity { get; set; }
        public string UnitCode { get; set; }
        public decimal TradeDiscountPerc { get; set; }
        public decimal TradeDiscountAmount { get; set; }
        public string TaxTypeCode { get; set; }
        public decimal CGSTPerc { get; set; }
        public decimal SGSTPerc { get; set; }
        public decimal IGSTPerc { get; set; }

        //Additional Reference
        public decimal TaxAmount { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialTypeDesc { get; set; }
        public string MaterialDesc { get; set; }
        public string TaxTypeDescription { get; set; }
        public decimal Total { get; set; }
        public Common  Common { get; set; }
    }

    public class SupplierInvoiceAdvanceSearch
    {       
        public string SearchTerm { get; set; }
        public DataTablePaging  DataTablePaging { get; set; } 
        public string FromDate { get; set; } 
        public string ToDate { get; set; }  
        public Guid SupplierID { get; set; }
    }
}
