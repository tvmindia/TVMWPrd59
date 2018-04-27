using ProductionApp.BusinessService.Contracts;
using ProductionApp.UserInterface.Models;
using ProductionApp.UserInterface.SecurityFilter;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProductionApp.DataAccessObject.DTO;

namespace ProductionApp.UserInterface.Controllers
{
    public class DashboardStoreController : Controller
    {
        #region Constructor Injection
        IMaterialReceiptBusiness _materialReceiptBusiness;
        IIssueToProductionBusiness _issueToProductionBusiness;
        IDocumentApprovalBusiness _documentApprovalBusiness;
        IMaterialBusiness _materialBusiness;
        public DashboardStoreController(IMaterialReceiptBusiness materialReceiptBusiness, IIssueToProductionBusiness issueToProductionBusiness,IDocumentApprovalBusiness documentApprovalBusiness, IMaterialBusiness materialBusiness)
        {
            _materialReceiptBusiness = materialReceiptBusiness;
            _issueToProductionBusiness = issueToProductionBusiness;
            _documentApprovalBusiness = documentApprovalBusiness;
            _materialBusiness = materialBusiness;
        }
        #endregion Constructor Injection

        // GET: HR    
        [AuthSecurityFilter(ProjectObject = "DashboardStore", Mode = "R")]
        public ActionResult Index(string Code)
        {
            ViewBag.SysModuleCode = Code;
            return View();
        }

        #region MyApprovals
        [AuthSecurityFilter(ProjectObject = "DashboardStore", Mode = "R")]
        public ActionResult MyApprovals()
        {
            DocumentApprovalViewModel DocumentApproval = new DocumentApprovalViewModel();
            DocumentApproval.DocumentApprovalList = Mapper.Map<List<DocumentApproval>, List<DocumentApprovalViewModel>>(_documentApprovalBusiness.GetStockAdjApprovalSummary());
            return PartialView("_MyApprovals", DocumentApproval);
        }
        #endregion MyApprovals

        #region IssueSummary
        [AuthSecurityFilter(ProjectObject = "DashboardStore", Mode = "R")]
        public ActionResult IssueSummary()
        {
            MaterialIssueViewModel MaterialIssue = new MaterialIssueViewModel();
            MaterialIssue.MaterialIssueList = Mapper.Map<List<MaterialIssue>,List<MaterialIssueViewModel>>(_issueToProductionBusiness.GetRecentMaterialIssueSummary());
            return PartialView("_IssueSummary",MaterialIssue);
        }
        #endregion IssueSummary

        #region ReceiptSummary
        [AuthSecurityFilter(ProjectObject = "DashboardStore", Mode = "R")]
        public ActionResult ReceiptSummary()
        {
            MaterialReceiptViewModel MaterialReceipt = new MaterialReceiptViewModel();
            MaterialReceipt.MaterialReceiptList = Mapper.Map<List<MaterialReceipt>, List<MaterialReceiptViewModel>>(_materialReceiptBusiness.GetRecentMaterialReceiptSummary());
            return PartialView("_ReceiptSummary",MaterialReceipt);
        }
        #endregion ReceiptSummary

        #region ReOrderAlert
        [AuthSecurityFilter(ProjectObject = "DashboardStore", Mode = "R")]
        public ActionResult ReOrderAlert()
        {
            MaterialViewModel MaterialVM = new MaterialViewModel();
            MaterialVM.MaterialList = Mapper.Map<List<Material>, List<MaterialViewModel>>(_materialBusiness.GetMaterialListForReorderAlert());
            return PartialView("_ReOrderAlert", MaterialVM);
        }
        #endregion ReOrderAlert
    }
}