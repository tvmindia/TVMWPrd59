using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductionApp.DataAccessObject.DTO;

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

        public List<DocumentApproval> GetAllDocumentsPendingForApprovals(DocumentApprovalAdvanceSearch documentApprovalAdvanceSearch) {

            return _documentApprovalRepository.GetAllDocumentsPendingForApprovals(documentApprovalAdvanceSearch);


        }
    }
}
