using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserInterface.Models;

namespace ProductionApp.UserInterface.Models
{
    public class AMCSysReportViewModel
    {
        public Guid ID { get; set; }
        public string ReportName { get; set; }
        public string ReportDescription { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string ReportGroup { get; set; }
        public int GroupOrder { get; set; }
        public string SPName { get; set; }
        public string SQL { get; set; }
        public int ReportOrder { get; set; }
        public List<AMCSysReportViewModel> AMCSysReportList { get; set; }
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
    }

    public class RequisitionSummaryReportViewModel
    {
        public List<RequisitionViewModel> RequisitionList { get; set; }
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "From Date")]
        public string FromDate { get; set; }
        [Display(Name = "To Date")]
        public string ToDate { get; set; }
        [Display(Name = "Requisition Status")]
        public string ReqStatus { get; set; }
        [Display(Name = "Requisition By")]
        public string RequisitionBy { get; set; }
        public Guid EmployeeID { get; set; }
        public EmployeeViewModel Employee { get; set; }
        [Display(Name = "Quick Filter")]
        public string DateFilter { get; set; }
    }

    public class RequisitionDetailReportViewModel
    {
        //public List<RequisitionViewModel> RequisitionList { get; set; }
        //public List<RequisitionDetailReportViewModel> RequisitionDetailList { get; set; }
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "From Date")]
        public string FromDate { get; set; }
        [Display(Name = "To Date")]
        public string ToDate { get; set; }
        [Display(Name = "Requisition Status")]
        public string ReqStatus { get; set; }
        [Display(Name = "Requisition By")]
        public string RequisitionBy { get; set; }
        public Guid EmployeeID { get; set; }
        public EmployeeViewModel Employee { get; set; }
        [Display(Name = "Quick Filter")]
        public string DateFilter { get; set; }
        [Display(Name = "Material")]
        public Guid MaterialID { get; set; }
        public MaterialViewModel Material { get; set; }
        [Display(Name = "Delivery Status")]
        public string DeliveryStatus { get; set; }

        public Guid ID { get; set; }
        public Guid ReqID { get; set; }
        public string Description { get; set; }
        public string RequestedQty { get; set; }
        public string ApproximateRate { get; set; }
        public Decimal Discount { get; set; }
        public string POQty { get; set; }
        public string OrderedQty { get; set; }
        public string ReceivedQty { get; set; }

        public string ReqNo { get; set; }
        public CommonViewModel Common { get; set; }
        public string Title { get; set; }
        public DateTime ReqDate { get; set; }
        public string ReqDateFormatted { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public List<RequisitionDetailReportViewModel> RequisitionDetailReportList { get; set; }
    }

    public class PurchaseSummaryReportViewModel
    { 
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "From Date")]
        public string FromDate { get; set; }
        [Display(Name = "To Date")]
        public string ToDate { get; set; }        
        public SupplierViewModel Supplier { get; set; }
        [Display(Name = "Supplier")]
        public Guid SupplierID { get; set; }
        [Display(Name = "PO Status")]
        public string Status { get; set; }
        [Display(Name = "Quick Filter")]
        public string DateFilter { get; set; }
        [Display(Name = "Email Sent(Y/N)")]
        public string EmailedYN { get; set; }
        public List<PurchaseOrderViewModel> PurchaseOrderList { get; set; }
    }





}