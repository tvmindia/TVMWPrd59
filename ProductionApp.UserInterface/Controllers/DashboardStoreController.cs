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
            return PartialView("_MyApprovals");
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
            data.Color = "bg-fuchsia";
            data.URL = "localhost/dashboard/";
            data.MaterialReceiptList = Mapper.Map<List<MaterialReceipt>, List<MaterialReceiptViewModel>>(_dashboardStoreBusiness.GetRecentMaterialReceiptSummary());
            return PartialView("_ReceiptSummary",data);
        }
        
        public ActionResult ReOrderAlert()
        {
            return PartialView("_ReOrderAlert");
        }
    }
}