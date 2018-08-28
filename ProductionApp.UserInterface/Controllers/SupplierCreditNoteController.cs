using AutoMapper;
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
    public class SupplierCreditNoteController : Controller
    {
        #region Constructor_Injection 

        AppConst c = new AppConst();
        ISupplierBusiness _supplierBusiness;
        ISupplierCreditNoteBusiness _supplierCreditNoteBusiness;

        public SupplierCreditNoteController(ISupplierCreditNoteBusiness SupplierCreditNoteBusiness, ISupplierBusiness SupplierBusiness)
        {
            _supplierCreditNoteBusiness = SupplierCreditNoteBusiness;
            _supplierBusiness = SupplierBusiness;
        }
        #endregion Constructor_Injection 

        // GET: SupplierCreditNote
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


        #region GetAllSupplierCreditNote

        [AuthSecurityFilter(ProjectObject = "SupplierCreditNote", Mode = "R")]
        public JsonResult GetAllSupplierCreditNote(DataTableAjaxPostModel model, SupplierCreditNoteAdvanceSearchViewModel supplierCreditNoteAdvanceSearchVM)
        {

            List<SupplierCreditNoteViewModel> SupplierCreditNotesList = Mapper.Map<List<SupplierCreditNote>, List<SupplierCreditNoteViewModel>>(_supplierCreditNoteBusiness.GetAllSupplierCreditNote());
            return Json(new
            {
                draw = model.draw,
                recordsTotal = SupplierCreditNotesList.Count != 0 ? SupplierCreditNotesList[0].TotalCount : 0,
                recordsFiltered = SupplierCreditNotesList.Count != 0 ? SupplierCreditNotesList[0].FilteredCount : 0,
                data = SupplierCreditNotesList
            });

        }
        #endregion  GetAllSupplierCreditNote

        #region GetSupplierCreditNote 
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SupplierCreditNote", Mode = "R")]
        public string GetSupplierCreditNote(string ID)
        {
            try
            {
                SupplierCreditNoteViewModel supplierCreditNoteObj = Mapper.Map<SupplierCreditNote, SupplierCreditNoteViewModel>(_supplierCreditNoteBusiness.GetSupplierCreditNote(ID != null && ID != "" ? Guid.Parse(ID) : Guid.Empty));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = supplierCreditNoteObj });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetSupplierCreditNote 

        #region InsertUpdateSupplierCreditNote
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "SupplierCreditNote", Mode = "W")]
        public string InsertUpdateSupplierCreditNote(SupplierCreditNoteViewModel supplierCreditNoteVM)
        {
            try
            {

                object result = null;
                AppUA _appUA = Session["AppUA"] as AppUA;
                Common _common = new Common();

                supplierCreditNoteVM.Common = new CommonViewModel();
                supplierCreditNoteVM.Common.CreatedBy = _appUA.UserName;
                supplierCreditNoteVM.Common.CreatedDate = _common.GetCurrentDateTime();
                supplierCreditNoteVM.Common.UpdatedBy = _appUA.UserName;
                supplierCreditNoteVM.Common.UpdatedDate = _common.GetCurrentDateTime();

                result = _supplierCreditNoteBusiness.InsertUpdateSupplierCreditNote(Mapper.Map<SupplierCreditNoteViewModel, SupplierCreditNote>(supplierCreditNoteVM));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result });

            }
            catch (Exception ex)
            {

                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion InsertUpdateSupplierCreditNote

        #region DeleteSupplierCreditNote
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SupplierCreditNote", Mode = "D")]
        public string DeleteSupplierCreditNote(string ID)
        {

            try
            {
                object result = null;
                AppUA _appUA = Session["AppUA"] as AppUA;
                result = _supplierCreditNoteBusiness.DeleteSupplierCreditNote(ID != null && ID != "" ? Guid.Parse(ID) : Guid.Empty, _appUA.UserName);
                return JsonConvert.SerializeObject(new { Result = "OK", Message = result });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion DeleteSupplierCreditNote




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