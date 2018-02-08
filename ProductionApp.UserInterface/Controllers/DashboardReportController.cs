using ProductionApp.UserInterface.Models;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Controllers
{
    public class DashboardReportController : Controller
    {
        // GET: Inventory
        public ActionResult Index(AMCSysModuleViewModel sysModuleVM)
        {
            ViewBag.SysModuleObj = sysModuleVM;
            return View();
        }
    }
}