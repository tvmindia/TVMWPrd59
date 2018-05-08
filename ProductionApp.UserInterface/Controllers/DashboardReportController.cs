﻿using AutoMapper;
using Newtonsoft.Json;
using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.UserInterface.Models;
using ProductionApp.UserInterface.SecurityFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Controllers
{
    public class DashboardReportController : Controller
    {
        #region Constructor Injection  
        AppConst _appConst = new AppConst();
        IReportBusiness _reportBusiness;
        IEmployeeBusiness _employeeBusiness;
        IRequisitionBusiness _requisitionBusiness;
        ICommonBusiness _commonBusiness;
        IMaterialBusiness _materialBusiness;
        ISupplierBusiness _supplierBusiness;

        public DashboardReportController(IReportBusiness reportBusiness,IEmployeeBusiness employeeBusiness,IRequisitionBusiness requisitionBusiness,ICommonBusiness commonBusiness,IMaterialBusiness materialBusiness, ISupplierBusiness supplierBusiness)
        {
            _reportBusiness = reportBusiness;
            _employeeBusiness = employeeBusiness;
            _requisitionBusiness = requisitionBusiness;
            _commonBusiness = commonBusiness;
            _materialBusiness = materialBusiness;
            _supplierBusiness = supplierBusiness;
        }
        #endregion Constructor Injection

        // GET: Inventory
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "DashboardReport", Mode = "R")]
        public ActionResult Index(string Code, string searchTerm)
        {
            ViewBag.SysModuleCode = Code;
            AMCSysReportViewModel AMCSysReport = new AMCSysReportViewModel();            
            AMCSysReport.AMCSysReportList= Mapper.Map<List<AMCSysReport>, List<AMCSysReportViewModel>>(_reportBusiness.GetAllReport(searchTerm));
            AMCSysReport.AMCSysReportList = AMCSysReport.AMCSysReportList != null ? AMCSysReport.AMCSysReportList.OrderBy(s => s.GroupOrder).ToList() : null;
            return View(AMCSysReport);
        }

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "DashboardReport", Mode = "R")]
        public ActionResult RequisitionSummaryReport(string Code)
        {
            ViewBag.SysModuleCode = Code;
            
            RequisitionSummaryReportViewModel requisitionSummaryReportVM = new RequisitionSummaryReportViewModel();           
            requisitionSummaryReportVM.Employee = new EmployeeViewModel();
            requisitionSummaryReportVM.Employee.SelectList = _employeeBusiness.GetEmployeeSelectList();
            return View(requisitionSummaryReportVM);
        }

        #region GetRequisitionSummaryReport
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "DashboardReport", Mode = "R")]
        public JsonResult GetRequisitionSummaryReport(DataTableAjaxPostModel model, RequisitionSummaryReportViewModel requisitionSummaryVM)
        {  
            Common con = new Common();
            DateTime dt = con.GetCurrentDateTime();
            if (requisitionSummaryVM != null)
            {
                if (requisitionSummaryVM.DateFilter == "30")
                {
                    requisitionSummaryVM.FromDate = dt.AddDays(-30).ToString("dd-MMM-yyyy");
                    requisitionSummaryVM.ToDate = dt.ToString("dd-MMM-yyyy");
                }
                if (requisitionSummaryVM.DateFilter == "60")
                {
                    requisitionSummaryVM.FromDate = dt.AddDays(-60).ToString("dd-MMM-yyyy");
                    requisitionSummaryVM.ToDate = dt.ToString("dd-MMM-yyyy");
                }
                if (requisitionSummaryVM.DateFilter == "90")
                {
                    requisitionSummaryVM.FromDate = dt.AddDays(-90).ToString("dd-MMM-yyyy");
                    requisitionSummaryVM.ToDate = dt.ToString("dd-MMM-yyyy");
                }
            }
            requisitionSummaryVM.DataTablePaging.Start = model.start;
            requisitionSummaryVM.DataTablePaging.Length = (requisitionSummaryVM.DataTablePaging.Length == 0 ? model.length : requisitionSummaryVM.DataTablePaging.Length);

            List<RequisitionViewModel> requisitionOrderList = Mapper.Map<List<Requisition>, List<RequisitionViewModel>>(_reportBusiness.GetRequisitionSummaryReport(Mapper.Map<RequisitionSummaryReportViewModel, RequisitionSummaryReport>(requisitionSummaryVM)));

            if (requisitionSummaryVM.DataTablePaging.Length == -1)
            {
                int totalResult = requisitionOrderList.Count != 0 ? requisitionOrderList[0].TotalCount : 0;
                int filteredResult = requisitionOrderList.Count != 0 ? requisitionOrderList[0].FilteredCount : 0;
                requisitionOrderList = requisitionOrderList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }
            var settings = new JsonSerializerSettings
            {
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
            };

            return Json(new
            {
                draw = model.draw,
                recordsTotal = requisitionOrderList.Count != 0 ? requisitionOrderList[0].TotalCount : 0,
                recordsFiltered = requisitionOrderList.Count != 0 ? requisitionOrderList[0].FilteredCount : 0,
                data = requisitionOrderList
            });
        }
        #endregion GetRequisitionSummaryReport



        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "DashboardReport", Mode = "R")]
        public ActionResult RequisitionDetailReport(string Code)
        {
            ViewBag.SysModuleCode = Code;

            RequisitionDetailReportViewModel requisitionDetailReportVM = new RequisitionDetailReportViewModel();
            requisitionDetailReportVM.Employee = new EmployeeViewModel();
            requisitionDetailReportVM.Employee.SelectList = _employeeBusiness.GetEmployeeSelectList();
            requisitionDetailReportVM.Material = new MaterialViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            requisitionDetailReportVM.Material.SelectList = new List<SelectListItem>();
            List<MaterialViewModel> materialList = Mapper.Map<List<Material>, List<MaterialViewModel>>(_materialBusiness.GetMaterialForSelectList());
            if (materialList != null)
                foreach (MaterialViewModel material in materialList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = material.MaterialCode + '-' + material.Description,
                        Value = material.ID.ToString(),
                        Selected = false
                    });
                }
            requisitionDetailReportVM.Material.SelectList = selectListItem;


            //requisitionDetailReportVM.Material.SelectList = _materialBusiness.GetMaterialForSelectList();
            return View(requisitionDetailReportVM);
        }

        #region GetRequisitionDetailReport
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "DashboardReport", Mode = "R")]
        public JsonResult GetRequisitionDetailReport(DataTableAjaxPostModel model, RequisitionDetailReportViewModel requisitionDetailVM)
        {
            Common con = new Common();
            DateTime dt = con.GetCurrentDateTime();
            if (requisitionDetailVM != null)
            {
                if (requisitionDetailVM.DateFilter == "30")
                {
                    requisitionDetailVM.FromDate = dt.AddDays(-30).ToString("dd-MMM-yyyy");
                    requisitionDetailVM.ToDate = dt.ToString("dd-MMM-yyyy");
                }
                if (requisitionDetailVM.DateFilter == "60")
                {
                    requisitionDetailVM.FromDate = dt.AddDays(-60).ToString("dd-MMM-yyyy");
                    requisitionDetailVM.ToDate = dt.ToString("dd-MMM-yyyy");
                }
                if (requisitionDetailVM.DateFilter == "90")
                {
                    requisitionDetailVM.FromDate = dt.AddDays(-90).ToString("dd-MMM-yyyy");
                    requisitionDetailVM.ToDate = dt.ToString("dd-MMM-yyyy");
                }
            }
            requisitionDetailVM.DataTablePaging.Start = model.start;
            requisitionDetailVM.DataTablePaging.Length = (requisitionDetailVM.DataTablePaging.Length == 0 ? model.length : requisitionDetailVM.DataTablePaging.Length);

            List<RequisitionDetailReportViewModel> requisitionOrderDetailList = Mapper.Map<List<RequisitionDetailReport>, List<RequisitionDetailReportViewModel>>(_reportBusiness.GetRequisitionDetailReport(Mapper.Map<RequisitionDetailReportViewModel, RequisitionDetailReport>(requisitionDetailVM)));

            if (requisitionDetailVM.DataTablePaging.Length == -1)
            {
                int totalResult = requisitionOrderDetailList.Count != 0 ? requisitionOrderDetailList[0].TotalCount : 0;
                int filteredResult = requisitionOrderDetailList.Count != 0 ? requisitionOrderDetailList[0].FilteredCount : 0;
                requisitionOrderDetailList = requisitionOrderDetailList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }
            var settings = new JsonSerializerSettings
            {
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
            };

            return Json(new
            {
                draw = model.draw,
                recordsTotal = requisitionOrderDetailList.Count != 0 ? requisitionOrderDetailList[0].TotalCount : 0,
                recordsFiltered = requisitionOrderDetailList.Count != 0 ? requisitionOrderDetailList[0].FilteredCount : 0,
                data = requisitionOrderDetailList
            });
        }
        #endregion GetRequisitionSummaryReport


        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "DashboardReport", Mode = "R")]
        public ActionResult PurchaseSummaryReport(string Code)
        {
            ViewBag.SysModuleCode = Code;

            PurchaseSummaryReportViewModel purchaseSummaryReportVM = new PurchaseSummaryReportViewModel();
            purchaseSummaryReportVM.Supplier = new SupplierViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            purchaseSummaryReportVM.Supplier.SelectList = new List<SelectListItem>();
            List<SupplierViewModel> supplierList = Mapper.Map<List<Supplier>, List<SupplierViewModel>>(_supplierBusiness.GetSupplierForSelectList());
            if (supplierList != null)
                foreach (SupplierViewModel supplier in supplierList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = supplier.CompanyName,
                        Value = supplier.ID.ToString(),
                        Selected = false
                    });
                }
            purchaseSummaryReportVM.Supplier.SelectList = selectListItem;
            return View(purchaseSummaryReportVM);
        }

        #region GetPurchaseSummaryReport
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "DashboardReport", Mode = "R")]
        public JsonResult GetPurchaseSummaryReport(DataTableAjaxPostModel model, PurchaseSummaryReportViewModel purchaseSummaryVM)
        {
            Common con = new Common();
            DateTime dt = con.GetCurrentDateTime();
            if (purchaseSummaryVM != null)
            {
                if (purchaseSummaryVM.DateFilter == "30")
                {
                    purchaseSummaryVM.FromDate = dt.AddDays(-30).ToString("dd-MMM-yyyy");
                    purchaseSummaryVM.ToDate = dt.ToString("dd-MMM-yyyy");
                }
                if (purchaseSummaryVM.DateFilter == "60")
                {
                    purchaseSummaryVM.FromDate = dt.AddDays(-60).ToString("dd-MMM-yyyy");
                    purchaseSummaryVM.ToDate = dt.ToString("dd-MMM-yyyy");
                }
                if (purchaseSummaryVM.DateFilter == "90")
                {
                    purchaseSummaryVM.FromDate = dt.AddDays(-90).ToString("dd-MMM-yyyy");
                    purchaseSummaryVM.ToDate = dt.ToString("dd-MMM-yyyy");
                }
            }
            purchaseSummaryVM.DataTablePaging.Start = model.start;
            purchaseSummaryVM.DataTablePaging.Length = (purchaseSummaryVM.DataTablePaging.Length == 0 ? model.length : purchaseSummaryVM.DataTablePaging.Length);

            List<PurchaseOrderViewModel> purchaseOrderList = Mapper.Map<List<PurchaseOrder>, List<PurchaseOrderViewModel>>(_reportBusiness.GetPurchaseSummaryReport(Mapper.Map<PurchaseSummaryReportViewModel, PurchaseSummaryReport>(purchaseSummaryVM)));

            if (purchaseSummaryVM.DataTablePaging.Length == -1)
            {
                int totalResult = purchaseOrderList.Count != 0 ? purchaseOrderList[0].TotalCount : 0;
                int filteredResult = purchaseOrderList.Count != 0 ? purchaseOrderList[0].FilteredCount : 0;
                purchaseOrderList = purchaseOrderList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }
            var settings = new JsonSerializerSettings
            {
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
            };

            return Json(new
            {
                draw = model.draw,
                recordsTotal = purchaseOrderList.Count != 0 ? purchaseOrderList[0].TotalCount : 0,
                recordsFiltered = purchaseOrderList.Count != 0 ? purchaseOrderList[0].FilteredCount : 0,
                data = purchaseOrderList
            });
        }
        #endregion GetPurchaseSummaryReport

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "dashboardReport", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":                   
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetReportList();";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "ExportReportData();";
                    //---------------------------------------
                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Href = Url.Action("Index", "DashboardReport", new { Code = "RPT" });
                    break;                
               
              
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion

    }
}