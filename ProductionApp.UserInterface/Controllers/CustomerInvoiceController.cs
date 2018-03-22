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
    public class CustomerInvoiceController : Controller
    {
        private ICustomerInvoiceBusiness _customerInvoiceBusiness;
        private ICustomerBusiness _customerBusiness;
        private IPackingSlipBusiness _packingSlipBusiness;
        public CustomerInvoiceController(ICustomerInvoiceBusiness customerInvoiceBusiness, ICustomerBusiness customerBusiness, IPackingSlipBusiness packingSlipBusiness)
        {
            _customerInvoiceBusiness = customerInvoiceBusiness;
            _customerBusiness = customerBusiness;
            _packingSlipBusiness = packingSlipBusiness;
        }
        // GET: CustomerInvoice
        public ActionResult ViewCustomerInvoice(string code)
        {
            ViewBag.SysModuleCode = code;
            return View();
        }
        public ActionResult NewCustomerInvoice(string code, Guid? id)
        {
            ViewBag.SysModuleCode = code;
            CustomerInvoiceViewModel customerInvoiceVM = new CustomerInvoiceViewModel
            {
                ID = id == null ? Guid.Empty : (Guid)id,
                IsUpdate = id == null ? false : true,

            };
            return View(customerInvoiceVM);
        }


        #region GetCustomerDetails
        //[AuthSecurityFilter(ProjectObject = "", Mode = "D")]
        public string GetCustomerDetails(string customerId)
        {
            try
            {
                CustomerViewModel customerVM = Mapper.Map<Customer, CustomerViewModel>(_customerBusiness.GetCustomer(Guid.Parse(customerId)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = customerVM, Message = "Success" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = ex });
            }
        }
        #endregion GetCustomerDetails

        #region GetPackingSlipDetail
        public string GetPackingSlipDetail(string packingSlipID)
        {
            try
            {
                List<CustomerInvoiceDetailViewModel> customerInvoiceDetailVM = new List<CustomerInvoiceDetailViewModel>();
                customerInvoiceDetailVM = Mapper.Map<List<CustomerInvoiceDetail>, List<CustomerInvoiceDetailViewModel>>(_customerInvoiceBusiness.GetPackingSlipDetailForCustomerInvoice(Guid.Parse(packingSlipID)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = customerInvoiceDetailVM, Message = "Success" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = ex });
            }
        }

        #endregion GetPackingSlipDetail


        #region GetPackingSlip
        public string GetPackingSlip(string packingSlipID)
        {
            try
            {
                PackingSlipViewModel packingSlipVM = Mapper.Map<PackingSlip, PackingSlipViewModel>(_packingSlipBusiness.GetPackingSlip(Guid.Parse(packingSlipID)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = packingSlipVM, Message = "Success" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = ex });
            }
        }

        #endregion GetPackingSlip

        #region ButtonStyling
        [HttpGet]
        //[AuthSecurityFilter(ProjectObject = "CustomerInvoice", Mode = "")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewCustomerInvoice", "CustomerInvoice", new { Code = "ACC" });
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetCustomerInvoiceList();";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "ImportCustomerInvoiceData();";
                    //---------------------------------------

                    break;
                case "Edit":

                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "New";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewCustomerInvoice", "CustomerInvoice", new { Code = "ACC" });

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
                    toolboxVM.ListBtn.Href = Url.Action("ViewCustomerInvoice", "CustomerInvoice", new { Code = "ACC" });

                    break; 
              
                case "Add":

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "Save();";

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Href = Url.Action("ViewCustomerInvoice", "CustomerInvoice", new { Code = "ACC" });
                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion

    }
}