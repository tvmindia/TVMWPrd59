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
    public class SalesOrderController : Controller
    {
        // GET: SalesOrder
        private ISalesOrderBusiness _salesOrderBusiness;
        private ICustomerBusiness _customerBusiness;
        private IEmployeeBusiness _employeeBusiness;
        private IProductBusiness _productBusiness;
        private IProductCategoryBusiness _productCategoryBusiness;
        private ITaxTypeBusiness _taxTypeBusiness;
        Common _common = new Common();
        AppConst _appConst = new AppConst();
        public SalesOrderController(ISalesOrderBusiness salesOrderBusiness,
            ICustomerBusiness customerBusiness,IEmployeeBusiness employeeBusiness,
            IProductBusiness productBusiness, IProductCategoryBusiness productCategoryBusiness,
            ITaxTypeBusiness taxTypeBusiness)

        {
            _salesOrderBusiness = salesOrderBusiness;
            _customerBusiness = customerBusiness;
            _employeeBusiness = employeeBusiness;
            _productBusiness = productBusiness;
            _productCategoryBusiness = productCategoryBusiness;
            _taxTypeBusiness = taxTypeBusiness;
        }
        [AuthSecurityFilter(ProjectObject = "SalesOrder", Mode = "R")]
        public ActionResult AddSalesOrder(string code, Guid? id)
        {
            ViewBag.SysModuleCode = code;
            SalesOrderViewModel salesOrderVM = new SalesOrderViewModel
            {
                ID = id == null ? Guid.Empty : (Guid)id,
                IsUpdate = id == null ? false : true,
        };
            salesOrderVM.ProductCategory = new ProductCategoryViewModel();
            salesOrderVM.ProductCategory.ProductCategorySelectList = _productCategoryBusiness.GetProductCategoryForSelectList();
            salesOrderVM.SalesOrderDetail = new SalesOrderDetailViewModel();
            salesOrderVM.SalesOrderDetail.Product = new ProductViewModel();
            salesOrderVM.SalesOrderDetail.Product.ProductSelectList=_productBusiness.GetProductForSelectList("PRO");
            salesOrderVM.Customer = new CustomerViewModel();

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
            salesOrderVM.Customer = customerVM;
            salesOrderVM.SalesOrderDetail.TaxType = new TaxTypeViewModel();
            salesOrderVM.SalesOrderDetail.TaxType.SelectList = new List<SelectListItem>();
            List<TaxTypeViewModel> taxTypeList = Mapper.Map<List<TaxType>, List<TaxTypeViewModel>>(_taxTypeBusiness.GetTaxTypeForSelectList());
            if (taxTypeList != null)
                foreach (TaxTypeViewModel taxType in taxTypeList)
                {
                    salesOrderVM.SalesOrderDetail.TaxType.SelectList.Add(new SelectListItem
                    {
                        Text = taxType.Description,
                        Value = taxType.Code,
                        Selected = false
                    });
                }

            return View(salesOrderVM);
        }

        [AuthSecurityFilter(ProjectObject = "SalesOrder", Mode = "R")]
        public ActionResult ListSalesOrder(string code)
        {
            ViewBag.SysModuleCode = code;

            List<SelectListItem> selectListItem = new List<SelectListItem>();
            SalesOrderAdvanceSearchViewModel salesOrderAdvanceSearchVM = new SalesOrderAdvanceSearchViewModel();

            salesOrderAdvanceSearchVM.Customer = new CustomerViewModel();
            salesOrderAdvanceSearchVM.Customer.SelectList = new List<SelectListItem>();
            List<CustomerViewModel> customerList = Mapper.Map<List<Customer>, List<CustomerViewModel>>(_customerBusiness.GetCustomerForSelectList());
            if (customerList != null)
                foreach (CustomerViewModel customer in customerList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = customer.CompanyName,
                        Value = customer.ID.ToString(),
                        Selected = false
                    });
                }
            salesOrderAdvanceSearchVM.Customer.SelectList = selectListItem;

            selectListItem = new List<SelectListItem>();
            salesOrderAdvanceSearchVM.Employee = new EmployeeViewModel();
            salesOrderAdvanceSearchVM.Employee.SelectList = new List<SelectListItem>();
            List<EmployeeViewModel> employeeList = Mapper.Map<List<Employee>, List<EmployeeViewModel>>(_employeeBusiness.GetEmployeeForSelectList());
            if (customerList != null)
                foreach (EmployeeViewModel employee in employeeList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = employee.Name,
                        Value = employee.ID.ToString(),
                        Selected = false
                    });
                }
            salesOrderAdvanceSearchVM.Employee.SelectList = selectListItem;


            return View(salesOrderAdvanceSearchVM);
        }

        /// <summary>
        /// Sales Order Summary view in Table
        /// </summary>
        /// <param name="model"></param>
        /// <param name="SalesOrderAdvanceSearchVM"></param>
        /// <returns></returns>
        #region GetAllSalesOrder
        [AuthSecurityFilter(ProjectObject = "SalesOrder", Mode = "R")]
        public JsonResult GetAllSalesOrder(DataTableAjaxPostModel model,SalesOrderAdvanceSearchViewModel SalesOrderAdvanceSearchVM)
        {
            SalesOrderAdvanceSearchVM.DataTablePaging.Start = model.start;
            SalesOrderAdvanceSearchVM.DataTablePaging.Length = (SalesOrderAdvanceSearchVM.DataTablePaging.Length == 0 ? model.length : SalesOrderAdvanceSearchVM.DataTablePaging.Length);
            List<SalesOrderViewModel> salesOrderList =Mapper.Map<List<SalesOrder>, List<SalesOrderViewModel>>(_salesOrderBusiness.GetAllSalesOrder(Mapper.Map<SalesOrderAdvanceSearchViewModel, SalesOrderAdvanceSearch>(SalesOrderAdvanceSearchVM)));
            return Json(new
            {
                draw = model.draw,
                recordsTotal =salesOrderList.Count != 0 ? salesOrderList[0].TotalCount : 0,
            recordsFiltered =salesOrderList.Count != 0 ? salesOrderList[0].FilteredCount : 0,
                data = salesOrderList
            });
        }
        #endregion GetAllSalesOrder

        /// <summary>
        /// Sales order Summary Detail View in Table
        /// </summary>
        /// <param name="model"></param>
        /// <param name="SalesOrderAdvanceSearchVM"></param>
        /// <returns></returns>
        #region GetAllSalesOrderDetail
        [AuthSecurityFilter(ProjectObject = "SalesOrder", Mode = "R")]
        public JsonResult GetAllSalesOrderDetail(DataTableAjaxPostModel model, SalesOrderAdvanceSearchViewModel SalesOrderAdvanceSearchVM)
        {
            SalesOrderAdvanceSearchVM.DataTablePaging.Start = model.start;
            SalesOrderAdvanceSearchVM.DataTablePaging.Length = (SalesOrderAdvanceSearchVM.DataTablePaging.Length == 0 ? model.length : SalesOrderAdvanceSearchVM.DataTablePaging.Length);
            List<SalesOrderViewModel> salesOrderDetailList = Mapper.Map<List<SalesOrder>, List<SalesOrderViewModel>>(_salesOrderBusiness.GetAllSalesOrderDetail(Mapper.Map<SalesOrderAdvanceSearchViewModel, SalesOrderAdvanceSearch>(SalesOrderAdvanceSearchVM)));
            return Json(new
            {
                draw = model.draw,
                recordsTotal =salesOrderDetailList.Count != 0 ? salesOrderDetailList[0].TotalCount : 0,
                recordsFiltered =salesOrderDetailList.Count != 0 ? salesOrderDetailList[0].FilteredCount : 0,
                data = salesOrderDetailList
            });
        }
        #endregion GetAllSalesOrderDetail

        #region InsertUpdateSalesOrder
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "SalesOrder", Mode = "W")]
        public string InsertUpdateSalesOrder(SalesOrderViewModel salesOrderVM)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                salesOrderVM.Common = new CommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _common.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _common.GetCurrentDateTime(),
                };
                //Fixing Employee ID To SalesPerson
                salesOrderVM.SalesPerson = salesOrderVM.EmployeeID;
                //Deserialize items
                object ResultFromJS;
                string ReadableFormat;
                if (salesOrderVM.DetailJSON != null)
                {
                    ResultFromJS = JsonConvert.DeserializeObject(salesOrderVM.DetailJSON);
                    ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                    salesOrderVM.SalesOrderDetailList = JsonConvert.DeserializeObject<List<SalesOrderDetailViewModel>>(ReadableFormat);
                }
                var result =_salesOrderBusiness.InsertUpdateSalesOrder(Mapper.Map<SalesOrderViewModel, SalesOrder>(salesOrderVM));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }

        }
        #endregion InsertUpdateSalesOrder

        #region GetSalesOrder
        [AuthSecurityFilter(ProjectObject = "SalesOrder", Mode = "R")]
        public string GetSalesOrder(string ID)
        {
            try
            {
                SalesOrderViewModel requisitionVM = new SalesOrderViewModel();
                requisitionVM = Mapper.Map<SalesOrder, SalesOrderViewModel>(_salesOrderBusiness.GetSalesOrder(Guid.Parse(ID)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = requisitionVM });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex });
            }
        }

        #endregion GetSalesOrder

        #region GetSalesOrderDetail
        [AuthSecurityFilter(ProjectObject = "SalesOrder", Mode = "R")]
        public string GetSalesOrderDetail(string ID)
        {
            try
            {
                List<SalesOrderDetailViewModel> salesOrderDetailVM = new List<SalesOrderDetailViewModel>();
                salesOrderDetailVM = Mapper.Map<List<SalesOrderDetail>, List<SalesOrderDetailViewModel>>(_salesOrderBusiness.GetSalesOrderDetail(Guid.Parse(ID)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = salesOrderDetailVM });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex });
            }
        }

        #endregion GetSalesOrderDetail

        #region DeleteSalesOrderDetail
        [AuthSecurityFilter(ProjectObject = "SalesOrder", Mode = "D")]
        public string DeleteSalesOrderDetail(string ID)
        {
            object result = null;
            try
            {
                if (string.IsNullOrEmpty(ID))
                {
                    throw new Exception("ID Missing");
                }
                result = _salesOrderBusiness.DeleteSalesOrderDetail(Guid.Parse(ID));
                return JsonConvert.SerializeObject(new { Result = "OK", Record = result, Message = _appConst.DeleteSuccess });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }

        }

        #endregion DeleteSalesOrderDetail

        #region GetCustomerDetails
        [AuthSecurityFilter(ProjectObject = "SalesOrder", Mode = "R")]
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

        #region DeleteSalesOrder
        [AuthSecurityFilter(ProjectObject = "SalesOrder", Mode = "D")]
        public string DeleteSalesOrder(string ID)
        {
            object result = null;
            try
            {
                if (string.IsNullOrEmpty(ID))
                {
                    throw new Exception("ID Missing");
                }
                result = _salesOrderBusiness.DeleteSalesOrder(Guid.Parse(ID));
                return JsonConvert.SerializeObject(new { Result = "OK", Record = result, Message = _appConst.DeleteSuccess });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }

        }

        #endregion DeleteSalesOrder

        #region GetProductListByCategoryCode
        public string GetProductListByCategoryCode(string code)
        {
            try
            {
                List<ProductViewModel> productList = Mapper.Map<List<Product>, List<ProductViewModel>>(_productBusiness.GetProductListByCategoryCode(code));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = productList, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion GetProductListByCategoryCode

        #region GetSaleOrderDetaiByGroupId
        [AuthSecurityFilter(ProjectObject = "SalesOrder", Mode = "R")]
        public string GetSaleOrderDetaiByGroupId(string ID)
        {
            try
            {
                List<SalesOrderDetailViewModel> salesOrderDetailVM = new List<SalesOrderDetailViewModel>();
                salesOrderDetailVM = Mapper.Map<List<SalesOrderDetail>, List<SalesOrderDetailViewModel>>(_salesOrderBusiness.GetSaleOrderDetaiByGroupId(Guid.Parse(ID)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = salesOrderDetailVM });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex });
            }
        }

        #endregion GetSaleOrderDetaiByGroupId

        #region ButtonStyling
        [HttpGet]
   //     [AuthSecurityFilter(ProjectObject = "SalesOrder", Mode = "")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("AddSalesOrder", "SalesOrder", new { code = "SALE" });
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetSalesOrderList('Reset');";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "ImportSalesOrderData();";
                    //---------------------------------------

                    break;
                case "Edit":

                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "New";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("AddSalesOrder", "SalesOrder", new { code = "SALE" });

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "Save(1);";

                    toolboxVM.deletebtn.Visible = true;
                    toolboxVM.deletebtn.Text = "Delete";
                    toolboxVM.deletebtn.Title = "Delete";
                    toolboxVM.deletebtn.Event = "DeleteClick()";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "Reset();";

                    //toolboxVM.SendForApprovalBtn.Visible = true;
                    //toolboxVM.SendForApprovalBtn.Text = "Send";
                    //toolboxVM.SendForApprovalBtn.Title = "Send For Approval";
                    //toolboxVM.SendForApprovalBtn.Event = "ShowSendForApproval('REQ');";

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Href = Url.Action("ListSalesOrder", "SalesOrder", new { code = "SALE" });

                    break;

                //case "Disable":

                //    toolboxVM.addbtn.Visible = true;
                //    toolboxVM.addbtn.Text = "New";
                //    toolboxVM.addbtn.Title = "Add New";
                //    toolboxVM.addbtn.Href = Url.Action("AddSalesOrder", "SalesOrder", new { code = "SALE" });

                //    toolboxVM.ListBtn.Visible = true;
                //    toolboxVM.ListBtn.Text = "List";
                //    toolboxVM.ListBtn.Title = "List";
                //    toolboxVM.ListBtn.Href = Url.Action("ListSalesOrder", "SalesOrder", new { code = "SALE" });

                //    break;
                case "Add":

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "Save(1);";

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Href = Url.Action("ListSalesOrder", "SalesOrder", new { code = "SALE" });
                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion
    }
}