using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Controllers
{
    public class MaterialStockAdjController : Controller
    {
        // GET: MaterialStockAdj
        public ActionResult Index()
        {
            return View();
        }

        #region ListStockAdjustment
        public ActionResult ListStockAdjustment(string code)
        {
            ViewBag.SysModule = code;
            return View();

        }
        #endregion ListStockAdjustment

        #region AddStockAdjustment
        public ActionResult AddStockAdjustment(string code)
        {
            ViewBag.SysModule = code;
            return View();
        }
        #endregion AddStockAdjustment
    }
}