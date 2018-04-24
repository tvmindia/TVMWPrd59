using ProductionApp.BusinessService.Contracts;
using ProductionApp.UserInterface.Models;
using ProductionApp.UserInterface.SecurityFilter;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProductionApp.DataAccessObject.DTO;

namespace ProductionApp.UserInterface.Controllers
{
    public class DashboardAccountController : Controller
    {
        #region Constructor Injection       
        ICustomerInvoiceBusiness _customerInvoiceBusiness;
        ICustomerPaymentBusiness _customerPaymentBusiness;


        public DashboardAccountController(ICustomerInvoiceBusiness customerInvoiceBusiness,ICustomerPaymentBusiness customerPaymentBusiness)
        {
            _customerInvoiceBusiness = customerInvoiceBusiness;
            _customerPaymentBusiness = customerPaymentBusiness;

        }
        #endregion Constructor Injection

        // GET: Dashboard
        [AuthSecurityFilter(ProjectObject = "DashboardAccount", Mode = "R")]
        public ActionResult Index(string Code)
        {
            ViewBag.SysModuleCode = Code;
            return View();
        }

        #region RecentCustomerInvoice
        [AuthSecurityFilter(ProjectObject = "DashboardAccount", Mode = "R")]
        public ActionResult RecentCustomerInvoice()
        {
            CustomerInvoiceViewModel CustomerInvoice = new CustomerInvoiceViewModel();
            CustomerInvoice.BaseURL = "CustomerInvoice/ViewCustomerInvoice?code=ACC";
            CustomerInvoice.CustomerInvoiceList = Mapper.Map<List<CustomerInvoice>, List<CustomerInvoiceViewModel>>(_customerInvoiceBusiness.GetRecentCustomerInvoice(CustomerInvoice.BaseURL));
            return PartialView("_RecentCustomerInvoice", CustomerInvoice);
        }
        #endregion RecentCustomerInvoice

        #region RecentCustomerPayment
        [AuthSecurityFilter(ProjectObject = "DashboardAccount", Mode = "R")]
        public ActionResult RecentCustomerPayment()
        {
            CustomerPaymentViewModel CustomerPayment = new CustomerPaymentViewModel();
            CustomerPayment.BaseURL = "CustomerPayment/ViewCustomerPayment?code=ACC";
            CustomerPayment.CustomerPaymentList = Mapper.Map<List<CustomerPayment>, List<CustomerPaymentViewModel>>(_customerPaymentBusiness.GetRecentCustomerPayment(CustomerPayment.BaseURL));
            return PartialView("_RecentCustomerPayment", CustomerPayment);
        }
        #endregion RecentCustomerPayment
    }
}