using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
   public class PurchaseOrder
    {
       public Guid ID { get; set; }
       public string PurchaseOrderNo { get; set; }
       public DateTime PurchaseOrderDate { get; set; }
       public DateTime PurchaseOrderIssuedDate { get; set; }
       public Guid SupplierID { get; set; }
       public string PurchaseOrderTitle { get; set; }
       public string MailingAddress { get; set; }
       public string ShippingAddress { get; set; }
       public decimal Discount { get; set; }
       public string GeneralNotes { get; set; }
       public string PurchaseOrderStatus { get; set; }
       public string MailBodyHeader { get; set; }
       public string MailBodyFooter { get; set; }
       public bool EmailSentYN { get; set; }
       public Guid LatestApprovalID { get; set; }
       public int LatestApprovalStatus { get; set; }
       public bool IsFinalApproved { get; set; }
       //additional properties
       public int TotalCount { get; set; }
       public int FilteredCount { get; set; }
       public string Supplier { get; set; }
       public Common Common { get; set; }
       public string PurchaseOrderDateFormatted { get; set; }
       public string PurchaseOrderIssuedDateFormatted { get; set; }
       public PurchaseOrderDetail PurchaseOrderDetail { get; set; }
       public decimal GrossAmount { get; set; }

    }
    public class PurchaseOrderDetail
    {
        public Guid ID { get; set; }
        public Guid PurchaseOrderID { get; set; }
        public Guid MaterialID { get; set; }
        public string MaterialDesc { get; set; }
        public string UnitCode { get; set; }
        public decimal Qty { get; set; }
        public decimal Rate { get; set; }
        public string TaxType { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal SGSTAmt { get; set; }
        public decimal IGSTAmt { get; set;  }
    }
    public class PurchaseOrderOtherCharges
    {
        public Guid ID { get; set; }
        public Guid PurchaseOrderID { get; set; }
        public string Description { get; set; }
        public string TaxType { get; set; }
        public decimal Amount { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal SGSTAmt { get; set; }
        public decimal IGSTAmt { get; set; }
    }
    public class PurchaseOrderAdvanceSearch
    {
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public Supplier Supplier { get; set; }
        public string Status { get; set; }
    }
}
