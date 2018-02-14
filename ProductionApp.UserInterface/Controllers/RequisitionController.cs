using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Controllers
{
    public class RequisitionController : Controller
    {
        // GET: Requisitions
        public ActionResult ViewRequisition(string code)
        {
            ViewBag.SysModuleCode = code;
            return View();
        }

        public ActionResult NewRequisition(string code)
        {
            ViewBag.SysModuleCode = code;
            return View();
        }

        public ActionResult RequisitionApproval(string code)
        {
            ViewBag.SysModuleCode = code;
            return View();
        }
    }
}