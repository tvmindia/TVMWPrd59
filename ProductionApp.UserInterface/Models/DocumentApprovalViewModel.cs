using System;
using System.Collections.Generic;
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
}