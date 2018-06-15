using AutoMapper;
using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.UserInterface.Models;
using ProductionApp.UserInterface.SecurityFilter;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
namespace ProductionApp.UserInterface.Controllers
{
    public class HomeController : Controller
    {
        IDynamicUIBusiness _dynamicUIBusiness;
        ISalesInvoieBusiness _salesInvoiceBusiness;
        IPurchaseInvoiceBusiness _purchaseInvoiceBusiness;
        IProductBusiness _productBusiness;
        IMaterialBusiness _materialBusiness;
        ICommonBusiness _commonBusiness;
        private ICustomerInvoiceBusiness _customerInvoiceBusiness;
        private ISupplierInvoiceBusiness _supplierInvoiceBusiness;
        public HomeController(IDynamicUIBusiness dynamicUIBusiness, ISalesInvoieBusiness salesInvoiceBusiness, IPurchaseInvoiceBusiness purchaseInvoiceBusiness,IProductBusiness productBusiness,ICommonBusiness commonBusiness, ISupplierInvoiceBusiness supplierInvoiceBusiness, ICustomerInvoiceBusiness customerInvoiceBusiness, IMaterialBusiness materialBusiness)
        {
            _dynamicUIBusiness = dynamicUIBusiness;
             _salesInvoiceBusiness= salesInvoiceBusiness;
            _purchaseInvoiceBusiness = purchaseInvoiceBusiness;
            _productBusiness = productBusiness;
            _commonBusiness = commonBusiness;
            _materialBusiness = materialBusiness;
            _supplierInvoiceBusiness = supplierInvoiceBusiness;
            _customerInvoiceBusiness = customerInvoiceBusiness;
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
            
            SalesSummaryList salesList = new SalesSummaryList();          
            salesList.SalesSmryList = Mapper.Map < List<SalesSummary>, List <SalesSummaryViewModel>>( _salesInvoiceBusiness.GetSalesSummary());
            return PartialView("_SalesSummary", salesList);
        }


        [AuthSecurityFilter(ProjectObject = "AdminDashBoard", Mode = "R")]
        public ActionResult PurchaseSummary()
        {

            PurchaseSummaryList purchaseList = new PurchaseSummaryList();
            purchaseList.PurchaseSmryList = Mapper.Map<List<PurchaseSummary>, List<PurchaseSummaryViewModel>>(_purchaseInvoiceBusiness.GetPurchaseSummary());

            return PartialView("_PurchaseSummary", purchaseList);
        }


        [AuthSecurityFilter(ProjectObject = "AdminDashBoard", Mode = "R")]
        public ActionResult IncomeExpenseSummary()
        {

            IncomeExpenseSummaryListViewModel list = new IncomeExpenseSummaryListViewModel();
            list.IncomeExpenseList = Mapper.Map<List<IncomeExpenseSummary>, List<IncomeExpenseSummaryViewModel>>(_dynamicUIBusiness.GetIncomeExpenseSummary());
            ViewBag.TotalIncome = 0;
            ViewBag.TotalExpense = 0;
            foreach(IncomeExpenseSummaryViewModel incomeExpenseSummary in list.IncomeExpenseList)
            {
                ViewBag.TotalIncome += incomeExpenseSummary.InAmount;
                ViewBag.TotalExpense += incomeExpenseSummary.ExAmount;
            }
            ViewBag.TotalProfit = ViewBag.TotalIncome - ViewBag.TotalExpense;
            return PartialView("_IncomeExpenseSummary", list);
        }




        [AuthSecurityFilter(ProjectObject = "AdminDashBoard", Mode = "R")]
        public ActionResult SalesInvoiceSummary()
        {
            ViewBag.ActionName = "Admin";
            AppUA appUA = Session["AppUA"] as AppUA;
            SalesInvoiceSummaryViewModel data = new SalesInvoiceSummaryViewModel();
            ViewBag.OutStandingSupplierInvoice = _supplierInvoiceBusiness.GetOutstandingSupplierInvoice();
            ViewBag.OutStandingCustomerInvoice = _customerInvoiceBusiness.GetOutstandingCustomerInvoice();
            return PartialView("_SalesInvoiceSummary", data);
        }


        [AuthSecurityFilter(ProjectObject = "AdminDashBoard", Mode = "R")]
        public ActionResult PurchaseInvoiceSummary()
        {
            ViewBag.ActionName = "Admin";
            AppUA appUA = Session["AppUA"] as AppUA;
            PurchaseInvoiceSummaryViewModel data = new PurchaseInvoiceSummaryViewModel();
            //data.TotalPendingPayments
            return PartialView("_PurchaseInvoiceSummary", data);
        }

        [AuthSecurityFilter(ProjectObject = "AdminDashBoard", Mode = "R")]
        public ActionResult InventorySummary()
        {
            InventorySummaryViewModel data = new InventorySummaryViewModel();
            data.MaterialSummaryList = Mapper.Map<List<MaterialSummary>, List<MaterialSummaryViewModel>>(_materialBusiness.GetMaterialSummary());
            data.TotalValue = data.MaterialSummaryList.Sum(v => v.Value);
            data.TotalValueFormatted = _commonBusiness.ConvertCurrency(data.TotalValue);     

            return PartialView("_InventorySummary", data);
        }


        [AuthSecurityFilter(ProjectObject = "AdminDashBoard", Mode = "R")]
        public ActionResult FGInventorySummary()
        {
             
            FGInventorySummaryViewModel data = new FGInventorySummaryViewModel();
            data.FinishedGoodsSummaryList = Mapper.Map<List<FinishedGoodSummary>, List<FinishedGoodSummaryViewModel>>(_productBusiness.GetFinishGoodsSummary());
            data.TotalValue = data.FinishedGoodsSummaryList.Sum(v => v.Value);
            data.TotalValueFormatted = _commonBusiness.ConvertCurrency(data.TotalValue);
            return PartialView("_FGInventorySummary", data);
        }




        [AuthSecurityFilter(ProjectObject = "AdminDashBoard", Mode = "R")]
        public ActionResult ProductionSummary()
        {
            ProductionSummaryListViewModel data = new ProductionSummaryListViewModel();
            data.ProductionSummaryList= Mapper.Map<List<ProductionSummary>, List<ProductionSummaryViewModel>>(_dynamicUIBusiness.GetProductionSummary());

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