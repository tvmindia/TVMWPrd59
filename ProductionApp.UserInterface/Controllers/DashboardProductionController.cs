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
    public class DashboardProductionController : Controller
    {
        #region Constructor Injection       
        IBillOfMaterialBusiness _billOfMaterialBusiness;
        IProductionTrackingBusiness _productionTrackingBusiness;
        IAssemblyBusiness _assemblyBusiness;
       
        public DashboardProductionController(IBillOfMaterialBusiness billOfMaterialBusiness, IProductionTrackingBusiness productionTrackingBusiness, IAssemblyBusiness assemblyBusiness)
        {
            _billOfMaterialBusiness = billOfMaterialBusiness;
            _productionTrackingBusiness = productionTrackingBusiness;
            _assemblyBusiness = assemblyBusiness;


        }
        #endregion Constructor Injection

        // GET: Production
        [AuthSecurityFilter(ProjectObject = "DashboardProduction", Mode = "R")]
        public ActionResult Index(string Code)
        {
            ViewBag.SysModuleCode = Code;
            return View();
        }

        #region RecentBillOfMaterials
        [AuthSecurityFilter(ProjectObject = "DashboardProduction", Mode = "R")]
        public ActionResult RecentBillOfMaterials()
        {
            BillOfMaterialViewModel BillOfMaterial = new BillOfMaterialViewModel();
            BillOfMaterial.BaseURL = "/BillOfMaterial/ViewBillOfMaterial?code=PROD";
            BillOfMaterial.BillOfMaterialList = Mapper.Map<List<BillOfMaterial>, List<BillOfMaterialViewModel>>(_billOfMaterialBusiness.GetRecentBillOfMaterial(BillOfMaterial.BaseURL));
            return PartialView("_RecentBillOfMaterials", BillOfMaterial);
        }
        #endregion RecentBillOfMaterials

        #region GetRecentProductionTracking
        [AuthSecurityFilter(ProjectObject = "DashboardProduction", Mode = "R")]
        public ActionResult GetRecentProductionTracking()
        {
            ProductionTrackingViewModel ProductionTracking = new ProductionTrackingViewModel();
            ProductionTracking.BaseURL = "/ProductionTracking/ViewProductionTracking?code=PROD";
            ProductionTracking.ProductionTrackingList = Mapper.Map<List<ProductionTracking>, List<ProductionTrackingViewModel>>(_productionTrackingBusiness.GetRecentProductionTracking(ProductionTracking.BaseURL));
            return PartialView("_RecentProductionTracking", ProductionTracking);
        }
        #endregion GetRecentProductionTracking

        #region RecentAssemblyProduct
        [AuthSecurityFilter(ProjectObject = "DashboardProduction", Mode = "R")]
        public ActionResult RecentAssemblyProduct()
         {
            AssemblyViewModel Assembly = new AssemblyViewModel();
            Assembly.BaseURL = "/Assembly/ViewAssembly?code=PROD";
            Assembly.AssemblyList = Mapper.Map<List<Assembly>, List<AssemblyViewModel>>(_assemblyBusiness.GetRecentAssemblyProduct(Assembly.BaseURL));
            return PartialView("_RecentAssemblyProduct", Assembly);
        }
        #endregion RecentAssemblyProduct
    }
}