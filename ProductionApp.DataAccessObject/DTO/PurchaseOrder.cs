﻿using System;
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
       public string EmailSentYN { get; set; }
       public Guid LatestApprovalID { get; set; }
       public int LatestApprovalStatus { get; set; }
       public bool IsFinalApproved { get; set; }
       public string SubscriberEmail { get; set; }
        //additional properties
        public string ApprovalStatus { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyName { get; set; }
        public string LogoURL { get; set; }
        public Guid hdnFileID { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public string Supplier { get; set; }
        public Common Common { get; set; }
        public string PurchaseOrderDateFormatted { get; set; }
        public string PurchaseOrderIssuedDateFormatted { get; set; }
        public PurchaseOrderDetail PurchaseOrderDetail { get; set; }
        public decimal GrossAmount { get; set; }
        public List<PurchaseOrderDetail> PODDetail { get; set; }
        public List<PurchaseOrderDetailRequisitionLink> PODDetailLink { get; set; }
        public PurchaseOrderMailPreview PurchaseOrderMailPreview { get; set; }
        public string PODDetailXML { get; set; }
        public string PODDetailLinkXML { get; set; }
        public List<PurchaseOrder> PurchaseOrderList { get; set; }
        public string BaseURL { get; set; }
        public string PONo { get; set; }
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
        public string TaxTypeCode { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal SGSTAmt { get; set; }
        public decimal IGSTAmt { get; set;  }
        public decimal Discount { get; set; }
        public decimal CGSTPerc { get; set; }
        public decimal IGSTPerc { get; set; }
        public decimal SGSTPerc { get; set; }
        //additional prop
        public string MaterialCode { get; set; }
        public string MaterialTypeDesc { get; set; }
        public decimal Amount { get; set; }
        public decimal PrevRcvQty { get; set; }
        public decimal PrevRcvQtyInKG { get; set; }
        public decimal PrevInvQty { get; set; }
        public decimal POQty { get; set; }
        public RequisitionDetail RequisitionDetail { get; set; }
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
        public Guid SupplierID { get; set; }
        public string Status { get; set; }
    }
    public class PurchaseOrderDetailRequisitionLink
    {
        public Guid ID { get; set; }
        public Guid PurchaseOrderDetailID { get; set; }
        public Guid ReqDetailID { get; set; }
        public decimal PurchaseOrderQty { get; set; }
        public Guid MaterialID { get; set; }
        public string TaxTypeCode { get; set; }
        public decimal CGSTAmt { get; set; }
        public decimal SGSTAmt { get; set; }
        public decimal IGSTAmt { get; set; }
        public decimal Discount { get; set; }
    }
    public class PurchaseOrderMailPreview
    {
        public string SentToEmails { get; set; }
        public string MailBody { get; set; }
        public bool Flag { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }
    }
}
