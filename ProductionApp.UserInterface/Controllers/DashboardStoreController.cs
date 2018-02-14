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
            data.URL = "localhost:23589";
            data.MaterialIssueHeaderList = Mapper.Map<List<MaterialIssueHeader>,List<MaterialIssueHeaderViewModel>>(_dashboardStoreBusiness.GetRecentIssueSummary());
            return PartialView("_IssueSummary",data);
        }

        public ActionResult ReceiptSummary()
        {
            return PartialView("_ReceiptSummary");
        }

        public ActionResult RecentMRNLink()
        {
            return PartialView("_RecentMRNLink");
        }

        public ActionResult ReOrderAlert()
        {
            return PartialView("_ReOrderAlert");
        }
    }
}