using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Models
{
    
        public class PurchaseOrderViewModel
        {
            public Guid ID { get; set; }
            [Display(Name = "PO Number")]
            public string PurchaseOrderNo { get; set; }
            [Display(Name = "PO Dtae")]
            public DateTime PurchaseOrderDate { get; set; }
            [Display(Name = "PO Issued Dtae")]
            public DateTime PurchaseOrderIssuedDate { get; set; }
            public Guid SupplierID { get; set; }
            [Display(Name = "Title")]
            public string PurchaseOrderTitle { get; set; }
            [Display(Name = "Mailing Address")]
            [DataType(DataType.MultilineText)]
            public string MailingAddress { get; set; }
            [Display(Name = "Shipping Address")]
            [DataType(DataType.MultilineText)]
            public string ShippingAddress { get; set; }
            public decimal Discount { get; set; }
            [DataType(DataType.MultilineText)]
            public string GeneralNotes { get; set; }
            [Display(Name = "PO Status")]
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
            public CommonViewModel Common { get; set; }
            public string PurchaseOrderDateFormatted { get; set; }
            public string PurchaseOrderIssuedDateFormatted { get; set; }
            public PurchaseOrderDetailViewModel PurchaseOrderDetail { get; set; }
            [Display(Name = "Gross Amount")]
            public decimal GrossAmount { get; set; }
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
            public string TaxType { get; set; }
            public decimal CGSTAmt { get; set; }
            public decimal SGSTAmt { get; set; }
            public decimal IGSTAmt { get; set; }
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
            public SupplierViewModel Supplier { get; set; }
            [Display(Name = "Status")]
            public string Status { get; set; }
            
    }
    
}