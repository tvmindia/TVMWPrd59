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
    public class ProductCategoryController : Controller
    {
        AppConst _appConst = new AppConst();
        private Common _common = new Common();
        private IProductCategoryBusiness _productCategoryBusiness;

        #region Constructor Injection
        public ProductCategoryController(IProductCategoryBusiness productCategoryBusiness)
        {
            _productCategoryBusiness = productCategoryBusiness;
        }
        #endregion Constructor Injection

        #region Index
        [AuthSecurityFilter(ProjectObject = "ProductCategory", Mode = "R")]
        public ActionResult Index(string code)
        {
            ViewBag.SysModuleCode = code;
            ProductCategoryAdvanceSearchViewModel productCategoryAdvanceSearchVM = new ProductCategoryAdvanceSearchViewModel();
            return View(productCategoryAdvanceSearchVM);
        }
        #endregion Index

        #region ProductCategory SelectList
        public ActionResult ProductCategorySelectList(string required)
        {
            ViewBag.IsRequired = required;
            ProductCategoryViewModel productCategoryVM = new ProductCategoryViewModel();
            productCategoryVM.ProductCategorySelectList = _productCategoryBusiness.GetProductCategoryForSelectList();
            return PartialView("_ProductCategoryDropdown", productCategoryVM);
        }
        #endregion ProductCategory SelectList


        #region GetAllProductCategory
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "ProductCategory", Mode = "R")]
        public JsonResult GetAllProductCategory(DataTableAjaxPostModel model, ProductCategoryAdvanceSearchViewModel productCategoryAdvanceSearchVM)
        {
            try
            {
                //setting options to our model
                productCategoryAdvanceSearchVM.DataTablePaging.Start = model.start;
                productCategoryAdvanceSearchVM.DataTablePaging.Length = (productCategoryAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : productCategoryAdvanceSearchVM.DataTablePaging.Length;

                // action inside a standard controller
                List<ProductCategoryViewModel> productCategoryVMList = Mapper.Map<List<ProductCategory>, List<ProductCategoryViewModel>>(_productCategoryBusiness.GetAllProductCategory(Mapper.Map<ProductCategoryAdvanceSearchViewModel, ProductCategoryAdvanceSearch>(productCategoryAdvanceSearchVM)));
                if (productCategoryAdvanceSearchVM.DataTablePaging.Length == -1)
                {
                    int totalResult = productCategoryVMList.Count != 0 ? productCategoryVMList[0].TotalCount : 0;
                    int filteredResult = productCategoryVMList.Count != 0 ? productCategoryVMList[0].FilteredCount : 0;
                    productCategoryVMList = productCategoryVMList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
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
                    recordsTotal = productCategoryVMList.Count != 0 ? productCategoryVMList[0].TotalCount : 0,
                    recordsFiltered = productCategoryVMList.Count != 0 ? productCategoryVMList[0].FilteredCount : 0,
                    data = productCategoryVMList
                });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return Json(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion GetAllProductCategory

        #region MasterPartial
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ProductCategory", Mode = "R")]
        public ActionResult MasterPartial(string masterCode)
        {
            ProductCategoryViewModel productCategoryVM = string.IsNullOrEmpty(masterCode) ? new ProductCategoryViewModel() : Mapper.Map<ProductCategory, ProductCategoryViewModel>(_productCategoryBusiness.GetProductCategory(masterCode));
            productCategoryVM.IsUpdate = string.IsNullOrEmpty(masterCode) ? false : true;
            return PartialView("_AddProductCategoryPartial", productCategoryVM);
        }
        #endregion MasterPartial

        #region InsertUpdateProductCategory
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "ProductCategory", Mode = "R")]
        public string InsertUpdateProductCategory(ProductCategoryViewModel productCategoryVM)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                productCategoryVM.Common = new CommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _common.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _common.GetCurrentDateTime(),
                };
                var result = _productCategoryBusiness.InsertUpdateProductCategory(Mapper.Map<ProductCategoryViewModel, ProductCategory>(productCategoryVM));
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion InsertUpdateProductCategory

        #region DeleteProductCategory
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ProductCategory", Mode = "R")]
        public string DeleteProductCategory(string code)
        {
            try
            {
                var result = _productCategoryBusiness.DeleteProductCategory(code);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion DeleteProductCategory

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ProductCategory", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddProductCategoryMaster('MSTR')";
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetProductCategoryList();";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "ImportProductCategoryData();";
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