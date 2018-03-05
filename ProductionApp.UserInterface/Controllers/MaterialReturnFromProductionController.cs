using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Controllers
{
    public class MaterialReturnFromProductionController : Controller
    {
        // GET: MaterialReturnFromProduction
        public ActionResult Index()
        {
            return View();
        }

        #region ListReceiveFromProduction
        public ActionResult ListReceiveFromProduction(string code)
        {
            ViewBag.SysModuleCode = code;
            return View();
        }
        #endregion ListReceiveFromProduction

        #region AddReceiveFromProduction
        public ActionResult AddReceiveFromProduction(string code)
        {
            ViewBag.SysModuleCode = code;
            return View();
        }
        #endregion AddReceiveFromProduction
    }
}