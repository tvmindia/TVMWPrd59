using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductionApp.DataAccessObject.DTO;
using System.Data;

namespace ProductionApp.BusinessService.Services
{
    public class DocumentApprovalBusiness: IDocumentApprovalBusiness
    {
        private IDocumentApprovalRepository _documentApprovalRepository;
        public DocumentApprovalBusiness(IDocumentApprovalRepository documentApprovalRepository)
        {
            _documentApprovalRepository = documentApprovalRepository;
        }

        public List<ApprovalHistory> GetApprovalHistory(Guid DocumentID, string DocumentTypeCode)
        {
          return  _documentApprovalRepository.GetApprovalHistory(DocumentID, DocumentTypeCode);
        }

        public List<DocumentApproval> GetAllDocumentsPendingForApprovals(DocumentApprovalAdvanceSearch documentApprovalAdvanceSearch)
        {
            return _documentApprovalRepository.GetAllDocumentsPendingForApprovals(documentApprovalAdvanceSearch);
        }

        public DataTable GetDocumentSummary(Guid DocumentID, string DocumentTypeCode)
        {
            return _documentApprovalRepository.GetDocumentSummary(DocumentID, DocumentTypeCode);
        }

        public object ApproveDocument(Guid ApprovalLogID, Guid DocumentID, string DocumentTypeCode)
        {
            return _documentApprovalRepository.ApproveDocument(ApprovalLogID, DocumentID, DocumentTypeCode);
        }

        public object RejectDocument(Guid ApprovalLogID, Guid DocumentID, string DocumentTypeCode, string Remarks)
        {
            return _documentApprovalRepository.RejectDocument(ApprovalLogID, DocumentID, DocumentTypeCode, Remarks);
        }

        public object ValidateDocumentsApprovalPermission(string LoginName, Guid DocumentID, string DocumentTypeCode)
        {
            return _documentApprovalRepository.ValidateDocumentsApprovalPermission(LoginName, DocumentID, DocumentTypeCode);

        }
    }
}
