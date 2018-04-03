using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Controllers
{
    public class ProductionTrackingController : Controller
    {
        // GET: ProductionTracking
        public ActionResult NewProductionTracking()
        {
            return View();
        }
    }
}