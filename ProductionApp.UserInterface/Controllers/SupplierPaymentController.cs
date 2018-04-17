using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Controllers
{
    public class SupplierPaymentController : Controller
    {
        // GET: SupplierPayment
        public ActionResult ViewSupplierPayment()
        {
            return View();
        }
        public ActionResult NewSupplierPayment()
        {
            return View();
        }
    }
}