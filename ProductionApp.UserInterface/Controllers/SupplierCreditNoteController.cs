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
    public class SupplierCreditNoteController : Controller
    {
        #region Constructor_Injection 

        AppConst c = new AppConst();
        ISupplierBusiness _supplierBusiness;
        ISupplierCreditNoteBusiness _supplierCreditNotesBusiness;

        public SupplierCreditNoteController(ISupplierCreditNoteBusiness SupplierCreditNoteBusiness, ISupplierBusiness SupplierBusiness)
        {
            _supplierCreditNotesBusiness = SupplierCreditNoteBusiness;
            _supplierBusiness = SupplierBusiness;
        }
        #endregion Constructor_Injection 

        // GET: SupplierCreditNote
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SupplierCreditNote", Mode = "R")]
        public ActionResult NewSupplierCreditNote(string code, Guid? id)
        {
          
            ViewBag.SysModuleCode = code;
            SupplierCreditNoteViewModel supplierCreditNoteVM = new SupplierCreditNoteViewModel()
            {
                ID = id == null ? Guid.Empty : (Guid)id,
                IsUpdate = id == null ? false : true
            };
            try
            {

                supplierCreditNoteVM = new SupplierCreditNoteViewModel();
                List<SelectListItem> selectListItem = new List<SelectListItem>();
                List<SupplierViewModel> SupplierList = Mapper.Map<List<Supplier>, List<SupplierViewModel>>(_supplierBusiness.GetSupplierForSelectList());
                foreach (SupplierViewModel Supplier in SupplierList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = Supplier.CompanyName,
                        Value = Supplier.ID.ToString(),
                        Selected = false
                    });
                }
                supplierCreditNoteVM.SupplierList = selectListItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(supplierCreditNoteVM);
        }


        [AuthSecurityFilter(ProjectObject = "SupplierCreditNote", Mode = "R")]
        public ActionResult ViewSupplierCreditNote(string code)
        {
            ViewBag.SysModuleCode = code;
            SupplierCreditNoteAdvanceSearchViewModel supplierCreditNoteVM = new SupplierCreditNoteAdvanceSearchViewModel();

            try
            {
                supplierCreditNoteVM = new SupplierCreditNoteAdvanceSearchViewModel();
                List<SelectListItem> selectListItem = new List<SelectListItem>();
                List<SupplierViewModel> SupplierList = Mapper.Map<List<Supplier>, List<SupplierViewModel>>(_supplierBusiness.GetSupplierForSelectList());
                foreach (SupplierViewModel Supplier in SupplierList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = Supplier.CompanyName,
                        Value = Supplier.ID.ToString(),
                        Selected = false
                    });
                }
                supplierCreditNoteVM.Supplier = new SupplierViewModel();
                supplierCreditNoteVM.Supplier.SelectList = selectListItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(supplierCreditNoteVM);
        }


        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SupplierCreditNote", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewSupplierCreditNote", "SupplierCreditNote", new { code = "ACC" });
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "BindOrReloadSupplierCreditNoteTable('Reset');";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "BindOrReloadSupplierCreditNoteTable('Export');";
                    //---------------------------------------

                    break;
                case "Edit":

                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "New";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewSupplierCreditNote", "SupplierCreditNote", new { code = "ACC" });

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
                    toolboxVM.ListBtn.Href = Url.Action("ViewSupplierCreditNote", "SupplierCreditNote", new { code = "ACC" });

                    break;

                case "Add":

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "Save();";

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Href = Url.Action("ViewSupplierCreditNote", "SupplierCreditNote", new { code = "ACC" });
                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion
    }
}