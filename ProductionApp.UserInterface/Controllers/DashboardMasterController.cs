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
    public class DashboardMasterController : Controller
    {
        // GET: DashboardMaster
        private IMastersCountBusiness _mastersCountBusiness;

        public DashboardMasterController(IMastersCountBusiness mastersCountBusiness)
        {
            _mastersCountBusiness = mastersCountBusiness;
        }

        [AuthSecurityFilter(ProjectObject = "DashboardMaster", Mode = "R")]
        public ActionResult Index(string Code)
        {
            ViewBag.SysModuleCode = Code;
            return View();
        }

        #region RecentMastersSummary
        [AuthSecurityFilter(ProjectObject = "DashboardMaster", Mode = "R")]
        public ActionResult RecentMastersSummary()
        {
            MastersCountViewModel MastersCount = new MastersCountViewModel();
            MastersCount.MasterCountList = Mapper.Map<List<MastersCount>, List<MastersCountViewModel>>(_mastersCountBusiness.GetRecentMastersCount());
            return PartialView("_RecentMastersSummary", MastersCount);
        }
        #endregion RecentMastersSummary
    }
}