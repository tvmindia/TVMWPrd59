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
    public class PurchaseOrderController : Controller
    {
        // GET: PurchaseOrder
        private IPurchaseOrderBusiness _purchaseOrderBusiness;
        private ISupplierBusiness _supplierBusiness;
        private IRequisitionBusiness _requisitionBusiness;
        Common _common = new Common();
        public PurchaseOrderController(IPurchaseOrderBusiness purchaseOrderBusiness, ISupplierBusiness supplierBusiness, IRequisitionBusiness requisitionBusiness)
        {
            _purchaseOrderBusiness = purchaseOrderBusiness;
            _supplierBusiness = supplierBusiness;
            _requisitionBusiness = requisitionBusiness;
        }
        [AuthSecurityFilter(ProjectObject = "PurchaseOrder", Mode = "R")]
        public ActionResult ViewPurchaseOrder(string code)
        {
            ViewBag.SysModuleCode = code;
            return View();
        }
        [AuthSecurityFilter(ProjectObject = "PurchaseOrder", Mode = "R")]
        public ActionResult NewPurchaseOrder(string code)
        {
            ViewBag.SysModuleCode = code;
            PurchaseOrderViewModel purchaseOrderVM = new PurchaseOrderViewModel();
            return View();
        }

        #region GetAllPurchaseOrder
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "PurchaseOrder", Mode = "R")]
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
        #region GetAllRequisitionForPurchaseOrder
        public JsonResult GetAllRequisitionForPurchaseOrder(DataTableAjaxPostModel model, RequisitionAdvanceSearchViewModel requisitionAdvanceSearchVM)
        {
            requisitionAdvanceSearchVM.DataTablePaging.Start = model.start;
            requisitionAdvanceSearchVM.DataTablePaging.Length = (requisitionAdvanceSearchVM.DataTablePaging.Length == 0 ? model.length : requisitionAdvanceSearchVM.DataTablePaging.Length);
            List<RequisitionViewModel> requisitionOrderList = Mapper.Map<List<Requisition>, List<RequisitionViewModel>>(_requisitionBusiness.GetAllRequisitionForPurchaseOrder(Mapper.Map<RequisitionAdvanceSearchViewModel, RequisitionAdvanceSearch>(requisitionAdvanceSearchVM)));


            return Json(new
            {
                draw = model.draw,
                recordsTotal = requisitionOrderList.Count != 0 ? requisitionOrderList[0].TotalCount : 0,
                recordsFiltered = requisitionOrderList.Count != 0 ? requisitionOrderList[0].FilteredCount : 0,
                data = requisitionOrderList
            });
        }


        #endregion GetAllRequisitionForPurchaseOrder

        #region GetRequisitionDetailsByIDs
        [AuthSecurityFilter(ProjectObject = "PurchaseOrder", Mode = "R")]
        public string GetRequisitionDetailsByIDs(string IDs, string POID)
        {
            try
            {
                List<RequisitionDetailViewModel> purchaseOrderRequisitionList = Mapper.Map<List<RequisitionDetail>, List<RequisitionDetailViewModel>>(_requisitionBusiness.GetRequisitionDetailsByIDs(IDs, POID));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = purchaseOrderRequisitionList });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex });
            }
        }
        #endregion GetRequisitionDetailsByIDs

        #region PurchaseOrder Dropdown
        public ActionResult PurchaseOrderDropdown()
        {
            PurchaseOrderViewModel purchaseOrderVM = new PurchaseOrderViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            purchaseOrderVM.SelectList=new List<SelectListItem>();
            List<PurchaseOrderViewModel> purchaseOrderList = Mapper.Map<List<PurchaseOrder>, List<PurchaseOrderViewModel>>(_purchaseOrderBusiness.GetAllPurchaseOrderForSelectList());
            if (purchaseOrderList != null)
                foreach (PurchaseOrderViewModel purchaseOrder in purchaseOrderList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = purchaseOrder.PurchaseOrderNo,
                        Value = purchaseOrder.ID.ToString(),
                        Selected = false
                    });
                }
            purchaseOrderVM.SelectList = selectListItem;
            return PartialView("_PurchaseOrderDropdown", purchaseOrderVM);
        }
        #endregion
        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "PurchaseOrder", Mode = "R")]
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