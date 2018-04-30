using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Controllers
{
    public class OtherExpenseController : Controller
    {
        private IOtherExpenseBusiness _otherExpenseBusiness;
        Common _common = new Common();
        AppConst _appConst = new AppConst();
        public OtherExpenseController(IOtherExpenseBusiness otherExpenseBusiness)
        {
            _otherExpenseBusiness = otherExpenseBusiness;
        }
        // GET: OtherExpense
        public ActionResult NewOtherExpense(string code)
        {
            ViewBag.SysModuleCode = code;
            return View();
        }
        public ActionResult ViewOtherExpense(string code)
        {
            ViewBag.SysModuleCode = code;
            return View();
        }
    }
}