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
    public class CustomerCreditNoteController : Controller
    {


        #region Constructor_Injection 

        AppConst c = new AppConst();
        ICustomerBusiness _customerBusiness;
        ICustomerCreditNoteBusiness _customerCreditNotesBusiness; 

        public CustomerCreditNoteController(ICustomerCreditNoteBusiness customerCreditNoteBusiness, ICustomerBusiness customerBusiness)
        {
            _customerCreditNotesBusiness = customerCreditNoteBusiness;
            _customerBusiness = customerBusiness;
        }
        #endregion Constructor_Injection 




        // GET: CustomerCreditNote
        [HttpGet]
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
                customerCreditNoteVM = new CustomerCreditNoteViewModel();
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