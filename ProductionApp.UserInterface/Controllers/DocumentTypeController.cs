using AutoMapper;
using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.UserInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Controllers
{
    public class DocumentTypeController : Controller
    {
        AppConst _appConst = new AppConst();
        private Common _common = new Common();
        private IDocumentTypeBusiness _documentTypeBusiness;

        #region Constructor Injection
        public DocumentTypeController(IDocumentTypeBusiness documentTypeBusiness)
        {
            _documentTypeBusiness = documentTypeBusiness;
        }
        #endregion Constructor Injection
        // GET: DocumentType
        public ActionResult Index()
        {
            return View();
        }

        #region DocumentTypeDropdown
        public ActionResult DocumentTypeDropdown(DocumentTypeViewModel documentTypeVM)
        {
            //UnitViewModel unitVM = new UnitViewModel();
            //documentTypeVM.UnitCode = documentTypeVM.Code;
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            documentTypeVM.SelectList = new List<SelectListItem>();
            List<DocumentTypeViewModel> documentTypeList = Mapper.Map<List<DocumentType>, List<DocumentTypeViewModel>>(_documentTypeBusiness.GetDocumentTypeForSelectList());
            if (documentTypeList != null)
                foreach (DocumentTypeViewModel documentType in documentTypeList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = documentType.Description,
                        Value = documentType.Type,
                        Selected = false
                    });
                }
            documentTypeVM.SelectList = selectListItem;
            return PartialView("_DocumentTypeDropdown", documentTypeVM);
        }
        #endregion DocumentTypeDropdown

    }
}