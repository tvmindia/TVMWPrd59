using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    public class DocumentApproval
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
            public string LatestDocumentStatus { get; set; }
            public Boolean IsNextApprover { get; set; }

        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
    }
    public class ApprovalHistory
    {
        public Guid ApproverID { get; set; }
        public string ApproverName { get; set; }
        public string ApproverLevel { get; set; }
        public string ApprovalDate { get; set; }
        public string ApprovalStatus { get; set; }
        public string Remarks { get; set; }
    }
    public class DocumentSummary
    {
        public object DataTable { get; set; }
        public Guid DocumentID { get; set; }
        public string DocumentTypeCode { get; set; }
    }

    public class DocumentApprovalAdvanceSearch
    {
       
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }     
        public string FromDate { get; set; }      
        public string ToDate { get; set; }       
        public DocumentType DocumentType { get; set; }
        public Boolean ShowAll { get; set; } 
        public String LoginName { get; set; }

    }

}
