using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductionApp.UserInterface.Models
{
    public class RequisitionViewModel
    {
        public Guid ID { get; set; }
        [Display(Name = "Requisition #")]
        public string ReqNo { get; set; }
        [Display(Name = "Title")]
        [Required(ErrorMessage = "Title required")]
        public string Title { get; set; }
        [Display(Name = "Requisition Date")]
        [Required(ErrorMessage = "Requisition Date required")]
        public DateTime ReqDate { get; set; }
        [Display(Name = "Requisition Date")]
        public string ReqDateFormatted { get; set; }
        [Display(Name = "Requisition Status")]
        public string ReqStatus { get; set; }
        [Display(Name = "Requisition By")]
        public string RequisitionBy { get; set; }
        public Guid EmployeeID { get; set; }
        public Guid LatestApprovalID { get; set; }
        public int LatestApprovalStatus { get; set; }
        public bool IsFinalApproved { get; set; }
        public bool IsUpdate { get; set; }

        //additional properties
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public string DetailJSON { get; set; }
        public DateTime ApprovalDate { get; set; }
        public string ApprovalDateFormatted { get; set; }
        public CommonViewModel Common { get; set; }
        public RequisitionDetailViewModel RequisitionDetail { get; set; }
        public List<RequisitionDetailViewModel> RequisitionDetailList { get; set; }
    }

    public class RequisitionDetailViewModel 
    {
        public Guid ID { get; set; }
        public Guid ReqID { get; set; }
        [Display(Name = "Material")]
        public Guid MaterialID { get; set; }
        public string Description { get; set; }
        [Display(Name = "Requested Quantity")]
        public string RequestedQty { get; set; }
      //  [Display(Name = "Current Stock")]
       // public string CurrentStock { get; set; }
       // public string MaterialCode { get; set; }
        [Display(Name = "Approximate Rate")]
        public string ApproximateRate { get; set; }
        public string POQty { get; set; }
        public string OrderedQty { get; set; }
        public string ReqNo { get; set; }
        public MaterialViewModel RawMaterial { get; set; }
        public CommonViewModel Common { get; set; }
    }

    public class RequisitionAdvanceSearchViewModel
    {

        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }

    }
}