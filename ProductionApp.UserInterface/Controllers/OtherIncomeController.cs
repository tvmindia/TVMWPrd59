using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Controllers
{
    public class OtherIncomeController : Controller
    {
        // GET: OtherIncome
        public ActionResult ViewOtherIncome()
        {
            return View();
        }

        public ActionResult NewOtherIncome()
        {
            return View();
        }
    }
}