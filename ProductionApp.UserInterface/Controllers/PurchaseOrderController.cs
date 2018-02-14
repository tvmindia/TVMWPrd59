using AutoMapper;
using Newtonsoft.Json;
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
    public class PurchaseOrderController : Controller
    {
        // GET: PurchaseOrder
        private IPurchaseOrderBusiness _purchaseOrderBusiness;
        private ISupplierBusiness _supplierBusiness;
        Common _common = new Common();
        public PurchaseOrderController(IPurchaseOrderBusiness purchaseOrderBusiness, ISupplierBusiness supplierBusiness)
        {
            _purchaseOrderBusiness = purchaseOrderBusiness;
            _supplierBusiness = supplierBusiness;
        }
        public ActionResult ViewPurchaseOrder(string code)
        {
            ViewBag.SysModuleCode = code;
            //ViewBag.toDate = (_common.GetCurrentDateTime()).ToString("dd-MMM-yyyy");
            //ViewBag.fromDate=_common.
            PurchaseOrderAdvanceSearchViewModel purchaseOrderAdvanceSearchVM = new PurchaseOrderAdvanceSearchViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            selectListItem = new List<SelectListItem>();
            selectListItem.Add(new SelectListItem { Text = "Open", Value = "Open", Selected = true });
            selectListItem.Add(new SelectListItem { Text = "Closed", Value = "Closed", Selected = false });
            selectListItem.Add(new SelectListItem { Text = "All", Value = "ALL", Selected = false });
            purchaseOrderAdvanceSearchVM.Status = selectListItem;

            purchaseOrderAdvanceSearchVM.Supplier = new SupplierViewModel();
            purchaseOrderAdvanceSearchVM.Supplier.SupplierList = new List<SelectListItem>();
            selectListItem = new List<SelectListItem>();
            List<SupplierViewModel> supplierList = Mapper.Map<List<Supplier>, List<SupplierViewModel>>(_supplierBusiness.GetAllSupplier());
            foreach(SupplierViewModel supplier in supplierList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = supplier.CompanyName,
                    Value = supplier.ID.ToString(),
                    Selected = false
                });
            }
            purchaseOrderAdvanceSearchVM.Supplier.SupplierList = selectListItem;
            return View(purchaseOrderAdvanceSearchVM);
        }
        #region GetAllPurchaseOrder
        [HttpPost]
        public JsonResult GetAllPurchaseOrder(DataTableAjaxPostModel model, PurchaseOrderAdvanceSearchViewModel purchaseOrderAdvanceSearchVM)
        {
            purchaseOrderAdvanceSearchVM.DataTablePaging.Start = model.start;
            purchaseOrderAdvanceSearchVM.DataTablePaging.Length = (purchaseOrderAdvanceSearchVM.DataTablePaging.Length == 0 ? model.length : purchaseOrderAdvanceSearchVM.DataTablePaging.Length);
            List<PurchaseOrderViewModel> purchaseOrderList = Mapper.Map<List<PurchaseOrder>, List<PurchaseOrderViewModel>>(_purchaseOrderBusiness.GetAllPurchaseOrder(Mapper.Map<PurchaseOrderAdvanceSearchViewModel, PurchaseOrderAdvanceSearch>(purchaseOrderAdvanceSearchVM)));
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.None
            };
            return Json(new
            {
                draw = model.draw,
            recordsTotal = purchaseOrderList.Count != 0 ? purchaseOrderList[0].TotalCount : 0,
            recordsFiltered = purchaseOrderList.Count != 0 ? purchaseOrderList[0].FilteredCount : 0,
            data= purchaseOrderList
            
        });
        }
        #endregion GetAllPurchaseOrder
        #region ButtonStyling
        [HttpGet]
        //[AuthSecurityFilter(ProjectObject = "Bank", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("AddPurchaseOrder", "PurchaseOrder", new { code = "PURCH" });
                    toolboxVM.addbtn.Event = "";
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetPurchaseOrderList();";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "ImportPurchaseOrderData();";
                    //---------------------------------------

                    break;
                case "Edit":

                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "New";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "openNav();";

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save Bank";
                    toolboxVM.savebtn.Event = "Save();";

                    toolboxVM.deletebtn.Visible = true;
                    toolboxVM.deletebtn.Text = "Delete";
                    toolboxVM.deletebtn.Title = "Delete Bank";
                    toolboxVM.deletebtn.Event = "Delete()";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "Reset();";

                    toolboxVM.CloseBtn.Visible = true;
                    toolboxVM.CloseBtn.Text = "Close";
                    toolboxVM.CloseBtn.Title = "Close";
                    toolboxVM.CloseBtn.Event = "closeNav();";

                    break;
                case "Add":

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "Save();";

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Event = "";
                    toolboxVM.ListBtn.Href = Url.Action("ViewPurchaseOrder", "PurchaseOrder", new { Code = "PURCH" });


                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion
    }
}