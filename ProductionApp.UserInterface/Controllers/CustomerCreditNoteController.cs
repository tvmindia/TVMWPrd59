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
    public class CustomerCreditNoteController : Controller
    {


        #region Constructor_Injection 

        AppConst c = new AppConst();
        ICustomerBusiness _customerBusiness;
        ICustomerCreditNoteBusiness _customerCreditNoteBusiness; 

        public CustomerCreditNoteController(ICustomerCreditNoteBusiness customerCreditNoteBusiness, ICustomerBusiness customerBusiness)
        {
            _customerCreditNoteBusiness = customerCreditNoteBusiness;
            _customerBusiness = customerBusiness;
        }
        #endregion Constructor_Injection 




        // GET: CustomerCreditNote
        [AuthSecurityFilter(ProjectObject = "CustomerCreditNote", Mode = "R")]
        public ActionResult NewCustomerCreditNote(string code, Guid? id)
        {
            ViewBag.SysModuleCode = code;
            CustomerCreditNoteViewModel customerCreditNoteVM = new CustomerCreditNoteViewModel()
            {
                ID = id == null ? Guid.Empty : (Guid)id,
                IsUpdate = id == null ? false : true
            };

            try
            {
                List<SelectListItem> selectListItem = new List<SelectListItem>();
                List<CustomerViewModel> customerList = Mapper.Map<List<Customer>, List<CustomerViewModel>>(_customerBusiness.GetCustomerForSelectList());
                foreach (CustomerViewModel customer in customerList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = customer.CompanyName,
                        Value = customer.ID.ToString(),
                        Selected = false
                    });
                }
                customerCreditNoteVM.CustomerList = selectListItem; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(customerCreditNoteVM);
        }


        [AuthSecurityFilter(ProjectObject = "CustomerCreditNote", Mode = "R")]
        public ActionResult ViewCustomerCreditNote(string code)
        {
            ViewBag.SysModuleCode = code;
            CustomerCreditNoteAdvanceSearchViewModel customerCreditNoteVM = new CustomerCreditNoteAdvanceSearchViewModel();         

            try
            {
                customerCreditNoteVM = new CustomerCreditNoteAdvanceSearchViewModel();
                List<SelectListItem> selectListItem = new List<SelectListItem>();
                List<CustomerViewModel> customerList = Mapper.Map<List<Customer>, List<CustomerViewModel>>(_customerBusiness.GetCustomerForSelectList());
                foreach (CustomerViewModel customer in customerList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = customer.CompanyName,
                        Value = customer.ID.ToString(),
                        Selected = false
                    });
                }
                customerCreditNoteVM.Customer = new CustomerViewModel();
                customerCreditNoteVM.Customer.SelectList = selectListItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(customerCreditNoteVM);
        }

        #region GetAllCustomerCreditNote
       
        [AuthSecurityFilter(ProjectObject = "CustomerCreditNote", Mode = "R")]
        public JsonResult GetAllCustomerCreditNote(DataTableAjaxPostModel model, CustomerCreditNoteAdvanceSearchViewModel customerCreditNoteAdvanceSearchVM)
        {

            customerCreditNoteAdvanceSearchVM.DataTablePaging.Start = model.start;
            customerCreditNoteAdvanceSearchVM.DataTablePaging.Length = (customerCreditNoteAdvanceSearchVM.DataTablePaging.Length == 0 ? model.length : customerCreditNoteAdvanceSearchVM.DataTablePaging.Length);
            List<CustomerCreditNoteViewModel> CustomerCreditNotesList = Mapper.Map<List<CustomerCreditNote>, List<CustomerCreditNoteViewModel>>(_customerCreditNoteBusiness.GetAllCustomerCreditNote(Mapper.Map<CustomerCreditNoteAdvanceSearchViewModel, CustomerCreditNoteAdvanceSearch>(customerCreditNoteAdvanceSearchVM)));
            return Json(new
            {
                draw = model.draw,
                recordsTotal = CustomerCreditNotesList.Count != 0 ? CustomerCreditNotesList[0].TotalCount : 0,
                recordsFiltered = CustomerCreditNotesList.Count != 0 ? CustomerCreditNotesList[0].FilteredCount : 0,
                data = CustomerCreditNotesList
            });

        }
        #endregion  GetAllCustomerCreditNote

        #region GetCustomerCreditNote 
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "CustomerCreditNote", Mode = "R")]
        public string GetCustomerCreditNote (string ID)
        {
            try
            {
                CustomerCreditNoteViewModel customerCreditNoteObj = Mapper.Map<CustomerCreditNote, CustomerCreditNoteViewModel>(_customerCreditNoteBusiness.GetCustomerCreditNote(ID != null && ID != "" ? Guid.Parse(ID) : Guid.Empty));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = customerCreditNoteObj });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion  GetCustomerCreditNote 

        #region InsertUpdateCustomerCreditNote
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "CustomerCreditNote", Mode = "W")]
        public string InsertUpdateCustomerCreditNote(CustomerCreditNoteViewModel customersCreditNoteVM)
        {
            try
            {

                object result = null;
                AppUA _appUA = Session["AppUA"] as AppUA;
                Common _common = new Common();

                customersCreditNoteVM.Common = new CommonViewModel();
                customersCreditNoteVM.Common.CreatedBy = _appUA.UserName;
                customersCreditNoteVM.Common.CreatedDate = _common.GetCurrentDateTime();
                customersCreditNoteVM.Common.UpdatedBy = _appUA.UserName;
                customersCreditNoteVM.Common.UpdatedDate = _common.GetCurrentDateTime();

                result = _customerCreditNoteBusiness.InsertUpdateCustomerCreditNote(Mapper.Map<CustomerCreditNoteViewModel, CustomerCreditNote>(customersCreditNoteVM));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result });

            }
            catch (Exception ex)
            {

                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion InsertUpdateCustomerCreditNote

        #region DeleteCustomerCreditNote
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "CustomerCreditNote", Mode = "D")]
        public string DeleteCustomerCreditNote(string ID)
        {

            try
            {
                object result = null;
                AppUA _appUA = Session["AppUA"] as AppUA;
                result = _customerCreditNoteBusiness.DeleteCustomerCreditNote(ID != null && ID != "" ? Guid.Parse(ID) : Guid.Empty, _appUA.UserName);
                return JsonConvert.SerializeObject(new { Result = "OK", Message = result });

            }
            catch (Exception ex)
            {
                AppConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion DeleteCustomerCreditNote

        #region CheckValue
        [AcceptVerbs("Get", "Post")]
        public ActionResult CheckValue(decimal CreditAmount)
        {
            //ProductionOrderDetailViewModel prodOrderDetailVM = new ProductionOrderDetailViewModel();
            if ((CreditAmount == 0))
            {

                return Json("<p><span style='vertical-align: 2px'>Credit Amount could not be greater than zero!</span></p>", JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        #endregion CheckValue 

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "CustomerCreditNote", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewCustomerCreditNote", "CustomerCreditNote", new { code = "ACC" });
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "BindOrReloadCustomerCreditNoteTable('Reset');";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "BindOrReloadCustomerCreditNoteTable('Export');";
                    //---------------------------------------

                    break;
                case "Edit":

                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "New";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewCustomerCreditNote", "CustomerCreditNote", new { code = "ACC" });

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
                    toolboxVM.ListBtn.Href = Url.Action("ViewCustomerCreditNote", "CustomerCreditNote", new { code = "ACC" });

                    break;

                case "Add":

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "Save();";

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Href = Url.Action("ViewCustomerCreditNote", "CustomerCreditNote", new { code = "ACC" });
                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion
    }
}