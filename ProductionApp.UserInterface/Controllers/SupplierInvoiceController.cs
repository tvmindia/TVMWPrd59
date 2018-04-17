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
    public class SupplierInvoiceController : Controller
    {
      
        private ISupplierInvoiceBusiness _supplierInvoiceBusiness;
        private ISupplierBusiness _supplierBusiness;
        private IChartOfAccountBusiness _chartOfAccountBusiness;
        private IPaymentTermBusiness _paymentTermBusiness;

        Common _common = new Common();
        AppConst _appConst = new AppConst();

        public SupplierInvoiceController(ISupplierInvoiceBusiness supplierInvoiceBusiness, ISupplierBusiness supplierBusiness, IChartOfAccountBusiness chartOfAccountBusiness, IPaymentTermBusiness paymentTermBusiness)
        {
            _supplierInvoiceBusiness = supplierInvoiceBusiness;
            _supplierBusiness = supplierBusiness;
            _chartOfAccountBusiness = chartOfAccountBusiness;
            _paymentTermBusiness = paymentTermBusiness;
        }
        // GET: SupplierInvoice

        [AuthSecurityFilter(ProjectObject = "SupplierInvoice", Mode = "R")]
        public ActionResult ViewSupplierInvoice(string code)
        {
            ViewBag.SysModuleCode = code;
            SupplierInvoiceAdvanceSearchViewModel supplierInvoiceAdvanceSearchVM = new SupplierInvoiceAdvanceSearchViewModel();
            supplierInvoiceAdvanceSearchVM.Supplier = new SupplierViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            supplierInvoiceAdvanceSearchVM.Supplier.SelectList = new List<SelectListItem>();
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
            supplierInvoiceAdvanceSearchVM.Supplier.SelectList= selectListItem;
            return View(supplierInvoiceAdvanceSearchVM);
        }
        [AuthSecurityFilter(ProjectObject = "SupplierInvoice", Mode = "R")]
        public ActionResult NewSupplierInvoice(string code, Guid? id)
        {
            ViewBag.SysModuleCode = code;
            SupplierInvoiceViewModel supplierInvoiceVM = new SupplierInvoiceViewModel
            {
                ID = id == null ? Guid.Empty : (Guid)id,
                IsUpdate = id == null ? false : true,
            };
            supplierInvoiceVM.Supplier = new SupplierViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            supplierInvoiceVM.Supplier.SelectList = new List<SelectListItem>();
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
            supplierInvoiceVM.Supplier.SelectList = selectListItem;

            supplierInvoiceVM.chartOfAccount = new ChartOfAccountViewModel()
            {
                ChartOfAccountSelectList = _chartOfAccountBusiness.GetChartOfAccountForSelectList()
            };
            return View(supplierInvoiceVM);
        }


        #region GetAllSupplierInvoice
        [AuthSecurityFilter(ProjectObject = "SupplierInvoice", Mode = "R")]
        public JsonResult GetAllSupplierInvoice(DataTableAjaxPostModel model, SupplierInvoiceAdvanceSearchViewModel supplierInvoiceAdvanceSearchVM)
        {
            supplierInvoiceAdvanceSearchVM.DataTablePaging.Start = model.start;
            supplierInvoiceAdvanceSearchVM.DataTablePaging.Length = (supplierInvoiceAdvanceSearchVM.DataTablePaging.Length == 0 ? model.length : supplierInvoiceAdvanceSearchVM.DataTablePaging.Length);
            List<SupplierInvoiceViewModel> supplierInvoiceList = Mapper.Map<List<SupplierInvoice>, List<SupplierInvoiceViewModel>>(_supplierInvoiceBusiness.GetAllSupplierInvoice(Mapper.Map<SupplierInvoiceAdvanceSearchViewModel, SupplierInvoiceAdvanceSearch>(supplierInvoiceAdvanceSearchVM)));
            return Json(new
            {
                draw = model.draw,
                recordsTotal = supplierInvoiceList.Count != 0 ? supplierInvoiceList[0].TotalCount : 0,
                recordsFiltered = supplierInvoiceList.Count != 0 ? supplierInvoiceList[0].FilteredCount : 0,
                data = supplierInvoiceList
            });
        }
        #endregion GetAllSupplierInvoice

        #region GetDueDate
        [AuthSecurityFilter(ProjectObject = "SupplierInvoice", Mode = "R")]
        public string GetDueDate(string Code, string InvoiceDate = "")
        {
            try
            {
                string PaymentDueDate;
                DateTime Datenow = _common.GetCurrentDateTime();
                PaymentTermViewModel payTermsObj = Mapper.Map<PaymentTerm, PaymentTermViewModel>(_paymentTermBusiness.GetPaymentTermDetails(Code));
                if (InvoiceDate == "")
                {
                    PaymentDueDate = Datenow.AddDays(payTermsObj.NoOfDays).ToString("dd-MMM-yyyy");
                }
                else
                {
                    PaymentDueDate = Convert.ToDateTime(InvoiceDate).AddDays(payTermsObj.NoOfDays).ToString("dd-MMM-yyyy");
                }

                return JsonConvert.SerializeObject(new { Result = "OK", Records = PaymentDueDate });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion GetDueDate

        #region GetCustomerDetails
        [AuthSecurityFilter(ProjectObject = "SupplierInvoice", Mode = "R")]
        public string GetSupplierDetails(string supplierId)
        {
            try
            {
                SupplierViewModel supplierVM = Mapper.Map<Supplier, SupplierViewModel>(_supplierBusiness.GetSupplier(Guid.Parse(supplierId)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = supplierVM, Message = "Success" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = ex });
            }
        }
        #endregion GetCustomerDetails

        #region ButtonStyling
        [HttpGet]
        //[AuthSecurityFilter(ProjectObject = "SupplierInvoice", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewSupplierInvoice", "SupplierInvoice", new { Code = "ACC" });
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "BindOrReloadSupplierInvoiceTable('Reset');";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "BindOrReloadSupplierInvoiceTable('Export');";
                    //---------------------------------------

                    break;
                case "Edit":

                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "New";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewSupplierInvoice", "SupplierInvoice", new { Code = "ACC" });

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "Save();";

                    toolboxVM.deletebtn.Visible = true;
                    toolboxVM.deletebtn.Text = "Delete";
                    toolboxVM.deletebtn.Title = "Delete";
                    toolboxVM.deletebtn.Event = "DeleteClick()";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "Reset();";

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Href = Url.Action("ViewSupplierInvoice", "SupplierInvoice", new { Code = "ACC" });

                    break;

                case "Add":

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "Save();";

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Href = Url.Action("ViewSupplierInvoice", "SupplierInvoice", new { Code = "ACC" });
                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion
    }
}