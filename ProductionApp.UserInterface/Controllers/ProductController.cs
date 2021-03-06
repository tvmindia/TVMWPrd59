﻿using AutoMapper;
using Newtonsoft.Json;
using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.UserInterface.Models;
using ProductionApp.UserInterface.SecurityFilter;
using SAMTool.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Controllers
{
    public class ProductController : Controller
    {
        AppConst _appConst = new AppConst();
        private DataAccessObject.DTO.Common _common = new DataAccessObject.DTO.Common();
        private IProductBusiness _productBusiness;
        private IUnitBusiness _unitBusiness;
        private IProductCategoryBusiness _productCategoryBusiness;

        #region Constructor Injection
        public ProductController(IProductBusiness productBusiness, IUnitBusiness unitBusiness, IProductCategoryBusiness productCategoryBusiness)
        {
            _productBusiness = productBusiness;
            _unitBusiness = unitBusiness;
            _productCategoryBusiness = productCategoryBusiness;
        }
        #endregion Constructor Injection

        #region Index
        [AuthSecurityFilter(ProjectObject = "Product", Mode = "R")]
        public ActionResult Index(string code)
        {
            ViewBag.SysModuleCode = code;
            ProductAdvanceSearchViewModel productAdvanceSearchVM = new ProductAdvanceSearchViewModel();
            productAdvanceSearchVM.Unit = new UnitViewModel();
            productAdvanceSearchVM.Unit.UnitSelectList = _unitBusiness.GetUnitForSelectList();
            productAdvanceSearchVM.ProductCategory = new ProductCategoryViewModel();
            productAdvanceSearchVM.ProductCategory.ProductCategorySelectList = _productCategoryBusiness.GetProductCategoryForSelectList();
            return View(productAdvanceSearchVM);
        }
        #endregion Index

        #region GetAllProduct
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "Product", Mode = "R")]
        public JsonResult GetAllProduct(DataTableAjaxPostModel model, ProductAdvanceSearchViewModel productAdvanceSearchVM)
        {
            try
            {
                //setting options to our model
                productAdvanceSearchVM.DataTablePaging.Start = model.start;
                productAdvanceSearchVM.DataTablePaging.Length = (productAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : productAdvanceSearchVM.DataTablePaging.Length;

                // action inside a standard controller
                List<ProductViewModel> productVMList = Mapper.Map<List<Product>, List<ProductViewModel>>(_productBusiness.GetAllProduct(Mapper.Map<ProductAdvanceSearchViewModel, ProductAdvanceSearch>(productAdvanceSearchVM)));
                if (productAdvanceSearchVM.DataTablePaging.Length == -1)
                {
                    int totalResult = productVMList.Count != 0 ? productVMList[0].TotalCount : 0;
                    int filteredResult = productVMList.Count != 0 ? productVMList[0].FilteredCount : 0;
                    productVMList = productVMList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
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
                    recordsTotal = productVMList.Count != 0 ? productVMList[0].TotalCount : 0,
                    recordsFiltered = productVMList.Count != 0 ? productVMList[0].FilteredCount : 0,
                    data = productVMList
                });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return Json(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion GetAllProduct

        #region CheckProductCodeExist
        public ActionResult CheckProductCodeExist(ProductViewModel productVM)
        {
            bool exists = _productBusiness.CheckProductCodeExist(Mapper.Map<ProductViewModel, Product>(productVM));
            if (exists)
            {
                return Json("<p><span style='vertical-align: 2px'>Product code is in use </span> <i class='fa fa-close' style='font-size:19px; color: red'></i></p>", JsonRequestBehavior.AllowGet);
            }
            //var result = new { success = true, message = "Success" };
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion CheckProductCodeExist

        //#region CheckProductCodeExist
        //[AcceptVerbs("Get", "Post")]
        //public ActionResult CheckProductCodeExist(Product productVM)
        //{
        //    try
        //    {
        //        bool exists = productVM.IsUpdate ? false : _productBusiness.CheckProductCodeExist(Mapper.Map<ProductViewModel, Product>(productVM));
        //        if (exists)
        //        {
        //            return Json("<p><span style='vertical-align: 2px'>Product code already in use </span> <i class='fa fa-close' style='font-size:19px; color: red'></i></p>", JsonRequestBehavior.AllowGet);
        //        }
        //        //var result = new { success = true, message = "Success" };
        //        return Json(true, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        AppConstMessage cm = _appConst.GetMessage(ex.Message);
        //        return Json(new { Result = "ERROR", Message = cm.Message });
        //    }
        //}
        //#endregion CheckProductCodeExist

        #region InsertUpdateProduct
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "Product", Mode = "W")]
        public string InsertUpdateProduct(ProductViewModel productVM)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                productVM.Common = new CommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _common.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _common.GetCurrentDateTime(),
                };
                var result = _productBusiness.InsertUpdateProduct(Mapper.Map<ProductViewModel, Product>(productVM));
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion InsertUpdateProduct

        #region MasterPartial
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Product", Mode = "R")]
        public ActionResult MasterPartial(string masterCode)
        {
            ProductViewModel productVM = string.IsNullOrEmpty(masterCode) ? new ProductViewModel() : Mapper.Map<Product, ProductViewModel>(_productBusiness.GetProduct(Guid.Parse(masterCode)));
            productVM.IsUpdate = string.IsNullOrEmpty(masterCode) ? false : true;
            productVM.IsInvoiceInKG=productVM.IsUpdate==true? productVM.IsInvoiceInKG:true;
            productVM.ProductCategory = new ProductCategoryViewModel();
            productVM.ProductCategory.ProductCategorySelectList = _productCategoryBusiness.GetProductCategoryForSelectList();
            productVM.Unit = new UnitViewModel();
            productVM.Unit.UnitSelectList = _unitBusiness.GetUnitForSelectList();

            Permission permission = Session["UserRights"] as Permission;
            string p = permission.SubPermissionList.Where(li => li.Name == "OpeningAccess").First().AccessCode;
            @ViewBag.OpeningAccess = false;
            if (p.Contains("R") || p.Contains("W"))
            {
                @ViewBag.OpeningAccess = true;
            }

            return PartialView("_AddProductPartial", productVM);
        }
        #endregion MasterPartial

        #region GetProduct
        [AuthSecurityFilter(ProjectObject = "Product", Mode = "R")]
        public string GetProduct(string ID)
        {
            try
            {
                ProductViewModel productVM = new ProductViewModel();
                productVM = Mapper.Map<Product, ProductViewModel>(_productBusiness.GetProduct(Guid.Parse(ID)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = productVM });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex });
            }
        }

        #endregion GetProduct

        #region ProductDropdown
        public ActionResult ProductDropdown(string required,string type=null)
        {
            ViewBag.IsRequired = required;
            ProductViewModel productVM = new ProductViewModel()
            {
                ProductSelectList = _productBusiness.GetProductForSelectList(type)
            };            
            return PartialView("_ProductDropdown", productVM);
        }
        #endregion ProductDropdown

        #region DeleteProduct
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Product", Mode = "D")]
        public string DeleteProduct(Guid id)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                string deletedBy = appUA.UserName;
                DateTime createdDate = _common.GetCurrentDateTime();
                var result = _productBusiness.DeleteProduct(id, deletedBy,createdDate);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }

        }
        #endregion DeleteProduct

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Product", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddProductMaster('MSTR')";
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetProductList();";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "ImportProductData();";
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