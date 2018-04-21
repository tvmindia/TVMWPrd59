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
    public class DashboardMasterController : Controller
    {
        // GET: DashboardMaster
        private IMastersCountBusiness _mastersCountBusiness;

        public DashboardMasterController(IMastersCountBusiness mastersCountBusiness)
        {
            _mastersCountBusiness = mastersCountBusiness;
        }
        public ActionResult Index(string Code)
        {
            ViewBag.SysModuleCode = Code;
            return View();
        }


        public ActionResult RecentMastersSummary()
        {
            MastersCountViewModel MastersCount = new MastersCountViewModel();
            MastersCount.MasterCountList = Mapper.Map<List<MastersCount>, List<MastersCountViewModel>>(_mastersCountBusiness.GetRecentMastersCount());
            return PartialView("_RecentMastersSummary", MastersCount);
        }
    }
}