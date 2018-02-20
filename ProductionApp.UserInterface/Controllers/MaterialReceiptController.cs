using AutoMapper;
using Newtonsoft.Json;
using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.UserInterface.Models;
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
        IMaterialReceiptBusiness _materialReceiptBusiness;
        public MaterialReceiptController(IMaterialReceiptBusiness materialReceiptBusiness)
        {
            _materialReceiptBusiness = materialReceiptBusiness;
        }
        #endregion Constructor Injection

        // GET: MaterialReceipt
        public ActionResult Index(string code)
        {
            ViewBag.SysModuleCode = code;
            return View();
        }

        #region GetAllMaterialReceipt
        [HttpPost]
        public JsonResult GetAllMaterialReceipt(DataTableAjaxPostModel model, MaterialReceiptAdvanceSearchViewModel MaterialReceiptAdvanceSearchVM)
        {
            MaterialReceiptAdvanceSearchVM.DataTablePaging.Start = model.start;
            MaterialReceiptAdvanceSearchVM.DataTablePaging.Length = (MaterialReceiptAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : MaterialReceiptAdvanceSearchVM.DataTablePaging.Length;
            List<MaterialReceiptViewModel> materialReceiptList = Mapper.Map<List<MaterialReceipt>, List<MaterialReceiptViewModel>>(
                _materialReceiptBusiness.GetAllMaterialReceipt(
                    Mapper.Map<MaterialReceiptAdvanceSearchViewModel, MaterialReceiptAdvanceSearch>(MaterialReceiptAdvanceSearchVM)));
            if (MaterialReceiptAdvanceSearchVM.DataTablePaging.Length == -1)
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

        #region MaterialReceiptDropDown
        #endregion MaterialReceiptDropDown

        #region ButtonStyling
        [HttpGet]
        //[AuthSecurityFilter(ProjectObject = "Bank", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    //toolboxVM.addbtn.Href = Url.Action("AddPurchaseOrder", "PurchaseOrder", new { code = "PURCH" });
                    //toolboxVM.addbtn.Event = "";

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
                    break;
                case "Add":
                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }
        #endregion ButtonStyling
    }
}