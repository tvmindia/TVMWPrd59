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

        #region ViewCustomer
        [AuthSecurityFilter(ProjectObject = "Customer", Mode = "R")]
        public ActionResult ViewCustomer(string code)
        {
            ViewBag.SysModuleCode = code;

            return View();
        }
        #endregion ViewCustomer

        #region NewCustomer
        [AuthSecurityFilter(ProjectObject = "Customer", Mode = "R")]
        public ActionResult NewCustomer(string code, Guid? id)
        {
            ViewBag.SysModuleCode = code;
            CustomerViewModel customerVM = new CustomerViewModel()
            {
                ID = id == null ? Guid.Empty : (Guid)id,
                IsUpdate = id == null ? false : true
            };
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            //Technician Drop down bind
            List<ContactTitleViewModel> contactTitleList = Mapper.Map<List<ContactTitle>, List<ContactTitleViewModel>>(_customerBusiness.GetContactTitleForSelectList());
            contactTitleList = contactTitleList == null ? null : contactTitleList.OrderBy(attset => attset.Title).ToList();
            foreach (ContactTitleViewModel tvm in contactTitleList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = tvm.Title,
                    Value = tvm.Title,
                    Selected = false
                });
            }
            customerVM.ContactTitleList = selectListItem;
            return View(customerVM);
        }
        #endregion NewCustomer

        #region CustomerDropdown
        public ActionResult CustomerDropdown(string required)
        {
            CustomerViewModel customerVM = new CustomerViewModel();
            customerVM.SelectList = new List<SelectListItem>();
            List<CustomerViewModel> customerList = Mapper.Map<List<Customer>, List<CustomerViewModel>>(_customerBusiness.GetCustomerForSelectList());
            ViewBag.IsRequired = required;
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
        #endregion CustomerDropdown

        #region GetCustomerDetails
        //[AuthSecurityFilter(ProjectObject = "", Mode = "R")]
        public string GetCustomerDetails(string id)
        {
            try
            {
                CustomerViewModel customerVM = Mapper.Map<Customer, CustomerViewModel>(_customerBusiness.GetCustomer(Guid.Parse(id)));
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
                    recordsTotal = customerVMList != null ? (customerVMList.Count != 0 ? customerVMList[0].TotalCount : 0) : 0,
                    recordsFiltered = customerVMList != null ? (customerVMList.Count != 0 ? customerVMList[0].FilteredCount : 0) : 0,
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

        #region InsertUpdateCustomer
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "Customer", Mode = "R")]
        public string InsertUpdateCustomer(CustomerViewModel customerVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AppUA appUA = Session["AppUA"] as AppUA;
                    customerVM.Common = new CommonViewModel
                    {
                        CreatedBy = appUA.UserName,
                        CreatedDate = _common.GetCurrentDateTime(),
                        UpdatedBy = appUA.UserName,
                        UpdatedDate = _common.GetCurrentDateTime(),
                    };
                    var result = _customerBusiness.InsertUpdateCustomer(Mapper.Map<CustomerViewModel, Customer>(customerVM));
                    return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });


                }
                catch (Exception ex)
                {
                    AppConstMessage cm = _appConst.GetMessage(ex.Message);
                    return JsonConvert.SerializeObject(new { Status = "ERROR", Record="", Message = cm.Message });
                }
            }
            else
            {
                List<string> modelErrors = new List<string>();
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var modelError in modelState.Errors)
                    {
                        modelErrors.Add(modelError.ErrorMessage);
                    }
                }
                return JsonConvert.SerializeObject(new { Result = "VALIDATION", Message = string.Join(",", modelErrors) });
            }
        }
        #endregion InsertUpdateCustomer

        #region MasterPartial
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Customer", Mode = "R")]
        public ActionResult MasterPartial(string masterCode)
        {
            CustomerViewModel customerVM = string.IsNullOrEmpty(masterCode) ? new CustomerViewModel() : Mapper.Map<Customer, CustomerViewModel>(_customerBusiness.GetCustomer(Guid.Parse(masterCode)));
            customerVM.IsUpdate = string.IsNullOrEmpty(masterCode) ? false : true;
            return PartialView("_AddCustomerPartial", customerVM);
        }
        #endregion MasterPartial

        #region DeleteCustomer
        [AuthSecurityFilter(ProjectObject = "Customer", Mode = "D")]
        public string DeleteCustomer(string id)
        {
            object result = null;
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    throw new Exception("Deletion Not Successfull");
                }
                result = _customerBusiness.DeleteCustomer(Guid.Parse(id));
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = _appConst.DeleteSuccess });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record="", Message = cm.Message });
            }

        }
        #endregion DeleteCustomer

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
                    toolboxVM.addbtn.Href = Url.Action("NewCustomer", "Customer", new { Code = "MSTR" });
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
                case "Add":

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "Save();";

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Href = Url.Action("ViewCustomer", "Customer", new { Code = "MSTR" });
                    break;

                case "Edit":

                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "New";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewCustomer", "Customer", new { Code = "MSTR" });

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
                    toolboxVM.ListBtn.Href = Url.Action("ViewCustomer", "Customer", new { Code = "MSTR" });

                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }
        #endregion ButtonStyling

    }
}