using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Services
{
    public class DocumentTypeBusiness: IDocumentTypeBusiness
    {
        private IDocumentTypeRepository _documentTypeRepository;

        public DocumentTypeBusiness(IDocumentTypeRepository documentTypeRepository)
        {
            _documentTypeRepository = documentTypeRepository;
        }

        public List<DocumentType> GetDocumentTypeForSelectList()
        {
            return _documentTypeRepository.GetDocumentTypeForSelectList();
        }
    }
}
