using AutoMapper;
using Newtonsoft.Json;
using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.UserInterface.Models;
using ProductionApp.UserInterface.SecurityFilter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System.IO;
using System.Web.SessionState;

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
        IApprovalStatusBusiness _approvalStatusBusiness;
        IMaterialTypeBusiness _materialTypeBusiness;
        IProductBusiness _productBusiness;
        ICustomerBusiness _customerBusiness;
        public DashboardReportController(IReportBusiness reportBusiness,IEmployeeBusiness employeeBusiness,IRequisitionBusiness requisitionBusiness,ICommonBusiness commonBusiness,IMaterialBusiness materialBusiness, ISupplierBusiness supplierBusiness,IApprovalStatusBusiness approvalStatusBusiness, 
            IMaterialTypeBusiness materialTypeBusiness, IProductBusiness productBusiness,ICustomerBusiness customerBusiness)
        {
            _reportBusiness = reportBusiness;
            _employeeBusiness = employeeBusiness;
            _requisitionBusiness = requisitionBusiness;
            _commonBusiness = commonBusiness;
            _materialBusiness = materialBusiness;
            _supplierBusiness = supplierBusiness;
            _approvalStatusBusiness = approvalStatusBusiness;
            _materialTypeBusiness = materialTypeBusiness;
            _productBusiness = productBusiness;
            _customerBusiness = customerBusiness;
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
        [AuthSecurityFilter(ProjectObject = "RequisitionReport", Mode = "R")]
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
        [AuthSecurityFilter(ProjectObject = "RequisitionReport", Mode = "R")]
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
        [AuthSecurityFilter(ProjectObject = "RequisitionReport", Mode = "R")]
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
        [AuthSecurityFilter(ProjectObject = "RequisitionReport", Mode = "R")]
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
        [AuthSecurityFilter(ProjectObject = "PurchaseReport", Mode = "R")]
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
        [AuthSecurityFilter(ProjectObject = "PurchaseReport", Mode = "R")]
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

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "PurchaseReport", Mode = "R")]
        public ActionResult PurchaseDetailReport(string Code)
        {
            ViewBag.SysModuleCode = Code;

            PurchaseDetailReportViewModel purchaseDetailReportVM = new PurchaseDetailReportViewModel();
            purchaseDetailReportVM.Supplier = new SupplierViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            purchaseDetailReportVM.Supplier.SelectList = new List<SelectListItem>();
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
            purchaseDetailReportVM.Supplier.SelectList = selectListItem;
             selectListItem = new List<SelectListItem>();
            purchaseDetailReportVM.Material = new MaterialViewModel();
            purchaseDetailReportVM.Material.SelectList = new List<SelectListItem>();
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
            purchaseDetailReportVM.Material.SelectList = selectListItem;
            purchaseDetailReportVM.Approval = new ApprovalStatusViewModel();
            purchaseDetailReportVM.Approval.StatusSelectList = _approvalStatusBusiness.GetApprovalStatusForSelectList();
            return View(purchaseDetailReportVM);
        }

        #region GetPurchaseDetailReport
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "PurchaseReport", Mode = "R")]
        public JsonResult GetPurchaseDetailReport(DataTableAjaxPostModel model, PurchaseDetailReportViewModel purchaseDetailVM)
        {
            Common con = new Common();
            DateTime dt = con.GetCurrentDateTime();
            if (purchaseDetailVM != null)
            {
                if (purchaseDetailVM.DateFilter == "30")
                {
                    purchaseDetailVM.FromDate = dt.AddDays(-30).ToString("dd-MMM-yyyy");
                    purchaseDetailVM.ToDate = dt.ToString("dd-MMM-yyyy");
                }
                if (purchaseDetailVM.DateFilter == "60")
                {
                    purchaseDetailVM.FromDate = dt.AddDays(-60).ToString("dd-MMM-yyyy");
                    purchaseDetailVM.ToDate = dt.ToString("dd-MMM-yyyy");
                }
                if (purchaseDetailVM.DateFilter == "90")
                {
                    purchaseDetailVM.FromDate = dt.AddDays(-90).ToString("dd-MMM-yyyy");
                    purchaseDetailVM.ToDate = dt.ToString("dd-MMM-yyyy");
                }
            }
            purchaseDetailVM.DataTablePaging.Start = model.start;
            purchaseDetailVM.DataTablePaging.Length = (purchaseDetailVM.DataTablePaging.Length == 0 ? model.length : purchaseDetailVM.DataTablePaging.Length);

            List<PurchaseDetailReportViewModel> purchaseOrderDetailList = Mapper.Map<List<PurchaseDetailReport>, List<PurchaseDetailReportViewModel>>(_reportBusiness.GetPurchaseDetailReport(Mapper.Map<PurchaseDetailReportViewModel, PurchaseDetailReport>(purchaseDetailVM)));

            if (purchaseDetailVM.DataTablePaging.Length == -1)
            {
                int totalResult = purchaseOrderDetailList.Count != 0 ? purchaseOrderDetailList[0].TotalCount : 0;
                int filteredResult = purchaseOrderDetailList.Count != 0 ? purchaseOrderDetailList[0].FilteredCount : 0;
                purchaseOrderDetailList = purchaseOrderDetailList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }
            var settings = new JsonSerializerSettings
            {
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
            };

            return Json(new
            {
                draw = model.draw,
                recordsTotal = purchaseOrderDetailList.Count != 0 ? purchaseOrderDetailList[0].TotalCount : 0,
                recordsFiltered = purchaseOrderDetailList.Count != 0 ? purchaseOrderDetailList[0].FilteredCount : 0,
                data = purchaseOrderDetailList
            });
        }
        #endregion GetPurchaseDetailReport

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "PurchaseRegisterReport", Mode = "R")]
        public ActionResult PurchaseRegisterReport(string Code)
        {
            ViewBag.SysModuleCode = Code;
            PurchaseRegisterReportViewModel purchaseRegisterReportVM = new PurchaseRegisterReportViewModel();
            purchaseRegisterReportVM.Supplier = new SupplierViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            purchaseRegisterReportVM.Supplier.SelectList = new List<SelectListItem>();
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
            purchaseRegisterReportVM.Supplier.SelectList = selectListItem;                  
            return View(purchaseRegisterReportVM);
        }


        #region GetPurchaseRegisterReport
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "PurchaseRegisterReport", Mode = "R")]
        public JsonResult GetPurchaseRegisterReport(DataTableAjaxPostModel model, PurchaseRegisterReportViewModel purchaseRegisterVM)
        {
            Common con = new Common();
            DateTime dt = con.GetCurrentDateTime();
            if (purchaseRegisterVM != null)
            {
                if (purchaseRegisterVM.DateFilter == "30")
                {
                    purchaseRegisterVM.FromDate = dt.AddDays(-30).ToString("dd-MMM-yyyy");
                    purchaseRegisterVM.ToDate = dt.ToString("dd-MMM-yyyy");
                }
                if (purchaseRegisterVM.DateFilter == "60")
                {
                    purchaseRegisterVM.FromDate = dt.AddDays(-60).ToString("dd-MMM-yyyy");
                    purchaseRegisterVM.ToDate = dt.ToString("dd-MMM-yyyy");
                }
                if (purchaseRegisterVM.DateFilter == "90")
                {
                    purchaseRegisterVM.FromDate = dt.AddDays(-90).ToString("dd-MMM-yyyy");
                    purchaseRegisterVM.ToDate = dt.ToString("dd-MMM-yyyy");
                }
            }
            purchaseRegisterVM.DataTablePaging.Start = model.start;
            purchaseRegisterVM.DataTablePaging.Length = (purchaseRegisterVM.DataTablePaging.Length == 0 ? model.length : purchaseRegisterVM.DataTablePaging.Length);

            List<PurchaseRegisterReportViewModel> purchaseRegisterList = Mapper.Map<List<PurchaseRegisterReport>, List<PurchaseRegisterReportViewModel>>(_reportBusiness.GetPurchaseRegisterReport(Mapper.Map<PurchaseRegisterReportViewModel, PurchaseRegisterReport>(purchaseRegisterVM)));

            if (purchaseRegisterVM.DataTablePaging.Length == -1)
            {
                int totalResult = purchaseRegisterList.Count != 0 ? purchaseRegisterList[0].TotalCount : 0;
                int filteredResult = purchaseRegisterList.Count != 0 ? purchaseRegisterList[0].FilteredCount : 0;
                purchaseRegisterList = purchaseRegisterList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }
            var settings = new JsonSerializerSettings
            {
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
            };

            return Json(new
            {
                draw = model.draw,
                recordsTotal = purchaseRegisterList.Count != 0 ? purchaseRegisterList[0].TotalCount : 0,
                recordsFiltered = purchaseRegisterList.Count != 0 ? purchaseRegisterList[0].FilteredCount : 0,
                data = purchaseRegisterList
            });
        }
        #endregion GetPurchaseRegisterReport

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "InventoryReport", Mode = "R")]
        public ActionResult InventoryReorderStatusReport(string Code)
        {
            ViewBag.SysModuleCode = Code;
            InventoryReorderStatusReportViewModel inventoryReorderStatusReportVM = new InventoryReorderStatusReportViewModel();
            inventoryReorderStatusReportVM.MaterialType = new MaterialTypeViewModel();
            inventoryReorderStatusReportVM.MaterialType.MaterialTypeSelectList = _materialTypeBusiness.GetMaterialTypeForSelectList();
            inventoryReorderStatusReportVM.Material = new MaterialViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            inventoryReorderStatusReportVM.Material.SelectList = new List<SelectListItem>();
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
            inventoryReorderStatusReportVM.Material.SelectList = selectListItem;



            return View(inventoryReorderStatusReportVM);
        }


        #region GetInventoryReorderStatusReport
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "InventoryReport", Mode = "R")]
        public JsonResult GetInventoryReorderStatusReport(DataTableAjaxPostModel model, InventoryReorderStatusReportViewModel inventoryReorderStatusVM)
        {         
            inventoryReorderStatusVM.DataTablePaging.Start = model.start;
            inventoryReorderStatusVM.DataTablePaging.Length = (inventoryReorderStatusVM.DataTablePaging.Length == 0 ? model.length : inventoryReorderStatusVM.DataTablePaging.Length);

            List<InventoryReorderStatusReportViewModel> inventoryReOrderList = Mapper.Map<List<InventoryReorderStatusReport>, List<InventoryReorderStatusReportViewModel>>(_reportBusiness.GetInventoryReorderStatusReport(Mapper.Map<InventoryReorderStatusReportViewModel, InventoryReorderStatusReport>(inventoryReorderStatusVM)));

            if (inventoryReorderStatusVM.DataTablePaging.Length == -1)
            {
                int totalResult = inventoryReOrderList.Count != 0 ? inventoryReOrderList[0].TotalCount : 0;
                int filteredResult = inventoryReOrderList.Count != 0 ? inventoryReOrderList[0].FilteredCount : 0;
                inventoryReOrderList = inventoryReOrderList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }
            var settings = new JsonSerializerSettings
            {
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
            };

            return Json(new
            {
                draw = model.draw,
                recordsTotal = inventoryReOrderList.Count != 0 ? inventoryReOrderList[0].TotalCount : 0,
                recordsFiltered = inventoryReOrderList.Count != 0 ? inventoryReOrderList[0].FilteredCount : 0,
                data = inventoryReOrderList
            });
        }
        #endregion GetInventoryReorderStatusReport

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "StockRegisterReport", Mode = "R")]
        public ActionResult StockRegisterReport(string Code)
        {
            ViewBag.SysModuleCode = Code;
            StockRegisterReportViewModel stockRegisterReportViewModelVM = new StockRegisterReportViewModel();
            stockRegisterReportViewModelVM.MaterialType = new MaterialTypeViewModel();
            stockRegisterReportViewModelVM.MaterialType.MaterialTypeSelectList = _materialTypeBusiness.GetMaterialTypeForSelectList();
            stockRegisterReportViewModelVM.Material = new MaterialViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            stockRegisterReportViewModelVM.Material.SelectList = new List<SelectListItem>();
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
            stockRegisterReportViewModelVM.Material.SelectList = selectListItem;
            return View(stockRegisterReportViewModelVM);
        }

        #region GetStockRegisterReport
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "StockRegisterReport", Mode = "R")]
        public JsonResult GetStockRegisterReport(DataTableAjaxPostModel model, StockRegisterReportViewModel stockRegisterReportVM)
        {
            stockRegisterReportVM.DataTablePaging.Start = model.start;
            stockRegisterReportVM.DataTablePaging.Length = (stockRegisterReportVM.DataTablePaging.Length == 0 ? model.length : stockRegisterReportVM.DataTablePaging.Length);

            List<StockRegisterReportViewModel> inventoryReOrderList = Mapper.Map<List<StockRegisterReport>, List<StockRegisterReportViewModel>>(_reportBusiness.GetStockRegisterReport(Mapper.Map<StockRegisterReportViewModel, StockRegisterReport>(stockRegisterReportVM)));

            if (stockRegisterReportVM.DataTablePaging.Length == -1)
            {
                int totalResult = inventoryReOrderList.Count != 0 ? inventoryReOrderList[0].TotalCount : 0;
                int filteredResult = inventoryReOrderList.Count != 0 ? inventoryReOrderList[0].FilteredCount : 0;
                inventoryReOrderList = inventoryReOrderList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }
            var settings = new JsonSerializerSettings
            {
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
            };

            return Json(new
            {
                draw = model.draw,
                recordsTotal = inventoryReOrderList.Count != 0 ? inventoryReOrderList[0].TotalCount : 0,
                recordsFiltered = inventoryReOrderList.Count != 0 ? inventoryReOrderList[0].FilteredCount : 0,
                data = inventoryReOrderList
            });
        }
        #endregion GetStockRegisterReport

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "StockLedgerReport", Mode = "R")]
        public ActionResult StockLedgerReport(string Code)
        {            
            ViewBag.SysModuleCode = Code;            
            StockLedgerReportViewModel stockLedgerReportVM = new StockLedgerReportViewModel();
            stockLedgerReportVM.MaterialType = new MaterialTypeViewModel();
            stockLedgerReportVM.MaterialType.MaterialTypeSelectList = _materialTypeBusiness.GetMaterialTypeForSelectList();
            stockLedgerReportVM.Material = new MaterialViewModel();
            List<SelectListItem> selectList = new List<SelectListItem>();
            foreach (Material material in _materialBusiness.GetMaterialForSelectList())
            {
                selectList.Add(new SelectListItem
                {
                    Text = material.MaterialCode + " - " + material.Description,
                    Value = material.ID.ToString(),
                    Selected = false,
                });
            }
            stockLedgerReportVM.Material.SelectList = selectList;
            return View(stockLedgerReportVM);
        }

        #region GetStockLedgerReport
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "StockLedgerReport", Mode = "R")]
        public JsonResult GetStockLedgerReport(DataTableAjaxPostModel model, StockLedgerReportViewModel stockLedgerReportVM)
        {
            Common con = new Common();
            DateTime dt = con.GetCurrentDateTime();
            if (stockLedgerReportVM != null)
            {
                if (stockLedgerReportVM.DateFilter == "30")
                {
                    stockLedgerReportVM.FromDate = dt.AddDays(-30).ToString("dd-MMM-yyyy");
                    stockLedgerReportVM.ToDate = dt.ToString("dd-MMM-yyyy");
                }
                if (stockLedgerReportVM.DateFilter == "60")
                {
                    stockLedgerReportVM.FromDate = dt.AddDays(-60).ToString("dd-MMM-yyyy");
                    stockLedgerReportVM.ToDate = dt.ToString("dd-MMM-yyyy");
                }
                if (stockLedgerReportVM.DateFilter == "90")
                {
                    stockLedgerReportVM.FromDate = dt.AddDays(-90).ToString("dd-MMM-yyyy");
                    stockLedgerReportVM.ToDate = dt.ToString("dd-MMM-yyyy");
                }
            }
            stockLedgerReportVM.DataTablePaging.Start = model.start;
            stockLedgerReportVM.DataTablePaging.Length = (stockLedgerReportVM.DataTablePaging.Length == 0 ? model.length : stockLedgerReportVM.DataTablePaging.Length);

            List<StockLedgerReportViewModel> stockLedgerList = Mapper.Map<List<StockLedgerReport>, List<StockLedgerReportViewModel>>(_reportBusiness.GetStockLedgerReport(Mapper.Map<StockLedgerReportViewModel, StockLedgerReport>(stockLedgerReportVM)));

            if (stockLedgerReportVM.DataTablePaging.Length == -1)
            {
                int totalResult = stockLedgerList.Count != 0 ? stockLedgerList[0].TotalCount : 0;
                int filteredResult = stockLedgerList.Count != 0 ? stockLedgerList[0].FilteredCount : 0;
                stockLedgerList = stockLedgerList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }
            var settings = new JsonSerializerSettings
            {
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
            };

            return Json(new
            {
                draw = model.draw,
                recordsTotal = stockLedgerList.Count != 0 ? stockLedgerList[0].TotalCount : 0,
                recordsFiltered = stockLedgerList.Count != 0 ? stockLedgerList[0].FilteredCount : 0,
                data = stockLedgerList
            });
        }
        #endregion GetStockLedgerReport

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "InventoryFGReport", Mode = "R")]
        public ActionResult InventoryReorderStatusFGReport(string Code)
        {
            ViewBag.SysModuleCode = Code;
            InventoryReOrderStatusFGReportViewModel inventoryReOrderStatusFGReportVM = new InventoryReOrderStatusFGReportViewModel();
            inventoryReOrderStatusFGReportVM.Product = new ProductViewModel();
            inventoryReOrderStatusFGReportVM.Product.ProductSelectList = _productBusiness.GetProductForSelectList();    

            return View(inventoryReOrderStatusFGReportVM);
        }

        #region GetInventoryReOrderStatusFGReport
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "InventoryFGReport", Mode = "R")]
        public JsonResult GetInventoryReOrderStatusFGReport(DataTableAjaxPostModel model, InventoryReOrderStatusFGReportViewModel inventoryReOrderStatusFGVM)
        {
            inventoryReOrderStatusFGVM.DataTablePaging.Start = model.start;
            inventoryReOrderStatusFGVM.DataTablePaging.Length = (inventoryReOrderStatusFGVM.DataTablePaging.Length == 0 ? model.length : inventoryReOrderStatusFGVM.DataTablePaging.Length);

            List<InventoryReOrderStatusFGReportViewModel> inventoryReOrderFGList = Mapper.Map<List<InventoryReOrderStatusFGReport>, List<InventoryReOrderStatusFGReportViewModel>>(_reportBusiness.GetInventoryReOrderStatusFGReport(Mapper.Map<InventoryReOrderStatusFGReportViewModel, InventoryReOrderStatusFGReport>(inventoryReOrderStatusFGVM)));

            if (inventoryReOrderStatusFGVM.DataTablePaging.Length == -1)
            {
                int totalResult = inventoryReOrderFGList.Count != 0 ? inventoryReOrderFGList[0].TotalCount : 0;
                int filteredResult = inventoryReOrderFGList.Count != 0 ? inventoryReOrderFGList[0].FilteredCount : 0;
                inventoryReOrderFGList = inventoryReOrderFGList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }
            var settings = new JsonSerializerSettings
            {
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
            };

            return Json(new
            {
                draw = model.draw,
                recordsTotal = inventoryReOrderFGList.Count != 0 ? inventoryReOrderFGList[0].TotalCount : 0,
                recordsFiltered = inventoryReOrderFGList.Count != 0 ? inventoryReOrderFGList[0].FilteredCount : 0,
                data = inventoryReOrderFGList
            });
        }
        #endregion GetInventoryReOrderStatusFGReport

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "StockRegisterFGReport", Mode = "R")]
        public ActionResult StockRegisterFGReport(string Code)
        {
            ViewBag.SysModuleCode = Code;
            StockRegisterFGReportViewModel stockRegisterFGReportVM = new StockRegisterFGReportViewModel();           
            return View(stockRegisterFGReportVM);
        }

        #region GetStockRegisterFGReport
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "StockRegisterFGReport", Mode = "R")]
        public JsonResult GetStockRegisterFGReport(DataTableAjaxPostModel model, StockRegisterFGReportViewModel stockRegisterFGReportVM)
        {
            stockRegisterFGReportVM.DataTablePaging.Start = model.start;
            stockRegisterFGReportVM.DataTablePaging.Length = (stockRegisterFGReportVM.DataTablePaging.Length == 0 ? model.length : stockRegisterFGReportVM.DataTablePaging.Length);

           // List<StockRegisterFGReportViewModel> stockRegisterFGList = Mapper.Map<List<StockRegisterFGReport>, List<StockRegisterFGReportViewModel>>(_reportBusiness.GetStockRegisterFGReport(Mapper.Map<StockRegisterFGReportViewModel, StockRegisterFGReport>(stockRegisterFGReportVM)));
            StockRegisterFGReportViewModel stockRegisterFGList = Mapper.Map<StockRegisterFGReport, StockRegisterFGReportViewModel>(_reportBusiness.GetStockRegisterFGReport(Mapper.Map<StockRegisterFGReportViewModel, StockRegisterFGReport>(stockRegisterFGReportVM)));

            if (stockRegisterFGReportVM.DataTablePaging.Length == -1)
            {
                int totalResult = stockRegisterFGList.StockRegisterFGReportList.Count != 0 ? stockRegisterFGList.StockRegisterFGReportList[0].TotalCount : 0;
                int filteredResult = stockRegisterFGList.StockRegisterFGReportList.Count != 0 ? stockRegisterFGList.StockRegisterFGReportList[0].FilteredCount : 0;
                stockRegisterFGList.StockRegisterFGReportList = stockRegisterFGList.StockRegisterFGReportList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }
            var settings = new JsonSerializerSettings
            {
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
            };

            return Json(new
            {
                draw = model.draw,
                recordsTotal = stockRegisterFGList.StockRegisterFGReportList.Count != 0 ? stockRegisterFGList.StockRegisterFGReportList[0].TotalCount : 0,
                recordsFiltered = stockRegisterFGList.StockRegisterFGReportList.Count != 0 ? stockRegisterFGList.StockRegisterFGReportList[0].FilteredCount : 0,
                data = stockRegisterFGList.StockRegisterFGReportList,
                TotalCostAmount= stockRegisterFGList.StockCostAmount,
                TotalSellingAmount= stockRegisterFGList.StockSellingAmount
            });
        }
        #endregion GetStockRegisterFGReport


        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "StockLedgerFGReport", Mode = "R")]
        public ActionResult StockLedgerFGReport(string Code)
        {
            ViewBag.SysModuleCode = Code;
            StockLedgerFGReportViewModel stockLedgerFGReportVM = new StockLedgerFGReportViewModel();           
            return View(stockLedgerFGReportVM);
        }


        #region GetStockLedgerFGReport
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "StockLedgerFGReport", Mode = "R")]
        public JsonResult GetStockLedgerFGReport(DataTableAjaxPostModel model, StockLedgerFGReportViewModel stockLedgerFGReportVM)
        {
            Common con = new Common();
            DateTime dt = con.GetCurrentDateTime();
            if (stockLedgerFGReportVM != null)
            {
                if (stockLedgerFGReportVM.DateFilter == "30")
                {
                    stockLedgerFGReportVM.FromDate = dt.AddDays(-30).ToString("dd-MMM-yyyy");
                    stockLedgerFGReportVM.ToDate = dt.ToString("dd-MMM-yyyy");
                }
                if (stockLedgerFGReportVM.DateFilter == "60")
                {
                    stockLedgerFGReportVM.FromDate = dt.AddDays(-60).ToString("dd-MMM-yyyy");
                    stockLedgerFGReportVM.ToDate = dt.ToString("dd-MMM-yyyy");
                }
                if (stockLedgerFGReportVM.DateFilter == "90")
                {
                    stockLedgerFGReportVM.FromDate = dt.AddDays(-90).ToString("dd-MMM-yyyy");
                    stockLedgerFGReportVM.ToDate = dt.ToString("dd-MMM-yyyy");
                }
            }
            stockLedgerFGReportVM.DataTablePaging.Start = model.start;
            stockLedgerFGReportVM.DataTablePaging.Length = (stockLedgerFGReportVM.DataTablePaging.Length == 0 ? model.length : stockLedgerFGReportVM.DataTablePaging.Length);

            List<StockLedgerFGReportViewModel> stockLedgerFGList = Mapper.Map<List<StockLedgerFGReport>, List<StockLedgerFGReportViewModel>>(_reportBusiness.GetStockLedgerFGReport(Mapper.Map<StockLedgerFGReportViewModel, StockLedgerFGReport>(stockLedgerFGReportVM)));

            if (stockLedgerFGReportVM.DataTablePaging.Length == -1)
            {
                int totalResult = stockLedgerFGList.Count != 0 ? stockLedgerFGList[0].TotalCount : 0;
                int filteredResult = stockLedgerFGList.Count != 0 ? stockLedgerFGList[0].FilteredCount : 0;
                stockLedgerFGList = stockLedgerFGList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }
            var settings = new JsonSerializerSettings
            {
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
            };

            return Json(new
            {
                draw = model.draw,
                recordsTotal = stockLedgerFGList.Count != 0 ? stockLedgerFGList[0].TotalCount : 0,
                recordsFiltered = stockLedgerFGList.Count != 0 ? stockLedgerFGList[0].FilteredCount : 0,
                data = stockLedgerFGList
            });
        }
        #endregion GetStockLedgerFGReport


        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ProductStageWiseStockReport", Mode = "R")]
        public ActionResult ProductStageWiseStockReport(string Code)
        {
            ViewBag.SysModuleCode = Code;
            ProductStageWiseStockReportViewModel productStageWiseStockReportVM = new ProductStageWiseStockReportViewModel();
            productStageWiseStockReportVM.Product = new ProductViewModel();
            productStageWiseStockReportVM.Product.ProductSelectList = _productBusiness.GetProductForSelectList("PRO");

            return View(productStageWiseStockReportVM);
        }

        #region GetProductStageWiseStockReport
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "ProductStageWiseStockReport", Mode = "R")]
        public JsonResult GetProductStageWiseStockReport(DataTableAjaxPostModel model, ProductStageWiseStockReportViewModel productStageWiseStockReportVM)
        {
            productStageWiseStockReportVM.DataTablePaging.Start = model.start;
            productStageWiseStockReportVM.DataTablePaging.Length = (productStageWiseStockReportVM.DataTablePaging.Length == 0 ? model.length : productStageWiseStockReportVM.DataTablePaging.Length);

            List<ProductStageWiseStockReportViewModel> productStagewiseReportList = Mapper.Map<List<ProductStageWiseStockReport>, List<ProductStageWiseStockReportViewModel>>(_reportBusiness.GetProductStageWiseStockReport(Mapper.Map<ProductStageWiseStockReportViewModel, ProductStageWiseStockReport>(productStageWiseStockReportVM)));

            if (productStageWiseStockReportVM.DataTablePaging.Length == -1)
            {
                int totalResult = productStagewiseReportList.Count != 0 ? productStagewiseReportList[0].TotalCount : 0;
                int filteredResult = productStagewiseReportList.Count != 0 ? productStagewiseReportList[0].FilteredCount : 0;
                productStagewiseReportList = productStagewiseReportList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }
            var settings = new JsonSerializerSettings
            {
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
            };

            return Json(new
            {
                draw = model.draw,
                recordsTotal = productStagewiseReportList.Count != 0 ? productStagewiseReportList[0].TotalCount : 0,
                recordsFiltered = productStagewiseReportList.Count != 0 ? productStagewiseReportList[0].FilteredCount : 0,
                data = productStagewiseReportList
            });
        }
        #endregion GetProductStageWiseStockReport


        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "DayBook", Mode = "R")]
        public ActionResult DayBookReport(string Code)
        {
            AppUA _appUA = Session["AppUA"] as AppUA;
            DateTime dt = _appUA.LoginDateTime;
            ViewBag.Date = dt.ToString("dd-MMM-yyyy");
            ViewBag.SysModuleCode = Code;
            DayBookViewModel dayBookReportVM = new DayBookViewModel();
            return View();
        }

        #region GetDayBookList
        [AuthSecurityFilter(ProjectObject = "DayBook", Mode = "R")]
        public string GetDayBookList(string date,string searchTerm)
        {
            try
            {
                DayBookViewModel dayBookVM = new DayBookViewModel();
                dayBookVM.DayBookList = Mapper.Map<List<DayBook>, List<DayBookViewModel>>(_reportBusiness.GetDayBook(date,searchTerm));

                return JsonConvert.SerializeObject(new { Result = "OK", Records = dayBookVM, Message = "Success" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = ex });
            }
        }
        #endregion GetDayBookList

        #region GetDayBookDetailByCode

        [AuthSecurityFilter(ProjectObject = "DayBook", Mode = "R")]
        public string GetDayBookDetailByCode(string code, string date)
        {
            try
            {
                DataSet ds = new DataSet();
                ds=_reportBusiness.GetDayBookDetailByCode(code,date); 
                return JsonConvert.SerializeObject(new { Result = "OK", Records = ds, Message = "Success" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = ex });
            }
        }

        #endregion GetDayBookDetailByCode

        //SalesAnalysisReport
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SalesAnalysisReport", Mode = "R")]
        public ActionResult SalesAnalysisReport(string Code)
        {
            
            Common con = new Common();
            DateTime dt = con.GetCurrentDateTime();
            ViewBag.Date = dt.ToString("dd-MMM-yyyy");
            ViewBag.SysModuleCode = Code;
            SalesAnalysisReportViewModel salesAnalysisReportVM = new SalesAnalysisReportViewModel();
            return View();
        }

        //GetSalesAnalysisReport
        #region GetSalesAnalysisReport

        [AuthSecurityFilter(ProjectObject = "SalesAnalysisReport", Mode = "R")]
        public string GetSalesAnalysisReport(string IsInvoicedOnly, string FromDate, string ToDate, string DateFilter)
        {
            try
            {
                DataSet ds = new DataSet();
                Common con = new Common();
                DateTime dt = con.GetCurrentDateTime();
             
                if (DateFilter == "30")
                {
                    FromDate = dt.AddDays(-30).ToString("dd-MMM-yyyy");
                    ToDate = dt.ToString("dd-MMM-yyyy");
                }
                if (DateFilter == "60")
                {
                    FromDate = dt.AddDays(-60).ToString("dd-MMM-yyyy");
                    ToDate = dt.ToString("dd-MMM-yyyy");
                }
                if (DateFilter == "90")
                {
                    FromDate = dt.AddDays(-90).ToString("dd-MMM-yyyy");
                    ToDate = dt.ToString("dd-MMM-yyyy");
                }
              
                ds =_reportBusiness.GetSalesAnalysisReport(IsInvoicedOnly,FromDate,ToDate);
                return JsonConvert.SerializeObject(new { Result = "OK", Records = ds, Message = "Success" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = ex });
            }
        }

        #endregion GetSalesAnalysisReport

        //MovementAnalysisReport
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "MovementAnalysisReport", Mode = "R")]
        public ActionResult MovementAnalysisReport(string Code)
        {
            AppUA _appUA = Session["AppUA"] as AppUA;
            DateTime dt = _appUA.LoginDateTime;
            ViewBag.Date = dt.ToString("dd-MMM-yyyy");
            ViewBag.SysModuleCode = Code;
            MovementAnalysisReportViewModel movementAnalysisReportVM = new MovementAnalysisReportViewModel();

            movementAnalysisReportVM.ProductSelectList = _productBusiness.GetProductForSelectList();
            movementAnalysisReportVM.EmployeeSelectList = _employeeBusiness.GetEmployeeSelectList();

            return View(movementAnalysisReportVM);
        }

        //MovementAnalysisReport
        #region MovementAnalysisReport

        [AuthSecurityFilter(ProjectObject = "MovementAnalysisReport", Mode = "R")]
        public string GetMovementAnalysisReport(string SalesPerson, string FromDate, string ToDate, string MonthFilter,string ProductID)
        {
            try
            {
                DataSet ds = new DataSet();
                Common con = new Common();
                DateTime dt = con.GetCurrentDateTime();
                MovementAnalysisReportViewModel movementAnalysisReportVM = new MovementAnalysisReportViewModel();
                if(ProductID!=null && ProductID!="")
                movementAnalysisReportVM.ProductID = Guid.Parse(ProductID);
                if(SalesPerson != null && SalesPerson != "")
                    movementAnalysisReportVM.EmployeeID = Guid.Parse(SalesPerson);
                movementAnalysisReportVM.FromDate = FromDate;
                movementAnalysisReportVM.ToDate = ToDate;
                movementAnalysisReportVM.MonthFilter = MonthFilter;

                ds=_reportBusiness.GetMovementAnalysisReport(Mapper.Map<MovementAnalysisReportViewModel, MovementAnalysisReport>(movementAnalysisReportVM));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = ds, Message = "Success" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = ex });
            }
        }

        #endregion MovementAnalysisReport


        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SalesRegisterReport", Mode = "R")]
        public ActionResult SalesRegisterReport(string Code)
        {
            ViewBag.SysModuleCode = Code;
            SalesRegisterReportViewModel salesRegisterReportVM = new SalesRegisterReportViewModel();
            salesRegisterReportVM.Customer = new CustomerViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            salesRegisterReportVM.Customer.SelectList = new List<SelectListItem>();
            List<CustomerViewModel> customerList = Mapper.Map<List<Customer>, List<CustomerViewModel>>(_customerBusiness.GetCustomerForSelectList());
            if (customerList != null)
                foreach (CustomerViewModel customer in customerList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = customer.CompanyName,
                        Value = customer.ID.ToString(),
                        Selected = false
                    });
                }
            salesRegisterReportVM.Customer.SelectList = selectListItem;

            return View(salesRegisterReportVM);
        }
            #region GetSalesRegisterReport
            [HttpPost]
        [AuthSecurityFilter(ProjectObject = "SalesRegisterReport", Mode = "R")]
        public JsonResult GetSalesRegisterReport(DataTableAjaxPostModel model, SalesRegisterReportViewModel salesRegisterReportVM)
        {
            salesRegisterReportVM.DataTablePaging.Start = model.start;
            salesRegisterReportVM.DataTablePaging.Length = (salesRegisterReportVM.DataTablePaging.Length == 0 ? model.length : salesRegisterReportVM.DataTablePaging.Length);

            List<SalesRegisterReportViewModel> salesRegisterReportList = Mapper.Map<List<SalesRegisterReport>, List<SalesRegisterReportViewModel>>(_reportBusiness.GetSalesRegisterReport(Mapper.Map<SalesRegisterReportViewModel, SalesRegisterReport>(salesRegisterReportVM)));

            if (salesRegisterReportVM.DataTablePaging.Length == -1)
            {
                int totalResult = salesRegisterReportList.Count != 0 ? salesRegisterReportList[0].TotalCount : 0;
                int filteredResult = salesRegisterReportList.Count != 0 ? salesRegisterReportList[0].FilteredCount : 0;
                salesRegisterReportList = salesRegisterReportList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }
            var settings = new JsonSerializerSettings
            {
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
            };

            return Json(new
            {
                draw = model.draw,
                recordsTotal = salesRegisterReportList.Count != 0 ? salesRegisterReportList[0].TotalCount : 0,
                recordsFiltered = salesRegisterReportList.Count != 0 ? salesRegisterReportList[0].FilteredCount : 0,
                data = salesRegisterReportList
            });
        }
        #endregion GetSalesRegisterReport

        public void DownloadExcel(ExcelExportViewModel excelExportVM)
        {
            try
            {
                string fileName = "";
                Common common = new Common();
                ExcelPackage excel = new ExcelPackage();
                object ResultFromJS = null;
                string ReadableFormat = null;
                switch (excelExportVM.DocumentType)
                {
                    case "REQ":
                        fileName = "RequisitionSummaryReport" + common.GetCurrentDateTime().ToString("dd|MMM|yy|hh:mm:ss");
                        RequisitionSummaryReportViewModel requisitionSummaryReportVM = new RequisitionSummaryReportViewModel();
                        ResultFromJS = JsonConvert.DeserializeObject(excelExportVM.AdvanceSearch);
                        ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                        requisitionSummaryReportVM = JsonConvert.DeserializeObject<RequisitionSummaryReportViewModel>(ReadableFormat);
                      
                        List<RequisitionViewModel> requisitionSummaryReportList = Mapper.Map<List<Requisition>, List<RequisitionViewModel>>(_reportBusiness.GetRequisitionSummaryReport(Mapper.Map<RequisitionSummaryReportViewModel, RequisitionSummaryReport>(requisitionSummaryReportVM)));
                       
                        var requisitionsummaryreportworkSheet = excel.Workbook.Worksheets.Add("RequisitionSummaryReport");
                        RequisitionViewModel[] requisitionSummaryReportVMListArray = requisitionSummaryReportList.ToArray();
                        requisitionsummaryreportworkSheet.Cells[1, 1].LoadFromCollection(requisitionSummaryReportVMListArray.Select(x => new
                        {
                            RequisitionNo = x.RequisitionNo,
                            RequisitionDate = x.ReqDateFormatted,
                            RequisitionBy = x.RequisitionBy,
                            Title = x.Title,
                            RequisitionStatus = x.ReqStatus,
                            Amount = x.ReqAmount,
                            RequiredDate = x.RequiredDateFormatted
                           
                        }), true, TableStyles.Light1);
                        requisitionsummaryreportworkSheet.Column(1).AutoFit();
                        requisitionsummaryreportworkSheet.Column(2).AutoFit();
                        requisitionsummaryreportworkSheet.Column(3).AutoFit();
                        requisitionsummaryreportworkSheet.Column(4).Width = 40;
                        requisitionsummaryreportworkSheet.Column(5).AutoFit();
                        requisitionsummaryreportworkSheet.Column(6).AutoFit();
                        requisitionsummaryreportworkSheet.Column(7).AutoFit();
                      
                        break;
                    case "PO":
                        fileName = "PurchaseSummaryReport" + common.GetCurrentDateTime().ToString("dd|MMM|yy|hh:mm:ss");
                        PurchaseSummaryReportViewModel purchaseSummaryReportVM = new PurchaseSummaryReportViewModel();
                        ResultFromJS = JsonConvert.DeserializeObject(excelExportVM.AdvanceSearch);
                        ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                        purchaseSummaryReportVM = JsonConvert.DeserializeObject<PurchaseSummaryReportViewModel>(ReadableFormat);
                        List<PurchaseOrderViewModel> purchaseSummaryReportList = Mapper.Map<List<PurchaseOrder>, List<PurchaseOrderViewModel>>(_reportBusiness.GetPurchaseSummaryReport(Mapper.Map<PurchaseSummaryReportViewModel, PurchaseSummaryReport>(purchaseSummaryReportVM)));
                        var purchasesummaryreportworkSheet = excel.Workbook.Worksheets.Add("PurchaseSummaryReport");
                        PurchaseOrderViewModel[] purchaseSummaryReportVMListArray = purchaseSummaryReportList.ToArray();
                        purchasesummaryreportworkSheet.Cells[1, 1].LoadFromCollection(purchaseSummaryReportVMListArray.Select(x => new
                        {
                          PurchaseOrderNo = x.PONo,
                          PurchaseOrderDate = x.PurchaseOrderDateFormatted,
                          PurchaseOrderIssuedDate = x.PurchaseOrderIssuedDateFormatted,
                          Title = x.PurchaseOrderTitle,
                          Supplier = x.Supplier,
                          Status = x.PurchaseOrderStatus,
                          OrderMailed = (x.EmailSentYN=="True" ? "YES" : "NO"),
                          TotalPOValue = x.GrossAmount                          

                        }), true, TableStyles.Light1);
                        purchasesummaryreportworkSheet.Column(1).AutoFit();
                        purchasesummaryreportworkSheet.Column(2).AutoFit();
                        purchasesummaryreportworkSheet.Column(3).AutoFit();
                        purchasesummaryreportworkSheet.Column(4).Width = 40;
                        purchasesummaryreportworkSheet.Column(5).Width = 40;
                        purchasesummaryreportworkSheet.Column(6).AutoFit();
                        purchasesummaryreportworkSheet.Column(7).AutoFit();
                        purchasesummaryreportworkSheet.Column(8).AutoFit();
                        break;
                    case "POD":
                        fileName = "PurchaseDetailReport" + common.GetCurrentDateTime().ToString("dd|MMM|yy|hh:mm:ss");
                        PurchaseDetailReportViewModel purchaseDetailReportVM = new PurchaseDetailReportViewModel();
                        ResultFromJS = JsonConvert.DeserializeObject(excelExportVM.AdvanceSearch);
                        ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                        purchaseDetailReportVM = JsonConvert.DeserializeObject<PurchaseDetailReportViewModel>(ReadableFormat);
                        List<PurchaseDetailReportViewModel> purchaseDetailReportList = Mapper.Map<List<PurchaseDetailReport>, List<PurchaseDetailReportViewModel>>(_reportBusiness.GetPurchaseDetailReport(Mapper.Map<PurchaseDetailReportViewModel, PurchaseDetailReport>(purchaseDetailReportVM)));
                        var purchasedetailreportworkSheet = excel.Workbook.Worksheets.Add("PurchaseDetailReport");
                        PurchaseDetailReportViewModel[] purchaseDetailReportVMListArray = purchaseDetailReportList.ToArray();
                        purchasedetailreportworkSheet.Cells[1, 1].LoadFromCollection(purchaseDetailReportVMListArray.Select(x => new
                        {
                            PurchaseOrderNo = x.PONo,
                            PurchaseOrderDate = x.PurchaseOrderDateFormatted,
                            Item = x.Material.Description,
                            Unit= x.Material.UnitCode,
                            OrderedQty=x.POQty,
                            DeliveredQty =x.PrevRcvQty,
                            Status = x.Status,                           
                            Supplier = x.Supplier.CompanyName,
                            ApprovalStatus = x.ApprovalStatus,
                            DeliveryStatus = x.DeliveryStatus                          

                        }), true, TableStyles.Light1);
                        purchasedetailreportworkSheet.Column(1).AutoFit();
                        purchasedetailreportworkSheet.Column(2).AutoFit();
                        purchasedetailreportworkSheet.Column(3).Width = 40;
                        purchasedetailreportworkSheet.Column(4).AutoFit();
                        purchasedetailreportworkSheet.Column(5).AutoFit();
                        purchasedetailreportworkSheet.Column(6).AutoFit();
                        purchasedetailreportworkSheet.Column(7).AutoFit();
                        purchasedetailreportworkSheet.Column(8).Width = 30;
                        purchasedetailreportworkSheet.Column(9).Width = 40;
                        purchasedetailreportworkSheet.Column(10).Width = 40;

                        break;
                    case "POR":
                        fileName = "PurchaseRegisterReport" + common.GetCurrentDateTime().ToString("dd|MMM|yy|hh:mm:ss");
                        PurchaseRegisterReportViewModel purchaseRegisterReportVM = new PurchaseRegisterReportViewModel();
                        ResultFromJS = JsonConvert.DeserializeObject(excelExportVM.AdvanceSearch);
                        ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                        purchaseRegisterReportVM = JsonConvert.DeserializeObject<PurchaseRegisterReportViewModel>(ReadableFormat);
                        List<PurchaseRegisterReportViewModel> purchaseRegisterList = Mapper.Map<List<PurchaseRegisterReport>, List<PurchaseRegisterReportViewModel>>(_reportBusiness.GetPurchaseRegisterReport(Mapper.Map<PurchaseRegisterReportViewModel, PurchaseRegisterReport>(purchaseRegisterReportVM)));
                        var purchaseregisterreportworkSheet = excel.Workbook.Worksheets.Add("PurchaseRegisterReport");
                        PurchaseRegisterReportViewModel[] purchaseRegisterReportVMListArray = purchaseRegisterList.ToArray();
                        purchaseregisterreportworkSheet.Cells[1, 1].LoadFromCollection(purchaseRegisterReportVMListArray.Select(x => new
                        {
                            PurchaseOrderNo = x.PONo,
                            PurchaseOrderDate = x.PurchaseOrderDateFormatted,                           
                            Supplier = x.Supplier.CompanyName,
                            Discount=x.Discount,
                            GSTPercent=x.GSTPerc,
                            GSTAmount=x.GSTAmt,
                            NetAmount=x.NetAmount,
                            TaxableAmount=x.TaxableAmount,
                            GrossAmount=x.GrossAmount,
                            InvoiceAmount=x.InvoicedAmount,
                            PaidAmount=x.PaidAmount
                                                      
                          

                        }), true, TableStyles.Light1);
                        purchaseregisterreportworkSheet.Column(1).AutoFit();
                        purchaseregisterreportworkSheet.Column(2).AutoFit();
                        purchaseregisterreportworkSheet.Column(3).Width = 40;
                        purchaseregisterreportworkSheet.Column(4).AutoFit();
                        purchaseregisterreportworkSheet.Column(5).AutoFit();
                        purchaseregisterreportworkSheet.Column(6).AutoFit();
                        purchaseregisterreportworkSheet.Column(7).AutoFit();
                        purchaseregisterreportworkSheet.Column(8).AutoFit();
                        purchaseregisterreportworkSheet.Column(9).AutoFit();
                        purchaseregisterreportworkSheet.Column(10).AutoFit();
                        purchaseregisterreportworkSheet.Column(11).AutoFit();
                        break;

                    case "INVM":
                        fileName = "InventoryReorderStatusReport" + common.GetCurrentDateTime().ToString("dd|MMM|yy|hh:mm:ss");
                        InventoryReorderStatusReportViewModel inventoryReorderStatusVM = new InventoryReorderStatusReportViewModel();
                        ResultFromJS = JsonConvert.DeserializeObject(excelExportVM.AdvanceSearch);
                        ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                        inventoryReorderStatusVM = JsonConvert.DeserializeObject<InventoryReorderStatusReportViewModel>(ReadableFormat);
                        List<InventoryReorderStatusReportViewModel> inventoryReOrderList = Mapper.Map<List<InventoryReorderStatusReport>, List<InventoryReorderStatusReportViewModel>>(_reportBusiness.GetInventoryReorderStatusReport(Mapper.Map<InventoryReorderStatusReportViewModel, InventoryReorderStatusReport>(inventoryReorderStatusVM)));

                        var inventoryreorderreportworkSheet = excel.Workbook.Worksheets.Add("InventoryReorderStatusReport");
                        InventoryReorderStatusReportViewModel[] inventoryReorderReportVMListArray = inventoryReOrderList.ToArray();
                        inventoryreorderreportworkSheet.Cells[1, 1].LoadFromCollection(inventoryReorderReportVMListArray.Select(x => new
                        {
                            Item = x.Description,
                            CurrentStock = x.CurrentStock,
                            PODueQty = x.PODueQty,
                            NetAvailable = x.NetAvailableQty,
                            ReorderLevel = x.ReorderQty,
                            ShortFall=x.ShortFall

                        }), true, TableStyles.Light1);
                        inventoryreorderreportworkSheet.Column(1).Width = 40;
                        inventoryreorderreportworkSheet.Column(2).AutoFit();
                        inventoryreorderreportworkSheet.Column(3).AutoFit();
                        inventoryreorderreportworkSheet.Column(4).AutoFit();
                        inventoryreorderreportworkSheet.Column(5).AutoFit();
                        inventoryreorderreportworkSheet.Column(6).AutoFit();                        
                        break;
                    default: break;
                }
                using (var memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=" + fileName + ".xlsx");
                    excel.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                    memoryStream.Close();
                    memoryStream.Dispose();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }




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
                case "ListDayBook":
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetReportList();";
                    ////----added for export button--------------
                    //toolboxVM.PrintBtn.Visible = true;
                    //toolboxVM.PrintBtn.Text = "Export";
                    //toolboxVM.PrintBtn.Title = "Export";
                    //toolboxVM.PrintBtn.Event = "ExportReportData();";
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