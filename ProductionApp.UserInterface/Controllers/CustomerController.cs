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
    public class CustomerController : Controller
    {
        // GET: Customer
        AppConst _appConst = new AppConst();
        private Common _common = new Common();
        private ICustomerBusiness _customerBusiness;

        #region Constructor Injection
        public CustomerController(ICustomerBusiness customerBusiness)
        {
            _customerBusiness = customerBusiness;
        }
        #endregion Constructor Injection

        #region Index
        [AuthSecurityFilter(ProjectObject = "Customer", Mode = "R")]
        public ActionResult Index(string code)
        {
            ViewBag.SysModuleCode = code;
            CustomerAdvanceSearchViewModel customerAdvanceSearchVM = new CustomerAdvanceSearchViewModel();
            return View(customerAdvanceSearchVM);
        }
        #endregion Index

        public ActionResult CustomerDropdown()
        {
            CustomerViewModel customerVM = new CustomerViewModel();
            customerVM.SelectList = new List<SelectListItem>();
            List<CustomerViewModel> customerList = Mapper.Map<List<Customer>, List<CustomerViewModel>>(_customerBusiness.GetCustomerForSelectList());
            if (customerList != null)
                foreach (CustomerViewModel customer in customerList)
                {
                    customerVM.SelectList.Add(new SelectListItem
                    {
                        Text = customer.CompanyName,
                        Value = customer.ID.ToString(),
                        Selected = false
                    });
                }
            return PartialView("_CustomerDropdown", customerVM);

        }

        #region GetCustomerDetails
        //[AuthSecurityFilter(ProjectObject = "", Mode = "R")]
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

        #region GetAllCustomer
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "Customer", Mode = "R")]
        public JsonResult GetAllCustomer(DataTableAjaxPostModel model, CustomerAdvanceSearchViewModel customerAdvanceSearchVM)
        {
            try
            {
                //setting options to our model
                customerAdvanceSearchVM.DataTablePaging.Start = model.start;
                customerAdvanceSearchVM.DataTablePaging.Length = (customerAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : customerAdvanceSearchVM.DataTablePaging.Length;

                //bankAdvanceSearchVM.OrderColumn = model.order[0].column;
                //bankAdvanceSearchVM.OrderDir = model.order[0].dir;

                // action inside a standard controller
                List<CustomerViewModel> customerVMList = Mapper.Map<List<Customer>, List<CustomerViewModel>>(_customerBusiness.GetAllCustomer(Mapper.Map<CustomerAdvanceSearchViewModel, CustomerAdvanceSearch>(customerAdvanceSearchVM)));
                if (customerAdvanceSearchVM.DataTablePaging.Length == -1)
                {
                    int totalResult = customerVMList.Count != 0 ? customerVMList[0].TotalCount : 0;
                    int filteredResult = customerVMList.Count != 0 ? customerVMList[0].FilteredCount : 0;
                    customerVMList = customerVMList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
                }
                var settings = new JsonSerializerSettings
                {
                    //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Formatting = Formatting.None
                };
                return Json(new
                {
                    // this is what datatables wants sending back
                    draw = model.draw,
                    recordsTotal = customerVMList.Count != 0 ? customerVMList[0].TotalCount : 0,
                    recordsFiltered = customerVMList.Count != 0 ? customerVMList[0].FilteredCount : 0,
                    data = customerVMList
                });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return Json(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion GetAllCustomer

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Customer", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddCustomerMaster('MSTR')";
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetCustomerList();";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "ImportCustomerData();";
                    //---------------------------------------
                    break;

                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion
    }
}