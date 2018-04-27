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
    public class MaterialReturnController : Controller
    {
        IMaterialReturnBusiness _materialReturnBusiness;
        private IMaterialBusiness _rawMaterialBusiness;
        private IEmployeeBusiness _employeeBusiness;
        private ISupplierBusiness _supplierBusiness;
        private ITaxTypeBusiness _taxTypeBusiness;
        AppConst _appConst = new AppConst();
        Common _common = new Common();
        public MaterialReturnController(IMaterialReturnBusiness materialReturnBusiness, IEmployeeBusiness employeeBusiness, IMaterialBusiness rawMaterialBusiness, ISupplierBusiness supplierBusiness, ITaxTypeBusiness taxTypeBusiness)
        {
            _materialReturnBusiness = materialReturnBusiness;
            _employeeBusiness = employeeBusiness;
            _rawMaterialBusiness = rawMaterialBusiness;
            _supplierBusiness = supplierBusiness;
            _taxTypeBusiness = taxTypeBusiness;
        }
        [AuthSecurityFilter(ProjectObject = "MaterialReturn", Mode = "R")]
        public ActionResult ViewMaterialReturn(string code)
        {
            ViewBag.SysModuleCode = code;
            MaterialReturnAdvanceSearchViewModel materialReturnAdvanceSearchVM = new MaterialReturnAdvanceSearchViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            materialReturnAdvanceSearchVM.Employee = new EmployeeViewModel();
            materialReturnAdvanceSearchVM.Employee.SelectList = new List<SelectListItem>();
            List<EmployeeViewModel> employeeList = Mapper.Map<List<Employee>, List<EmployeeViewModel>>(_employeeBusiness.GetEmployeeForSelectList());
            if (employeeList != null)
            {
                foreach (EmployeeViewModel Emp in employeeList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = Emp.Name,
                        Value = Emp.ID.ToString(),
                        Selected = false,
                    });

                }
            }
            materialReturnAdvanceSearchVM.Employee.SelectList = selectListItem;
            selectListItem = new List<SelectListItem>();
            materialReturnAdvanceSearchVM.Supplier = new SupplierViewModel();
            materialReturnAdvanceSearchVM.Supplier.SelectList = new List<SelectListItem>();
            List<SupplierViewModel> supplierList = Mapper.Map<List<Supplier>, List<SupplierViewModel>>(_supplierBusiness.GetSupplierForSelectList());
            if (employeeList != null)
            {
                foreach (SupplierViewModel supplier in supplierList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = supplier.CompanyName,
                        Value = supplier.ID.ToString(),
                        Selected = false,
                    });

                }
            }
            materialReturnAdvanceSearchVM.Supplier.SelectList = selectListItem;
            return View(materialReturnAdvanceSearchVM);
        }

        [AuthSecurityFilter(ProjectObject = "MaterialReturn", Mode = "R")]
        public ActionResult NewMaterialReturn(string code, Guid? id)
        {
            ViewBag.SysModuleCode = code;
            MaterialReturnViewModel materialReturnVM = new MaterialReturnViewModel();
            materialReturnVM.ID = id == null ? Guid.Empty : (Guid)id;
            materialReturnVM.IsUpdate = id == null ? false : true;
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            materialReturnVM.Employee = new EmployeeViewModel();
            materialReturnVM.Employee.SelectList = new List<SelectListItem>();
            List<EmployeeViewModel> employeeList = Mapper.Map<List<Employee>, List<EmployeeViewModel>>(_employeeBusiness.GetEmployeeForSelectList());
            if (employeeList != null)
            {
                foreach (EmployeeViewModel Emp in employeeList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = Emp.Name,
                        Value = Emp.ID.ToString(),
                        Selected = false,
                    });

                }
            }
            materialReturnVM.Employee.SelectList = selectListItem;
            materialReturnVM.MaterialReturnDetail = new MaterialReturnDetailViewModel();
            return View(materialReturnVM);
        }

        #region GetAllReturnToSupplier
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "MaterialReturn", Mode = "R")]
        public JsonResult GetAllReturnToSupplier(DataTableAjaxPostModel model, MaterialReturnAdvanceSearchViewModel materialReturnAdvanceSearchVM)
        {
            materialReturnAdvanceSearchVM.DataTablePaging.Start = model.start;
            materialReturnAdvanceSearchVM.DataTablePaging.Length = (materialReturnAdvanceSearchVM.DataTablePaging.Length == 0 ? model.length : materialReturnAdvanceSearchVM.DataTablePaging.Length);
            List<MaterialReturnViewModel> materialIssueOrderList = Mapper.Map<List<MaterialReturn>, List<MaterialReturnViewModel>>(_materialReturnBusiness.GetAllReturnToSupplier(Mapper.Map<MaterialReturnAdvanceSearchViewModel, MaterialReturnAdvanceSearch>(materialReturnAdvanceSearchVM)));
            if (materialReturnAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = materialIssueOrderList.Count != 0 ? materialIssueOrderList[0].TotalCount : 0;
                int filteredResult = materialIssueOrderList.Count != 0 ? materialIssueOrderList[0].FilteredCount : 0;
                materialIssueOrderList = materialIssueOrderList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }

            return Json(new
            {
                draw = model.draw,
                recordsTotal = materialIssueOrderList.Count != 0 ? materialIssueOrderList[0].TotalCount : 0,
                recordsFiltered = materialIssueOrderList.Count != 0 ? materialIssueOrderList[0].FilteredCount : 0,
                data = materialIssueOrderList
            });
        }

        #endregion GetAllReturnToSupplier

        #region TaxTypeCode
        [AuthSecurityFilter(ProjectObject = "PurchaseOrder", Mode = "R")]
        public string GetTaxtype(string Code)
        {
            try
            {
                TaxTypeViewModel taxTypeVM = new TaxTypeViewModel();
                taxTypeVM = Mapper.Map<TaxType, TaxTypeViewModel>(_taxTypeBusiness.GetTaxTypeDetailsByCode(Code));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = taxTypeVM, Message = "Success" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = ex });
            }
        }
        #endregion TaxTypeByCode

        #region InsertUpdateReturnToSupplier
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "MaterialReturn", Mode = "R")]
        public string InsertUpdateReturnToSupplier(MaterialReturnViewModel materialReturnVM)
        {

            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                materialReturnVM.Common = new CommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _common.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _common.GetCurrentDateTime(),
                };
                //Deserialize items
                object ResultFromJS = JsonConvert.DeserializeObject(materialReturnVM.DetailJSON);
                string ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                materialReturnVM.MaterialReturnDetailList = JsonConvert.DeserializeObject<List<MaterialReturnDetailViewModel>>(ReadableFormat);
                var result = _materialReturnBusiness.InsertUpdateMaterialReturn(Mapper.Map<MaterialReturnViewModel, MaterialReturn>(materialReturnVM));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = cm.Message });
            }

        }
        #endregion InsertUpdateReturnToSupplier

        #region GetMaterialReturn
        [AuthSecurityFilter(ProjectObject = "MaterialReturn", Mode = "R")]
        public string GetMaterialReturn(string id)
        {
            try
            {
                MaterialReturnViewModel materialReturnVM = new MaterialReturnViewModel();
                materialReturnVM = Mapper.Map<MaterialReturn, MaterialReturnViewModel>(_materialReturnBusiness.GetMaterialReturn(Guid.Parse(id)));
                return JsonConvert.SerializeObject(new { Result = "OK", Record = materialReturnVM, Message = "Success" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Record = "", Message = ex });
            }
        }
        #endregion GetMaterialReturn

        #region GetMaterialReturnDetail
        [AuthSecurityFilter(ProjectObject = "MaterialReturn", Mode = "R")]
        public string GetMaterialReturnDetail(string id)
        {
            try
            {
                List<MaterialReturnDetailViewModel> materialReturnDetailVM = new List<MaterialReturnDetailViewModel>();
                materialReturnDetailVM = Mapper.Map<List<MaterialReturnDetail>, List<MaterialReturnDetailViewModel>>(_materialReturnBusiness.GetMaterialReturnDetail(Guid.Parse(id)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = materialReturnDetailVM, Message = "Success" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = ex });
            }
        }
        #endregion

        #region DeleteMaterialReturnDetail
        [AuthSecurityFilter(ProjectObject = "MaterialReturn", Mode = "R")]
        public string DeleteMaterialReturnDetail(string id)
        {
            object result = null;
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    throw new Exception("ID Missing");
                }
                result = _materialReturnBusiness.DeleteMaterialReturnDetail(Guid.Parse(id));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result, Message = _appConst.DeleteSuccess });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion

        #region DeleteMaterialReturn
        [AuthSecurityFilter(ProjectObject = "MaterialReturn", Mode = "R")]
        public string DeleteMaterialReturn(string id)
        {
            object result = null;
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    throw new Exception("ID Missing");
                }
                result = _materialReturnBusiness.DeleteMaterialReturn(Guid.Parse(id));
                return JsonConvert.SerializeObject(new { Result = "OK", Record = result, Message = _appConst.DeleteSuccess });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion

        #region GetMaterial
        public string GetMaterial(string id)
        {
            try
            {
                MaterialViewModel rawMaterialVM = new MaterialViewModel();
                if(id != "")
                rawMaterialVM = Mapper.Map<Material, MaterialViewModel>(_rawMaterialBusiness.GetMaterial(Guid.Parse(id)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = rawMaterialVM, Message = "Sucess" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = ex });
            }
        }
        #endregion

        #region GetSupplierDetails
        public string GetSupplierDetails(string supplierid)
        {
            try
            {
                SupplierViewModel supplierVM = Mapper.Map<Supplier, SupplierViewModel>(_supplierBusiness.GetSupplier(Guid.Parse(supplierid)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = supplierVM, Message = "Success" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = ex });
            }
        }
        #endregion GetSupplierDetails

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "MaterialReturn", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewMaterialReturn", "MaterialReturn", new { code = "STR" });
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResettblReturnToSupplier();";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "ImportReturnToSupplierData();";
                    //---------------------------------------

                    break;
                case "Edit":

                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "New";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewMaterialReturn", "MaterialReturn", new { code = "STR" });

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
                    toolboxVM.ListBtn.Event = "";
                    toolboxVM.ListBtn.Href = Url.Action("ViewMaterialReturn", "MaterialReturn", new { Code = "STR" });


                    break;
                case "Add":

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "Save();";

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Event = "";
                    toolboxVM.ListBtn.Href = Url.Action("ViewMaterialReturn", "MaterialReturn", new { Code = "STR" });


                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion
    }
}