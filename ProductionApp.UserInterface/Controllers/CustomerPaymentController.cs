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
    public class CustomerPaymentController : Controller
    {
        private ICustomerPaymentBusiness _customerPaymentBusiness;
        AppConst _appConst = new AppConst();
        Common _common = new Common();
        public CustomerPaymentController(ICustomerPaymentBusiness customerPaymentBusiness)
        {
            _customerPaymentBusiness = customerPaymentBusiness;
        }
        [AuthSecurityFilter(ProjectObject = "CustomerPayment", Mode = "R")]
        public ActionResult NewCustomerPayment(string code, Guid? id)
        {
            ViewBag.SysModuleCode = code;
            CustomerPaymentViewModel customerPaymentVM = new CustomerPaymentViewModel();
            customerPaymentVM.ID = id == null ? Guid.Empty : (Guid)id;
            customerPaymentVM.IsUpdate = id == null ? false : true;
            customerPaymentVM.CustomerPaymentDetail = new CustomerPaymentDetailViewModel();
            return View(customerPaymentVM);
        }
        [AuthSecurityFilter(ProjectObject = "CustomerPayment", Mode = "R")]
        public ActionResult ViewCustomerPayment(string code)
        {
            ViewBag.SysModuleCode = code;
            return View();
        }

        #region GetAllCustomerPayment
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "CustomerPayment", Mode = "R")]
        public JsonResult GetAllCustomerPayment(DataTableAjaxPostModel model, CustomerPaymentAdvanceSearchViewModel customerPaymentAdvanceSearchVM)
        {
            customerPaymentAdvanceSearchVM.DataTablePaging.Start = model.start;
            customerPaymentAdvanceSearchVM.DataTablePaging.Length = (customerPaymentAdvanceSearchVM.DataTablePaging.Length == 0 ? model.length : customerPaymentAdvanceSearchVM.DataTablePaging.Length);
            List<CustomerPaymentViewModel> customerPaymentList = Mapper.Map<List<CustomerPayment>, List<CustomerPaymentViewModel>>(_customerPaymentBusiness.GetAllCustomerPayment(Mapper.Map<CustomerPaymentAdvanceSearchViewModel, CustomerPaymentAdvanceSearch>(customerPaymentAdvanceSearchVM)));
            if (customerPaymentAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = customerPaymentList.Count != 0 ? customerPaymentList[0].TotalCount : 0;
                int filteredResult = customerPaymentList.Count != 0 ? customerPaymentList[0].FilteredCount : 0;
                customerPaymentList = customerPaymentList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }

            return Json(new
            {
                draw = model.draw,
                recordsTotal = customerPaymentList.Count != 0 ? customerPaymentList[0].TotalCount : 0,
                recordsFiltered = customerPaymentList.Count != 0 ? customerPaymentList[0].FilteredCount : 0,
                data = customerPaymentList
            });
        }
        #endregion

        #region InsertUpdateCustomerPayment
       // [AuthSecurityFilter(ProjectObject = "CustomerPayment", Mode = "R")]
        [HttpPost]
        public string InsertUpdateCustomerPayment(CustomerPaymentViewModel customerPaymentVM)
        {

            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                customerPaymentVM.Common = new CommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _common.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _common.GetCurrentDateTime(),
                };
                //Deserialize items
                object ResultFromJS = JsonConvert.DeserializeObject(customerPaymentVM.DetailJSON);
                string ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                customerPaymentVM.CustomerPaymentDetailList = JsonConvert.DeserializeObject<List<CustomerPaymentDetailViewModel>>(ReadableFormat);
                var result = _customerPaymentBusiness.InsertUpdateCustomerPayment(Mapper.Map<CustomerPaymentViewModel, CustomerPayment>(customerPaymentVM));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = cm.Message });
            }

        }
        #endregion InsertUpdateCustomerPayment

        #region ValidateCustomerPayment
        public string ValidateCustomerPayment(string id,string paymentRefNo)
        {
            object result = null;
            try
            {
                result = _customerPaymentBusiness.ValidateCustomerPayment(Guid.Parse(id),paymentRefNo);
                return JsonConvert.SerializeObject(new { Result = "OK", Record = result, Message = "" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion ValidateCustomerPayment

        #region GetOutStandingInvoices
        [AuthSecurityFilter(ProjectObject = "CustomerPayment", Mode = "R")]
        public string GetOutStandingInvoices(string paymentId,string CustomerId)
        {
            try
            {
                List<CustomerInvoiceViewModel> customerInvoiceVM = new List<CustomerInvoiceViewModel>();
                customerInvoiceVM = Mapper.Map<List<CustomerInvoice>, List<CustomerInvoiceViewModel>>(_customerPaymentBusiness.GetOutStandingInvoices(Guid.Parse(paymentId), Guid.Parse(CustomerId)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = customerInvoiceVM, Message = "Success" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = ex });
            }
        }
        #endregion

        #region GetOutStandingAmount
        [AuthSecurityFilter(ProjectObject = "CustomerPayment", Mode = "R")]
        public string GetOutStandingAmount(string Id)
        {
            try
            {
                CustomerInvoiceViewModel customerInvoiceVM = new CustomerInvoiceViewModel();
                customerInvoiceVM= Mapper.Map<CustomerInvoice, CustomerInvoiceViewModel>(_customerPaymentBusiness.GetOutstandingAmount(Guid.Parse(Id)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = customerInvoiceVM, Message = "Success" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = ex });
            }
        }
        #endregion

        #region GetCustomerPayment
        [AuthSecurityFilter(ProjectObject = "CustomerPayment", Mode = "R")]
        public string GetCustomerPayment(string Id)
        {
            try
            { 
            CustomerPaymentViewModel customerPaymentVM = new CustomerPaymentViewModel();
            customerPaymentVM = Mapper.Map<CustomerPayment, CustomerPaymentViewModel>(_customerPaymentBusiness.GetCustomerPayment(Id));
            return JsonConvert.SerializeObject(new { Result = "OK", Record = customerPaymentVM, Message = "Success" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Record = "", Message = ex });
            }
        }
        #endregion GetCustomerPayment

        #region DeleteCustomerPayment
        [AuthSecurityFilter(ProjectObject = "CustomerPayment", Mode = "R")]
        public string DeleteCustomerPayment(string id)
        {
            object result = null;
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    throw new Exception("ID Missing");
                }
                result = _customerPaymentBusiness.DeleteCustomerPayment(Guid.Parse(id));
                return JsonConvert.SerializeObject(new { Result = "OK", Record = result, Message = _appConst.DeleteSuccess });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion DeleteCustomerPayment

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "CustomerPayment", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewCustomerPayment", "CustomerPayment", new { Code = "ACC" });
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResettblCustomerPayment();";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "ImportCustomerPaymentData();";
                    //---------------------------------------

                    break;
                case "Edit":

                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "New";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewCustomerPayment", "CustomerPayment", new { Code = "ACC" });

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
                    toolboxVM.ListBtn.Href = Url.Action("ViewCustomerPayment", "CustomerPayment", new { Code = "ACC" });

                    break;

                case "Add":

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "Save();";

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Href = Url.Action("ViewCustomerPayment", "CustomerPayment", new { Code = "ACC" });
                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion
    }
}