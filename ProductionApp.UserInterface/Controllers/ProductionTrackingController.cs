using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.UserInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Newtonsoft.Json;

namespace ProductionApp.UserInterface.Controllers
{
    public class ProductionTrackingController : Controller
    {

        #region Constructor Injection
        Common _common = new Common();
        private IProductionTrackingBusiness _ProductionTrackingBusiness;
        public ProductionTrackingController(IProductionTrackingBusiness ProductionTrackingBusiness)
        {
            _ProductionTrackingBusiness = ProductionTrackingBusiness;
        }
        #endregion Constructor Injection

        // GET: ProductionTracking
        #region NewProductionTracking
        public ActionResult NewProductionTracking(string code, string id)
        {
            ViewBag.SysModuleCode = code;

            List<SelectListItem> selectList = new List<SelectListItem>();
            List<ProductionTrackingSearchViewModel> ProductionTrackingSearchList = Mapper.Map<List<ProductionTrackingSearch>, List<ProductionTrackingSearchViewModel>>(_ProductionTrackingBusiness.GetProductionTrackingSearchList());
            if (ProductionTrackingSearchList != null)
            {
                foreach(ProductionTrackingSearchViewModel ProductionTrackingSearch in ProductionTrackingSearchList)
                {
                    selectList.Add(new SelectListItem
                    {
                        Text = ProductionTrackingSearch.Text,
                        Value = ProductionTrackingSearch.Value,
                        Selected = false
                    });
                }
            }

            ProductionTrackingViewModel ProductionTracking = new ProductionTrackingViewModel
            {
                ID = id == null ? Guid.Empty : Guid.Parse(id),
                IsUpdate = id == null ? false : true,
                SelectList = selectList
            };
            return View(ProductionTracking);
        }
        #endregion NewProductionTracking

        #region ButtonStyling
        [HttpGet]
        //[AuthSecurityFilter(ProjectObject = "MaterialReceipt", Mode = "R")]
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