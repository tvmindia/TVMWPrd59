using ProductionApp.BusinessService.Contracts;
using ProductionApp.UserInterface.Models;
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
        public DashboardStoreController(IMaterialReceiptBusiness materialReceiptBusiness)
        {
            _materialReceiptBusiness = materialReceiptBusiness;
        }
        #endregion Constructor Injection

        // GET: HR
        public ActionResult Index(string Code)
        {
            ViewBag.SysModuleCode = Code;
            return View();
        }

        public ActionResult MyApprovals()
        {
            MaterialStockAdjViewModel MaterialStockAdj = new MaterialStockAdjViewModel();
            MaterialStockAdj.MaterialStockAdjList = new List<MaterialStockAdjViewModel>();
            return PartialView("_MyApprovals",MaterialStockAdj);
        }

        public ActionResult IssueSummary()
        {
            MaterialIssueViewModel MaterialIssue = new MaterialIssueViewModel();
            MaterialIssue.MaterialIssueList = new List<MaterialIssueViewModel>();//Mapper.Map<List<MaterialIssue>,List<MaterialIssueViewModel>>(_dashboardStoreBusiness.GetRecentIssueSummary());
            return PartialView("_IssueSummary",MaterialIssue);
        }

        public ActionResult ReceiptSummary()
        {
            MaterialReceiptViewModel MaterialReceipt = new MaterialReceiptViewModel();
            MaterialReceipt.MaterialReceiptList = Mapper.Map<List<MaterialReceipt>, List<MaterialReceiptViewModel>>(_materialReceiptBusiness.GetRecentMaterialReceiptSummary());
            return PartialView("_ReceiptSummary",MaterialReceipt);
        }
        
        public ActionResult ReOrderAlert()
        {
            //The view model is given for temporary purpose need to be made later
            MaterialReceiptViewModel MaterialReceipt = new MaterialReceiptViewModel();
            MaterialReceipt.MaterialReceiptList = new List<MaterialReceiptViewModel>();
            return PartialView("_ReOrderAlert",MaterialReceipt);
        }
    }
}