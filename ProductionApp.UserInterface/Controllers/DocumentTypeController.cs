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
            documentTypeVM.DocumentTypeCode = documentTypeVM.Code;
            documentTypeVM.SelectList = _documentTypeBusiness.GetDocumentTypeForSelectList();
            return PartialView("_DocumentTypeDropdown", documentTypeVM);
        }
        #endregion DocumentTypeDropdown

    }
}