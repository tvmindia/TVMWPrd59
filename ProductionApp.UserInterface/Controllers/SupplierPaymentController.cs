using AutoMapper;
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
    public class SupplierPaymentController : Controller
    {
        private ISupplierPaymentBusiness _supplierPaymentBusiness;
        private ISupplierBusiness _supplierBusiness;
      //  private IChartOfAccountBusiness _chartOfAccountBusiness;
      //  private IPaymentTermBusiness _paymentTermBusiness;
      //  private IPurchaseOrderBusiness _purchaseOrderBusiness;

        Common _common = new Common();
        AppConst _appConst = new AppConst();

        public SupplierPaymentController(ISupplierPaymentBusiness supplierPaymentBusiness,
            ISupplierBusiness supplierBusiness)
            //, IChartOfAccountBusiness chartOfAccountBusiness, 
            //IPaymentTermBusiness paymentTermBusiness, IPurchaseOrderBusiness purchaseOrderBusiness)
        {
            _supplierPaymentBusiness = supplierPaymentBusiness;
            _supplierBusiness = supplierBusiness;
            //_chartOfAccountBusiness = chartOfAccountBusiness;
            //_paymentTermBusiness = paymentTermBusiness;
            //_purchaseOrderBusiness = purchaseOrderBusiness;
        }
        // GET: SupplierPayment
        [AuthSecurityFilter(ProjectObject = "SupplierPayment", Mode = "R")]
        public ActionResult ViewSupplierPayment(string code)
        {
            ViewBag.SysModuleCode = code;
            SupplierPaymentAdvanceSearchViewModel supplierPaymentAdvanceSearchVM = new SupplierPaymentAdvanceSearchViewModel();
            supplierPaymentAdvanceSearchVM.Supplier = new SupplierViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            supplierPaymentAdvanceSearchVM.Supplier.SelectList = new List<SelectListItem>();
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
            supplierPaymentAdvanceSearchVM.Supplier.SelectList = selectListItem;
            return View(supplierPaymentAdvanceSearchVM);
        }
        [AuthSecurityFilter(ProjectObject = "SupplierPayment", Mode = "R")]
        public ActionResult NewSupplierPayment(string code, Guid? id)
        {
            ViewBag.SysModuleCode = code;
            SupplierPaymentViewModel supplierPaymentVM = new SupplierPaymentViewModel
            {
                ID = id == null ? Guid.Empty : (Guid)id,
                IsUpdate = id == null ? false : true,
            };
            supplierPaymentVM.Supplier = new SupplierViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            supplierPaymentVM.Supplier.SelectList = new List<SelectListItem>();
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
            supplierPaymentVM.Supplier.SelectList = selectListItem;
            return View(supplierPaymentVM);
        }



        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SupplierPayment", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewSupplierPayment", "SupplierPayment", new { Code = "ACC" });
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
                    toolboxVM.addbtn.Href = Url.Action("NewSupplierPayment", "SupplierPayment", new { Code = "ACC" });

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
                    toolboxVM.ListBtn.Href = Url.Action("ViewSupplierPayment", "SupplierPayment", new { Code = "ACC" });

                    break;

                case "Add":

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "Save();";

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Href = Url.Action("ViewSupplierPayment", "SupplierPayment", new { Code = "ACC" });
                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion

    }
}