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
    public class SupplierController : Controller
    {
        // GET: Supplier
        private Common _common = new Common();
        AppConst _appConst = new AppConst();
        private ISupplierBusiness _supplierBusiness;
        private ICustomerBusiness _customerBusiness;

        public SupplierController(ISupplierBusiness supplierBusiness, ICustomerBusiness customerBusiness)
        {
            _supplierBusiness = supplierBusiness;
            _customerBusiness = customerBusiness;
        }

        [AuthSecurityFilter(ProjectObject = "Supplier", Mode = "R")]
        public ActionResult ViewSupplier(string code)
        {
            ViewBag.SysModuleCode = code;

            return View();
        }

        [AuthSecurityFilter(ProjectObject = "Supplier", Mode = "R")]
        public ActionResult NewSupplier(string code, Guid? id)
        {
            ViewBag.SysModuleCode = code;
            SupplierViewModel supplierVM = new SupplierViewModel()
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
            supplierVM.ContactTitleList = selectListItem;
            return View(supplierVM);
        }

        public ActionResult SupplierDropdown()
        {
            SupplierViewModel supplierVM = new SupplierViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            supplierVM.SelectList= new List<SelectListItem>();
            List<SupplierViewModel> supplierList = Mapper.Map<List<Supplier>, List<SupplierViewModel>>(_supplierBusiness.GetSupplierForSelectList());
            if(supplierList!=null)
            foreach (SupplierViewModel supplier in supplierList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = supplier.CompanyName,
                    Value = supplier.ID.ToString(),
                    Selected = false
                });
            }
            supplierVM.SelectList = selectListItem;
            return PartialView("_SupplierDropdown",supplierVM);
                 
        }

        #region MasterPartial
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Supplier", Mode = "R")]
        public ActionResult MasterPartial(string masterCode)
        {
            SupplierViewModel supplierVM = string.IsNullOrEmpty(masterCode) ? new SupplierViewModel() : Mapper.Map<Supplier, SupplierViewModel>(_supplierBusiness.GetSupplier(Guid.Parse(masterCode)));
            supplierVM.IsUpdate = string.IsNullOrEmpty(masterCode) ? false : true;
            return PartialView("_AddSupplierPartial", supplierVM);
        }
        #endregion MasterPartial

        #region GetAllSupplier
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "Supplier", Mode = "R")]
        public JsonResult GetAllSupplier(DataTableAjaxPostModel model, SupplierAdvanceSearchViewModel supplierAdvanceSearchVM)
        {
            try
            {
                //setting options to our model
                supplierAdvanceSearchVM.DataTablePaging.Start = model.start;
                supplierAdvanceSearchVM.DataTablePaging.Length = (supplierAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : supplierAdvanceSearchVM.DataTablePaging.Length;

                // action inside a standard controller
                List<SupplierViewModel> customerVMList = Mapper.Map<List<Supplier>, List<SupplierViewModel>>(_supplierBusiness.GetAllSupplier(Mapper.Map<SupplierAdvanceSearchViewModel, SupplierAdvanceSearch>(supplierAdvanceSearchVM)));
                if (supplierAdvanceSearchVM.DataTablePaging.Length == -1)
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
        #endregion GetAllSupplier

        #region InsertUpdateSupplier
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "Supplier", Mode = "R")]
        public string InsertUpdateSupplier(SupplierViewModel supplierVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AppUA appUA = Session["AppUA"] as AppUA;
                    supplierVM.Common = new CommonViewModel
                    {
                        CreatedBy = appUA.UserName,
                        CreatedDate = _common.GetCurrentDateTime(),
                        UpdatedBy = appUA.UserName,
                        UpdatedDate = _common.GetCurrentDateTime(),
                    };
                    var result = _supplierBusiness.InsertUpdateSupplier(Mapper.Map<SupplierViewModel, Supplier>(supplierVM));
                    return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
                }
                catch (Exception ex)
                {
                    AppConstMessage cm = _appConst.GetMessage(ex.Message);
                    return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
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

        #region GetSupplierDetails
        [AuthSecurityFilter(ProjectObject = "Supplier", Mode = "R")]
        public string GetSupplierDetails(string id)
        {
            try
            {
                SupplierViewModel customerVM = Mapper.Map<Supplier, SupplierViewModel>(_supplierBusiness.GetSupplier(Guid.Parse(id)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = customerVM, Message = "Success" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = ex });
            }
        }
        #endregion GetSupplierDetails

        #region DeleteSupplier
        [AuthSecurityFilter(ProjectObject = "Supplier", Mode = "D")]
        public string DeleteSupplier(string id)
        {
            object result = null;
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    throw new Exception("Deletion Not Successfull");
                }
                result = _supplierBusiness.DeleteSupplier(Guid.Parse(id));
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = _appConst.DeleteSuccess });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record="", Message = cm.Message });
            }

        }
        #endregion DeleteSupplier

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Supplier", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewSupplier", "Supplier", new { Code = "MSTR" });
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetSupplierList();";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "ImportSupplierData();";
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
                    toolboxVM.ListBtn.Href = Url.Action("ViewSupplier", "Supplier", new { Code = "MSTR" });
                    break;

                case "Edit":

                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "New";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewSupplier", "Supplier", new { Code = "MSTR" });

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
                    toolboxVM.ListBtn.Href = Url.Action("ViewSupplier", "Supplier", new { Code = "MSTR" });

                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }
        #endregion ButtonStyling

    }
}