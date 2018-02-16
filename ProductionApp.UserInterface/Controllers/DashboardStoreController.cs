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
        IDashboardStoreBusiness _dashboardStoreBusiness;
        public DashboardStoreController(IDashboardStoreBusiness dashboardStoreBusiness)
        {
            _dashboardStoreBusiness = dashboardStoreBusiness;
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
            MyApprovalsViewModel data = new MyApprovalsViewModel();
            data.Title = "My Stock Approvals";
            data.Color = "bg-green";
            data.URL = "localhost/dashboard/";
            data.MaterialStockAdjList = Mapper.Map<List<MaterialStockAdj>, List<MaterialStockAdjViewModel>>(_dashboardStoreBusiness.GetRecentStockAdjustment());
            return PartialView("_MyApprovals",data);
        }

        public ActionResult IssueSummary()
        {
            IssueSummaryViewModel data = new IssueSummaryViewModel();
            data.Title = "Issue Summary";
            data.Color = "bg-aqua";
            data.URL = "localhost/dashboard/";
            data.MaterialIssueList = Mapper.Map<List<MaterialIssue>,List<MaterialIssueViewModel>>(_dashboardStoreBusiness.GetRecentIssueSummary());
            return PartialView("_IssueSummary",data);
        }

        public ActionResult ReceiptSummary()
        {
            ReceiptSummaryViewModel data = new ReceiptSummaryViewModel();
            data.Title = "Receipt Summary";
            data.Color = "bg-purple";
            data.URL = "localhost/dashboard/";
            data.MaterialReceiptList = Mapper.Map<List<MaterialReceipt>, List<MaterialReceiptViewModel>>(_dashboardStoreBusiness.GetRecentMaterialReceiptSummary());
            return PartialView("_ReceiptSummary",data);
        }
        
        public ActionResult ReOrderAlert()
        {//The view model is given for temporary purpose need to be made later
            ReceiptSummaryViewModel data = new ReceiptSummaryViewModel();
            data.Title = "Reorder Alert";
            data.Color = "bg-maroon";
            data.URL = "localhost/dashboard/";
            data.MaterialReceiptList = new List<MaterialReceiptViewModel>();
            return PartialView("_ReOrderAlert",data);
        }
    }
}