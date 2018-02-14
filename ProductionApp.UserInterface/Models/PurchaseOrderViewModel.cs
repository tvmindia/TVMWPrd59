using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Models
{
    
        public class PurchaseOrderHeaderViewModel
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
            public CommonViewModel Common { get; set; }
            public string PurchaseOrderDateFormatted { get; set; }
            public string PurchaseOrderIssuedDateFormatted { get; set; }
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
            public string Supplier { get; set; }
            [Display(Name = "Status")]
            public List<SelectListItem> Status { get; set; }
    }
    
}