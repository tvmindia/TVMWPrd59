using AutoMapper;
using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.UserInterface.Models;
using ProductionApp.UserInterface.SecurityFilter;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Controllers
{
    public class HomeController : Controller
    {
        IDynamicUIBusiness _dynamicUIBusiness;
        public HomeController(IDynamicUIBusiness dynamicUIBusiness)
        {
            _dynamicUIBusiness = dynamicUIBusiness;
        }
        // GET: Home
        [AuthSecurityFilter(ProjectObject = "Home", Mode = "")]
        public ActionResult Index()
        {
            AppUA appUA = Session["AppUA"] as AppUA;
            HomeViewModel homeVMObj = new HomeViewModel(); 
            List<AMCSysModuleViewModel> SysModuleVMList = Mapper.Map<List<AMCSysModule>, List<AMCSysModuleViewModel>>(_dynamicUIBusiness.GetAllModule());
            homeVMObj.SysModuleVMObj = new AMCSysModuleViewModel {
                SysModuleList = SysModuleVMList
            };
            ViewBag.ActionName = "Home";
            return View(homeVMObj);
        }


        [AuthSecurityFilter(ProjectObject = "AdminDashBoard", Mode = "R")]
        public ActionResult Admin()
        {
            ViewBag.ActionName = "Admin";
            AppUA appUA = Session["AppUA"] as AppUA;
          
            return View();
        }

        [AuthSecurityFilter(ProjectObject = "AdminDashBoard", Mode = "R")]
        public ActionResult SalesSummary()
        {
            ViewBag.ActionName = "Admin";
            AppUA appUA = Session["AppUA"] as AppUA;
            SalesSummaryViewModel data = new SalesSummaryViewModel();

            return PartialView("_SalesSummary", data);
        }


        [AuthSecurityFilter(ProjectObject = "AdminDashBoard", Mode = "R")]
        public ActionResult PurchaseSummary()
        {
            ViewBag.ActionName = "Admin";
            AppUA appUA = Session["AppUA"] as AppUA;
            PurchaseSummaryViewModel data = new PurchaseSummaryViewModel();

            return PartialView("_PurchaseSummary", data);
        }


        [AuthSecurityFilter(ProjectObject = "AdminDashBoard", Mode = "R")]
        public ActionResult IncomeExpenseSummary()
        {
            ViewBag.ActionName = "Admin";
            AppUA appUA = Session["AppUA"] as AppUA;
            IncomeExpenseSummaryViewModel data = new IncomeExpenseSummaryViewModel();

            return PartialView("_IncomeExpenseSummary", data);
        }




        [AuthSecurityFilter(ProjectObject = "AdminDashBoard", Mode = "R")]
        public ActionResult SalesInvoiceSummary()
        {
            ViewBag.ActionName = "Admin";
            AppUA appUA = Session["AppUA"] as AppUA;
            SalesInvoiceSummaryViewModel data = new SalesInvoiceSummaryViewModel();

            return PartialView("_SalesInvoiceSummary", data);
        }


        [AuthSecurityFilter(ProjectObject = "AdminDashBoard", Mode = "R")]
        public ActionResult PurchaseInvoiceSummary()
        {
            ViewBag.ActionName = "Admin";
            AppUA appUA = Session["AppUA"] as AppUA;
            PurchaseInvoiceSummaryViewModel data = new PurchaseInvoiceSummaryViewModel();

            return PartialView("_PurchaseInvoiceSummary", data);
        }

        [AuthSecurityFilter(ProjectObject = "AdminDashBoard", Mode = "R")]
        public ActionResult InventorySummary()
        {
            ViewBag.ActionName = "Admin";
            AppUA appUA = Session["AppUA"] as AppUA;
            InventorySummaryViewModel data = new InventorySummaryViewModel();

            return PartialView("_InventorySummary", data);
        }


        [AuthSecurityFilter(ProjectObject = "AdminDashBoard", Mode = "R")]
        public ActionResult ProductionSummary()
        {
            ViewBag.ActionName = "Admin";
            AppUA appUA = Session["AppUA"] as AppUA;
            ProductionSummaryViewModel data = new ProductionSummaryViewModel();

            return PartialView("_ProductionSummary", data);
        }

        [AuthSecurityFilter(ProjectObject = "AdminDashBoard", Mode = "R")]
        public ActionResult DispatchSummary()
        {
            ViewBag.ActionName = "Admin";
            AppUA appUA = Session["AppUA"] as AppUA;
            DispatchSummaryViewModel data = new DispatchSummaryViewModel();

            return PartialView("_DispatchSummary", data);
        }

    }
}