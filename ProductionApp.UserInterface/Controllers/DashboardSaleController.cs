using ProductionApp.BusinessService.Contracts;
using ProductionApp.UserInterface.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProductionApp.DataAccessObject.DTO;
namespace ProductionApp.UserInterface.Controllers
{
    public class DashboardSaleController : Controller
    {
        #region Constructor Injection       
        ISalesOrderBusiness _salesOrderBusiness;
        IPackingSlipBusiness _packingSlipBusiness;      
        public DashboardSaleController(ISalesOrderBusiness salesOrderBusiness,IPackingSlipBusiness packingSlipBusiness)
        {
            _salesOrderBusiness = salesOrderBusiness;
            _packingSlipBusiness = packingSlipBusiness;          
        }
        #endregion Constructor Injection


        // GET: DashboardSales
        public ActionResult Index(string Code)
        {
            ViewBag.SysModuleCode = Code;
            return View();
        }
        public ActionResult RecentSalesOrder()
        {
            SalesOrderViewModel SalesOrder = new SalesOrderViewModel();
            SalesOrder.BaseURL = "/SalesOrder/ListSalesOrder?code=SALE";
            SalesOrder.SalesOrderList = Mapper.Map<List<SalesOrder>, List<SalesOrderViewModel>>(_salesOrderBusiness.GetRecentSalesOrder(SalesOrder.BaseURL));
            return PartialView("_RecentSalesOrder", SalesOrder);
        }

        public ActionResult RecentPackingSlip()
        {
            PackingSlipViewModel PackingSlip = new PackingSlipViewModel();
            PackingSlip.BaseURL = "/PackingSlip/ListPackingSlips?code=SALE";
            PackingSlip.PackingSlipList = Mapper.Map<List<PackingSlip>, List<PackingSlipViewModel>>(_packingSlipBusiness.GetRecentPackingSlip(PackingSlip.BaseURL));
            return PartialView("_RecentPackingSlip", PackingSlip);
        }
    }
}