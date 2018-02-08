using ProductionApp.UserInterface.Models;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Controllers
{
    public class DashboardProductionController : Controller
    {
        // GET: Production
        public ActionResult Index(AMCSysModuleViewModel sysModuleVM)
        {
            ViewBag.SysModuleObj = sysModuleVM;
            return View();
        }
    }
}