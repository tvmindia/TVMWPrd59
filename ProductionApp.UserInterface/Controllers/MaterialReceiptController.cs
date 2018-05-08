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
    public class MaterialReceiptController : Controller
    {
        #region Constructor Injection
        private IMaterialReceiptBusiness _materialReceiptBusiness;
        private IMaterialBusiness _materialBusiness;
        private IPurchaseOrderBusiness _purchaseOrderBusiness;
        private IUnitBusiness _unitBusiness;
        private ISupplierBusiness _supplierBusiness;
        AppConst _appConst = new AppConst();
        Common _common = new Common();
        public MaterialReceiptController(IMaterialReceiptBusiness materialReceiptBusiness, IMaterialBusiness materialBusiness, ISupplierBusiness supplierBusiness, IPurchaseOrderBusiness purchaseOrderBusiness, IUnitBusiness unitBusiness)
        {
            _materialReceiptBusiness = materialReceiptBusiness;
            _materialBusiness = materialBusiness;
            _supplierBusiness = supplierBusiness;
            _purchaseOrderBusiness = purchaseOrderBusiness;
            _unitBusiness = unitBusiness;
        }
        #endregion Constructor Injection

        // GET: MaterialReceipt
        #region View MaterialReceipt Index
        [AuthSecurityFilter(ProjectObject = "MaterialReceipt", Mode = "W")]
        public ActionResult ViewMaterialReceipt(string code)
        {
            ViewBag.SysModuleCode = code;
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            List<SupplierViewModel> supplierList = Mapper.Map<List<Supplier>, List<SupplierViewModel>>(_supplierBusiness.GetSupplierForSelectList());
            MaterialReceiptAdvanceSearchViewModel materialReceiptAdvanceSearchVM = new MaterialReceiptAdvanceSearchViewModel()
            {
                Supplier = new SupplierViewModel()
                {
                    SelectList = new List<SelectListItem>()
                },
                PurchaseOrder = new PurchaseOrderViewModel()
                {
                    SelectList = _purchaseOrderBusiness.PurchaseOrderDropdownList(Guid.Empty)
                }
            };
            foreach (SupplierViewModel supplier in supplierList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = supplier.CompanyName,
                    Value = supplier.ID.ToString(),
                    Selected = false,
                });

            }
            materialReceiptAdvanceSearchVM.Supplier.SelectList = selectListItem;
            return View(materialReceiptAdvanceSearchVM);
        }
        #endregion View MaterialReceipt Index

        #region NewMaterialReceipt
        [AuthSecurityFilter(ProjectObject = "MaterialReceipt", Mode = "W")]
        public ActionResult NewMaterialReceipt(string code, Guid? id)
        {
            ViewBag.SysModuleCode = code;
            MaterialReceiptViewModel materialReceiptVM = new MaterialReceiptViewModel
            {
                ID = id == null ? Guid.Empty : (Guid)id,
                IsUpdate = id == null ? false : true,
                MaterialReceiptDetail = new MaterialReceiptDetailViewModel
                {
                    Material = new MaterialViewModel(),
                    Qty = 0,
                    QtyInKG=0
                }
            };
            return View(materialReceiptVM);
        }
        #endregion NewMaterialReceipt

        #region GetAllMaterialReceipt
        [AuthSecurityFilter(ProjectObject = "MaterialReceipt", Mode = "R")]
        [HttpPost]
        public JsonResult GetAllMaterialReceipt(DataTableAjaxPostModel model, MaterialReceiptAdvanceSearchViewModel materialReceiptAdvanceSearchVM)
        {
            materialReceiptAdvanceSearchVM.DataTablePaging.Start = model.start;
            materialReceiptAdvanceSearchVM.DataTablePaging.Length = (materialReceiptAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : materialReceiptAdvanceSearchVM.DataTablePaging.Length;
            List<MaterialReceiptViewModel> materialReceiptList = Mapper.Map<List<MaterialReceipt>, List<MaterialReceiptViewModel>>(_materialReceiptBusiness.GetAllMaterialReceipt(Mapper.Map<MaterialReceiptAdvanceSearchViewModel, MaterialReceiptAdvanceSearch>(materialReceiptAdvanceSearchVM)));
            if (materialReceiptAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = materialReceiptList.Count != 0 ? materialReceiptList[0].TotalCount : 0;
                int filteredResult = materialReceiptList.Count != 0 ? materialReceiptList[0].FilteredCount : 0;
                materialReceiptList = materialReceiptList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.None
            };
            return Json(new
            {
                draw = model.draw,
                recordsTotal = materialReceiptList.Count != 0 ? materialReceiptList[0].TotalCount : 0,
                recordsFiltered = materialReceiptList.Count != 0 ? materialReceiptList[0].FilteredCount : 0,
                data = materialReceiptList
            });
        }
        #endregion GetAllMaterialReceipt

        #region GetMaterial
        [AuthSecurityFilter(ProjectObject = "MaterialReceipt", Mode = "R")]
        public string GetMaterial(string id)
        {
            try
            {
                MaterialViewModel materialVM = new MaterialViewModel();
                materialVM = Mapper.Map<Material, MaterialViewModel>(_materialBusiness.GetMaterial(Guid.Parse(id)));
                return JsonConvert.SerializeObject(new { Result = "OK", Record = materialVM, Message = "Success" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Record = "", Message = ex });
            }
        }
        #endregion GetMaterial

        #region InsertUpdateMaterialReceipt
        [AuthSecurityFilter(ProjectObject = "MaterialReceipt", Mode = "W")]
        public string InsertUpdateMaterialReceipt(MaterialReceiptViewModel materialReceiptVM)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                materialReceiptVM.Common = new CommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _common.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _common.GetCurrentDateTime(),
                };
                //Deserialize items
                object resultFromJS = JsonConvert.DeserializeObject(materialReceiptVM.DetailJSON);
                string readableFormat = JsonConvert.SerializeObject(resultFromJS);
                materialReceiptVM.MaterialReceiptDetailList = JsonConvert.DeserializeObject<List<MaterialReceiptDetailViewModel>>(readableFormat);
                object result = _materialReceiptBusiness.InsertUpdateMaterialReceipt(Mapper.Map<MaterialReceiptViewModel, MaterialReceipt>(materialReceiptVM));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result, Message = _appConst.InsertSuccess });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion InsertUpdateMaterialReceipt

        #region GetAllPurchaseOrderItem
        [AuthSecurityFilter(ProjectObject = "MaterialReceipt", Mode = "R")]
        public string GetAllPurchaseOrderItem(string id)
        {
            try
            {
                PurchaseOrderViewModel purchaseOrderVM = new PurchaseOrderViewModel();
                purchaseOrderVM.PODDetail = Mapper.Map<List<PurchaseOrderDetail>, List<PurchaseOrderDetailViewModel>>(_purchaseOrderBusiness.GetPurchaseOrderDetail(Guid.Parse(id)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = purchaseOrderVM.PODDetail, Message = "Success" });
            }
            catch(Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Record = "", Message = ex });
            }
        }
        #endregion GetAllPurchaseOrderItem

        #region GetMaterialReceipt
        [AuthSecurityFilter(ProjectObject = "MaterialReceipt", Mode = "R")]
        public string GetMaterialReceipt(string id)
        {
            try
            {
                MaterialReceiptViewModel materialReceiptVM = Mapper.Map<MaterialReceipt, MaterialReceiptViewModel>(_materialReceiptBusiness.GetMaterialReceipt(Guid.Parse(id)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = materialReceiptVM, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion GetMaterialReceipt

        #region GetAllMaterialReceiptDetail
        [AuthSecurityFilter(ProjectObject = "MaterialReceipt", Mode = "R")]
        public string GetAllMaterialReceiptDetail(string id)
        {
            try
            {
                List<MaterialReceiptDetailViewModel> materialReceiptDetailList = Mapper.Map< List<MaterialReceiptDetail>, List< MaterialReceiptDetailViewModel >>(_materialReceiptBusiness.GetAllMaterialReceiptDetailByHeaderID(Guid.Parse(id)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = materialReceiptDetailList, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion GetAllMaterialReceiptDetail

        #region DeleteMaterialReceipt
        [AuthSecurityFilter(ProjectObject = "MaterialReceipt", Mode = "R")]
        public string DeleteMaterialReceipt(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    throw new Exception("ID Missing");
                }
                AppUA appUA = Session["AppUA"] as AppUA;
                MaterialReceipt materialReceipt = new MaterialReceipt();
                materialReceipt.Common = new Common();
                materialReceipt.ID = Guid.Parse(id);
                materialReceipt.Common.CreatedBy = appUA.UserName;
                materialReceipt.Common.CreatedDate = _common.GetCurrentDateTime();
                object result = _materialReceiptBusiness.DeleteMaterialReceipt(materialReceipt);
                return JsonConvert.SerializeObject(new { Result = "OK", Record = result, Message = _appConst.DeleteSuccess });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Record = "", Message = ex });
            }
        }
        #endregion DeleteMaterialReceipt

        #region DeleteMaterialReceiptDetail
        [AuthSecurityFilter(ProjectObject = "MaterialReceipt", Mode = "R")]
        public string DeleteMaterialReceiptDetail(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    throw new Exception("ID Missing");
                }
                AppUA appUA = Session["AppUA"] as AppUA;
                MaterialReceipt materialReceipt = new MaterialReceipt();
                materialReceipt.Common = new Common();
                materialReceipt.ID = Guid.Parse(id);
                materialReceipt.Common.CreatedBy = appUA.UserName;
                materialReceipt.Common.CreatedDate = _common.GetCurrentDateTime();
                object result = _materialReceiptBusiness.DeleteMaterialReceiptDetail(materialReceipt);
                return JsonConvert.SerializeObject(new { Result = "OK", Record = result, Message = _appConst.DeleteSuccess });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Record = "", Message = ex });
            }
        }
        #endregion DeleteMaterialReceiptDetail

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "MaterialReceipt", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewMaterialReceipt", "MaterialReceipt", new { code = "STR" });
                    toolboxVM.addbtn.Event = "";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetMaterialReceipt();";

                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "ImportMaterialReceipt();";

                    break;
                case "Edit":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewMaterialReceipt", "MaterialReceipt", new { code = "STR" });
                    toolboxVM.addbtn.Event = "";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "Reset();";
                    
                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Href = Url.Action("ViewMaterialReceipt", "MaterialReceipt", new { code = "STR" });
                    toolboxVM.ListBtn.Event = "";

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "Save()";

                    toolboxVM.deletebtn.Visible = true;
                    toolboxVM.deletebtn.Text = "Delete";
                    toolboxVM.deletebtn.Title = "Delete";
                    toolboxVM.deletebtn.Event = "DeleteClick()";
                    break;
                case "Add":
                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Href = Url.Action("ViewMaterialReceipt", "MaterialReceipt", new { code = "STR" });
                    toolboxVM.ListBtn.Event = "";

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "Save()";
                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }
        #endregion ButtonStyling
    }
}