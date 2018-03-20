using ProductionApp.BusinessService.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Controllers
{
    public class CustomerInvoiceController : Controller
    {
        private ICustomerInvoiceBusiness _customerInvoiceBusiness;
        public CustomerInvoiceController(ICustomerInvoiceBusiness customerInvoiceBusiness)
        {
            _customerInvoiceBusiness = customerInvoiceBusiness;
        }
        // GET: CustomerInvoice
        public ActionResult ViewCustomerInvoice()
        {
            return View();
        }
        public ActionResult NewCustomerInvoice()
        {
            return View();
        }
    }
}