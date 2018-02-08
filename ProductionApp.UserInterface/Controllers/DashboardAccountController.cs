using ProductionApp.UserInterface.Models;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Controllers
{
    public class DashboardAccountController : Controller
    {
        // GET: Dashboard
        public ActionResult Index(AMCSysModuleViewModel sysModuleVM)
        {
            ViewBag.SysModuleObj = sysModuleVM;
            return View();
        }
    }
}