using ProductionApp.UserInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Controllers
{
    public class DashboardAccountsController : Controller
    {
        // GET: Dashboard
        public ActionResult Index(AMCSysModuleViewModel sysModuleVM)
        {
            ViewBag.SysModuleObj = sysModuleVM;
            return View();
        }
    }
}