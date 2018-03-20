using AutoMapper;
using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.UserInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Controllers
{
    public class PaymentTermController : Controller
    {
        private IPaymentTermBusiness _paymentTermBusiness;
        public PaymentTermController(IPaymentTermBusiness paymentTermBusiness)
        {
            _paymentTermBusiness = paymentTermBusiness;
        }
        // GET: PaymentTerm
        public ActionResult PaymentTerm()
        {
            return View();
        }
        public ActionResult PaymentTermDropdown()
        {
            PaymentTermViewModel paymentTermVM = new PaymentTermViewModel();
            paymentTermVM.SelectList = new List<SelectListItem>();
            List<PaymentTermViewModel> paymentTermList = Mapper.Map<List<PaymentTerm>, List<PaymentTermViewModel>>(_paymentTermBusiness.GetAllPaymentTerm());
            if (paymentTermList != null)
                foreach (PaymentTermViewModel paymentTerm in paymentTermList)
                {
                    paymentTermVM.SelectList.Add(new SelectListItem
                    {
                        Text = paymentTerm.Description,
                        Value = paymentTerm.Code,
                        Selected = false
                    });
                }
            return PartialView("_PaymentTermDropdown", paymentTermVM);

        }
    }
}