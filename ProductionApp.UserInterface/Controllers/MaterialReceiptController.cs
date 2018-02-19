using AutoMapper;
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
        public void GetAllMaterialReceipt()
        {
            List<MaterialReceiptViewModel> materialReceiptList = Mapper.Map<List<MaterialReceipt>, List<MaterialReceiptViewModel>>(_materialReceiptBusiness.GetAllMaterialReceipt());
        }
        #endregion GetAllMaterialReceipt

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
                    //toolboxVM.resetbtn.Event = "ResetPurchaseOrderList();";

                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    //toolboxVM.PrintBtn.Event = "ImportPurchaseOrderData();";

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

        #endregion
    }
}