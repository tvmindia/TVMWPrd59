using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Controllers
{
    public class CustomerPaymentController : Controller
    {
        private ICustomerPaymentBusiness _customerPaymentBusiness;
        AppConst _appConst = new AppConst();
       
        public CustomerPaymentController(ICustomerPaymentBusiness customerPaymentBusiness)
        {
            _customerPaymentBusiness = customerPaymentBusiness;
        }
        public ActionResult NewCustomerPayment(string code, Guid? id)
        {
            ViewBag.SysModuleCode = code;
            return View();
        }
        public ActionResult ViewCustomerPayment(string code)
        {
            ViewBag.SysModuleCode = code;
            return View();
        }
    }
}