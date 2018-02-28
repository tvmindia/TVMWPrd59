using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductionApp.UserInterface.Models
{
    public class DocumentApprovalViewModel
    {
        public string DocumentTypeCode { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public DateTime DocumentDate { get; set; }
        public string DocumentDateFormatted { get; set; }
        public int StatusCode { get; set; }
        public string DocumentStatus { get; set; }
        public int ApproverLevel { get; set; }
        public string Approver { get; set; }
        public string DocumentCreatedBy { get; set; }
        public Guid ApprovalLogID { get; set; }
        public Guid DocumentID { get; set; }
        public Guid UserID { get; set; }
        public Guid ApproverID { get; set; }
        public Guid LastApprovedUserID { get; set; }
        public int LatestDocumentStatus { get; set; }
        public Boolean IsNextApprover { get; set; }
    }

    public class ApprovalHistoryViewModel
    {
        public Guid ApproverID { get; set; }
        public string ApproverName { get; set; }
        public string ApproverLevel { get; set; }
        public string ApprovalDate { get; set; }
        public string ApprovalStatus { get; set; }
        public string Remarks { get; set; }

    }
    public class DocumentSummaryViewModel
    {
        object DataTable { get; set; }
    }

    public class DocumentApprovalAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "Document Date From")]
        public string FromDate { get; set; }
        [Display(Name = "Document Date To")]
        public string ToDate { get; set; }
        [Display(Name = "Document Type")]
        public DocumentTypeViewModel DocumentType { get; set; }
      

    }

}