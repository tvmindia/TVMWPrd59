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
        [Required(ErrorMessage = "Requisition Date required")]
        public string ReqDateFormatted { get; set; }
        [Display(Name = "Requisition Status")]
        public string ReqStatus { get; set; }
        [Display(Name = "Requisition By")]
        public string RequisitionBy { get; set; }
        [Display(Name = "Required Date")]
        public DateTime RequiredDate { get; set; }
        public Guid EmployeeID { get; set; }
        public Guid LatestApprovalID { get; set; }
        public string ApprovalStatus { get; set; }
        public int LatestApprovalStatus { get; set; }
        public bool IsFinalApproved { get; set; }
        public bool IsUpdate { get; set; }

        //additional properties
        public Guid hdnFileID { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public string DetailJSON { get; set; }
        public DateTime ApprovalDate { get; set; }
        public string ApprovalDateFormatted { get; set; }
        public CommonViewModel Common { get; set; }
        public RequisitionDetailViewModel RequisitionDetail { get; set; }
        public List<RequisitionDetailViewModel> RequisitionDetailList { get; set; }
        public List<RequisitionViewModel> RequisitionList { get; set; }
        public string BaseURL { get; set; }   
        public RequisitionAdvanceSearchViewModel RequisitionAdvanceSearch { get; set; }
        public string ReqAmount { get; set; }
        
        public string RequiredDateFormatted { get; set; }
        public string RequisitionNo { get; set; }
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
        public Decimal Discount { get; set; }
        public string POQty { get; set; }
        public string OrderedQty { get; set; }
        public string ReqNo { get; set; }
        public MaterialViewModel Material { get; set; }
        public CommonViewModel Common { get; set; }       
    }

    public class RequisitionAdvanceSearchViewModel
    {

        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }

        [Display(Name = "FromDate")]
        public string FromDate { get; set; }
        [Display(Name = "ToDate")]
        public string ToDate { get; set; }
        [Display(Name = "Requisition Status")]
        public string ReqStatus { get; set; }
        [Display(Name = "Requisition By")]
        public string RequisitionBy { get; set; }
        public Guid EmployeeID { get; set; }
        public EmployeeViewModel Employee { get; set; }
        [Display(Name = "Filter")]
        public string DateFilter { get; set; }

    }
}