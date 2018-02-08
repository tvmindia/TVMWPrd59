using ProductionApp.UserInterface.Models;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Controllers
{
    public class DashboardSettingsController : Controller
    {
        // GET: DashboardSettings
        public ActionResult Index(AMCSysModuleViewModel sysModuleVM)
        {
            ViewBag.SysModuleObj = sysModuleVM;
            return View();
        }
    }
}