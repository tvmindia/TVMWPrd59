using ProductionApp.UserInterface.Models;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Controllers
{
    public class DashboardMasterController : Controller
    {
        // GET: DashboardMaster
        public ActionResult Index(AMCSysModuleViewModel sysModuleVM)
        {
            ViewBag.SysModuleObj = sysModuleVM;
            return View();
        }
    }
}