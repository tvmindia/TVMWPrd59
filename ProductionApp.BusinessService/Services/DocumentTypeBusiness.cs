using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProductionApp.BusinessService.Services
{
    public class DocumentTypeBusiness: IDocumentTypeBusiness
    {
        private IDocumentTypeRepository _documentTypeRepository;

        public DocumentTypeBusiness(IDocumentTypeRepository documentTypeRepository)
        {
            _documentTypeRepository = documentTypeRepository;
        }
        public List<SelectListItem> GetDocumentTypeForSelectList()
        {
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            List<DocumentType> documentTypeList = _documentTypeRepository.GetDocumentTypeForSelectList();
            if (documentTypeList != null)
                foreach (DocumentType productCategory in documentTypeList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = productCategory.Description,
                        Value = productCategory.Code,
                        Selected = false
                    });
                }
            return selectListItem;
        }
    }
}
