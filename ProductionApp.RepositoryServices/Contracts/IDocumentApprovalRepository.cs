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
        object ApproveDocumentInsert(Guid ApprovalLogID, Guid DocumentID, string DocumentTypeCode);
    }
}
