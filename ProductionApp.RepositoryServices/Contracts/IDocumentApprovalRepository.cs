using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.RepositoryServices.Contracts
{
    public interface IDocumentApprovalRepository
    {
        List<ApprovalHistory> GetApprovalHistory(Guid DocumentID, string DocumentTypeCode);

        List<DocumentApproval> GetAllDocumentsPendingForApprovals(DocumentApprovalAdvanceSearch documentApprovalAdvanceSearch);
        DataTable GetDocumentSummary(Guid DocumentID, string DocumentTypeCode);
        object ApproveDocument(Guid ApprovalLogID, Guid DocumentID, string DocumentTypeCode);
        object RejectDocument(Guid ApprovalLogID, Guid DocumentID, string DocumentTypeCode, string Remarks);
        object ValidateDocumentsApprovalPermission(string LoginName, Guid DocumentID, string DocumentTypeCode);
        List<DocumentApprover> GetApproversByDocType(string docType);

        object SendDocForApproval(Guid documentID, string documentTypeCode, string approvers, string createdBy, DateTime createdDate);
        object ReSendDocForApproval(Guid documentID, string documentTypeCode, Guid latestApprovalID, string createdBy, DateTime createdDate);
        DocumentApprovalMailDetail GetApprovalMailDetails(Guid DocumentID, string DocumentTypeCode);
        List<DocumentApproval> GetStockAdjApprovalSummary();
        List<DocumentApproval> GetAllApprovalHistory(DocumentApprovalAdvanceSearch documentApprovalAdvanceSearch);
    }
}
