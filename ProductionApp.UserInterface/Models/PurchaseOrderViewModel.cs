using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserInterface.Models;

namespace ProductionApp.UserInterface.Models
{
    
        public class PurchaseOrderViewModel
        {
            public Guid ID { get; set; }
            [Display(Name = "PO Number")]
            public string PurchaseOrderNo { get; set; }
            [Display(Name = "PO Date")]
            [Required(ErrorMessage = "PO Date required")]
            public DateTime PurchaseOrderDate { get; set; }
            [Display(Name = "PO Issued Date")]
            [Required(ErrorMessage = "PO Issued Date required")]
            public DateTime PurchaseOrderIssuedDate { get; set; }
            public Guid SupplierID { get; set; }
            [Display(Name = "PO Title")]
            [Required(ErrorMessage = "PO Title required")]
            public string PurchaseOrderTitle { get; set; }
            [Display(Name = "Mailing Address")]
            [DataType(DataType.MultilineText)]
            public string MailingAddress { get; set; }
            [Display(Name = "Shipping Address")]
            [DataType(DataType.MultilineText)]
            public string ShippingAddress { get; set; }
            [Display(Name = "Cash Discount")]
            public decimal Discount { get; set; }
            [DataType(DataType.MultilineText)]
            public string GeneralNotes { get; set; }
            [Display(Name = "PO Status")]
            public string PurchaseOrderStatus { get; set; }
            [Display(Name = "Body Header")]
            [DataType(DataType.MultilineText)]
            public string MailBodyHeader { get; set; }
            [Display(Name = "Body Footer")]
            [DataType(DataType.MultilineText)]
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
            [Required(ErrorMessage = "Supplier required")]
            public string Supplier { get; set; }
            public CommonViewModel Common { get; set; }
            public string PurchaseOrderDateFormatted { get; set; }
            public string PurchaseOrderIssuedDateFormatted { get; set; }
            public PurchaseOrderDetailViewModel PurchaseOrderDetail { get; set; }
            [Display(Name = "Gross Amount")]
            public decimal GrossAmount { get; set; }
            [Display(Name = "Total Taxable Amount")]
            public decimal ItemTotal { get; set; }
            [Display(Name = "Total CGST Amount")]
            public decimal CGSTTotal { get; set; }
            [Display(Name = "Total SGST Amount")]
            public decimal SGSTTotal { get; set; }
            [Display(Name = "Total Tax Amount")]
            public decimal TaxTotal { get; set; }
            public List<SelectListItem> SelectList { get; set; }
            public List<PurchaseOrderDetailViewModel> PODDetail { get; set; }
            public List<PurchaseOrderDetailRequisitionLinkViewModel> PODDetailLink { get; set; }
            public PurchaseOrderMailPreviewViewModel PurchaseOrderMailPreview { get; set; }
            public string PODDetailXML { get; set; }
            public string PODDetailLinkXML { get; set; }
            public PDFTools PDFTools { get; set; }
            public List<PurchaseOrderViewModel> PurchaseOrderList { get; set; }
            public string BaseURL { get; set; }
    }
        public class PurchaseOrderDetailViewModel
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
            public decimal IGSTAmt { get; set; }
            public decimal Discount { get; set; }
            //additional prop
            public string MaterialTypeDesc { get; set; }
            public string MaterialCode { get; set; }
            public decimal Amount { get; set; }
            public RequisitionDetailViewModel RequisitionDetail { get; set; }
    }
        public class PurchaseOrderOtherChargesViewModel
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
        public class PurchaseOrderAdvanceSearchViewModel
        {
            [Display(Name = "Search")]
            public string SearchTerm { get; set; }
            public DataTablePagingViewModel DataTablePaging { get; set; }
            [Display(Name = "FromDate")]
            public string FromDate { get; set; }
            [Display(Name = "ToDate")]
            public string ToDate { get; set; }
            [Display(Name = "Supplier")]
            public Guid SupplierID { get; set; }
            public SupplierViewModel Supplier { get; set; }
            [Display(Name = "Status")]
            public string Status { get; set; }
            
    }
        public class PurchaseOrderDetailRequisitionLinkViewModel
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
        public class PurchaseOrderMailPreviewViewModel
        {
           [Display(Name = "Send To")]
           public string SentToEmails { get; set; }
           public string MailBody { get; set; }
           public bool Flag { get; set; }
           public PurchaseOrderViewModel PurchaseOrder { get; set; }
         }
}