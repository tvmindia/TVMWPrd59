﻿using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.UserInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Newtonsoft.Json;
using ProductionApp.UserInterface.SecurityFilter;

namespace ProductionApp.UserInterface.Controllers
{
    public class ProductionTrackingController : Controller
    {

        #region Constructor Injection
        Settings settings = new Settings();
        Common _common = new Common();
        AppConst _appConst = new AppConst();
        private IProductionTrackingBusiness _ProductionTrackingBusiness;
        public ProductionTrackingController(IProductionTrackingBusiness ProductionTrackingBusiness)
        {
            _ProductionTrackingBusiness = ProductionTrackingBusiness;
        }
        #endregion Constructor Injection

        // GET: ProductionTracking
        #region NewProductionTracking
        [AuthSecurityFilter(ProjectObject = "ProductionTracking", Mode = "R")]
        public ActionResult NewProductionTracking(string code, string id)
        {
            ViewBag.SysModuleCode = code;

            ProductionTrackingViewModel ProductionTracking = new ProductionTrackingViewModel
            {
                ID = id == null ? Guid.Empty : Guid.Parse(id),
                IsUpdate = id == null ? false : true,
                EntryDateFormatted = _common.GetCurrentDateTime().ToString(settings.DateFormat),
                SubComponent = new SubComponentViewModel()
            };
            return View(ProductionTracking);
        }
        #endregion NewProductionTracking

        #region GetProductionTrackingSearchList
        public string GetProductionTrackingSearchList(string searchTerm)
        {
            try
            {
                List<ProductionTrackingViewModel> productionTrackingList = Mapper.Map<List<ProductionTracking>, List<ProductionTrackingViewModel>>(_ProductionTrackingBusiness.GetProductionTrackingSearchList(searchTerm));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = productionTrackingList, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion GetProductionTrackingSearchList

        #region InsertUpdateProductionTracking
        public string InsertUpdateProductionTracking(ProductionTrackingViewModel productionTrackingVM)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                productionTrackingVM.Common = new CommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _common.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _common.GetCurrentDateTime(),
                };


                object result = _ProductionTrackingBusiness.InsertUpdateProductionTracking(Mapper.Map<ProductionTrackingViewModel, ProductionTracking>(productionTrackingVM));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result, Message = _appConst.InsertSuccess });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion InsertUpdateProductionTracking

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ProductionTracking", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    //toolboxVM.addbtn.Href = Url.Action("NewMaterialReceipt", "MaterialReceipt", new { code = "PROD" });
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
                    //toolboxVM.addbtn.Href = Url.Action("NewMaterialReceipt", "MaterialReceipt", new { code = "PROD" });
                    toolboxVM.addbtn.Event = "";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "Reset();";

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    //toolboxVM.ListBtn.Href = Url.Action("ViewMaterialReceipt", "MaterialReceipt", new { code = "PROD" });
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
                    //toolboxVM.ListBtn.Href = Url.Action("ViewMaterialReceipt", "MaterialReceipt", new { code = "PROD" });
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