using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Controllers
{
    public class SalesOrderController : Controller
    {
        // GET: SalesOrder
        public ActionResult AddSalesOrder(string code, Guid? id)
        {
            ViewBag.SysModuleCode = code;

            return View();
        }
        public ActionResult ListSalesOrder(string code)
        {
            ViewBag.SysModuleCode = code;

            return View();
        }
    }
}