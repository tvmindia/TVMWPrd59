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
    public class DashboardPurchaseController : Controller
    {

        #region Constructor Injection       
        IRequisitionBusiness _requisitionBusiness;
        IPurchaseOrderBusiness _purchaseOrderBusiness;     
        public DashboardPurchaseController(IRequisitionBusiness requisitionBusiness,IPurchaseOrderBusiness purchaseOrderBusiness)
        {
            _requisitionBusiness = requisitionBusiness;
            _purchaseOrderBusiness = purchaseOrderBusiness;         
        }
        #endregion Constructor Injection

        // GET: PurchaseDashboard
        [AuthSecurityFilter(ProjectObject = "DashboardPurchase", Mode = "R")]
        public ActionResult Index(string Code)
        {
            ViewBag.SysModuleCode = Code;
            return View();
        }

        #region RecentRequisition
        [AuthSecurityFilter(ProjectObject = "DashboardPurchase", Mode = "R")]
        public ActionResult RecentRequisition()
        {       
            RequisitionViewModel Requisition = new RequisitionViewModel();
            Requisition.BaseURL= "/Requisition/ViewRequisition?code=PURCH/";
            Requisition.RequisitionList = Mapper.Map<List<Requisition>, List<RequisitionViewModel>>(_requisitionBusiness.GetRecentRequisition(Requisition.BaseURL));
            return PartialView("_RecentRequisitions", Requisition);
        }
        #endregion RecentRequisition

        #region RecentPurchaseOrder
        [AuthSecurityFilter(ProjectObject = "DashboardPurchase", Mode = "R")]
        public ActionResult RecentPurchaseOrder()
        {
            PurchaseOrderViewModel PurchaseOrder = new PurchaseOrderViewModel();
            PurchaseOrder.BaseURL = "/PurchaseOrder/ViewPurchaseOrder?code=PURCH/";
            PurchaseOrder.PurchaseOrderList = Mapper.Map<List<PurchaseOrder>, List<PurchaseOrderViewModel>>(_purchaseOrderBusiness.RecentPurchaseOrder(PurchaseOrder.BaseURL));
            return PartialView("_PurchaseOrderSummary", PurchaseOrder);
        }
        #endregion RecentPurchaseOrder
    }
}