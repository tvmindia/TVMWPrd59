using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Controllers
{
    public class MaterialReceiptController : Controller
    {
        // GET: MaterialReceipt
        public ActionResult Index(string code)
        {
            ViewBag.SysModuleCode = code;
            return View();
        }
    }
}