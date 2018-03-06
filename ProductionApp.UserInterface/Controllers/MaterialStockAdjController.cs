using ProductionApp.UserInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.UserInterface.SecurityFilter;
using Newtonsoft.Json;
using AutoMapper;


namespace ProductionApp.UserInterface.Controllers
{
    public class MaterialStockAdjController : Controller
    {
        // GET: MaterialStockAdj
        private IMaterialBusiness _rawMaterialBusiness;
        private IMaterialStockAdjBusiness _materialStockAdjBusiness;
        //private IEmployeeBusiness _employeeBusiness;
        Common _common = new Common();
        AppConst _appConst = new AppConst();
        public MaterialStockAdjController(IMaterialStockAdjBusiness materialStockAdjBusiness, IMaterialBusiness rawMaterialBusiness)
        {
            _materialStockAdjBusiness = materialStockAdjBusiness;
            _rawMaterialBusiness = rawMaterialBusiness;
            //_employeeBusiness = employeeBusiness;
        }
        public ActionResult Index()
        {
            return View();
        }

        #region ListStockAdjustment
        public ActionResult ListStockAdjustment(string code)
        {
            ViewBag.SysModuleCode = code;            
            return View();

        }
        #endregion 

        #region AddStockAdjustment
        public ActionResult AddStockAdjustment(string code)
        {
            ViewBag.SysModule = code;
            return View();
        }
        #endregion 

        #region GetAllMaterialStockAdjustment
        public JsonResult GetAllMaterialStockAdjustment(DataTableAjaxPostModel model,MaterialStockAdjAdvanceSearchViewModel materialStockAdjAdvanceSearchVM)
        {
            materialStockAdjAdvanceSearchVM.DataTablePaging.Start = model.start;
            materialStockAdjAdvanceSearchVM.DataTablePaging.Length = (materialStockAdjAdvanceSearchVM.DataTablePaging.Length == 0 ? model.length : materialStockAdjAdvanceSearchVM.DataTablePaging.Length);
            List<MaterialStockAdjViewModel> materialStockAdjList = Mapper.Map<List<MaterialStockAdj>, List<MaterialStockAdjViewModel>>(_materialStockAdjBusiness.GetAllMaterialStockAdjustment(Mapper.Map<MaterialStockAdjAdvanceSearchViewModel, MaterialStockAdjAdvanceSearch>(materialStockAdjAdvanceSearchVM)));
            if (materialStockAdjAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = materialStockAdjList.Count != 0 ? materialStockAdjList[0].TotalCount : 0;
                int filteredResult = materialStockAdjList.Count != 0 ? materialStockAdjList[0].FilteredCount : 0;
                materialStockAdjList = materialStockAdjList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }

            return Json(new
            {
                draw = model.draw,
                recordsTotal = materialStockAdjList.Count != 0 ? materialStockAdjList[0].TotalCount : 0,
                recordsFiltered = materialStockAdjList.Count != 0 ? materialStockAdjList[0].FilteredCount : 0,
                data = materialStockAdjList
            });
        }
        #endregion

        #region ButtonStyling
        [HttpGet]
        //[AuthSecurityFilter(ProjectObject = "IssueToProduction", Mode = "")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("AddStockAdjustment", "MaterialStockAdj", new { code = "STR" });
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetStockAdjustmentList();";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "ImportStockAdjustmentData();";
                    //---------------------------------------

                    break;
                case "Edit":

                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "New";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("AddStockAdjustment", "MaterialStockAdj", new { code = "STR" });

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
                    toolboxVM.ListBtn.Href = Url.Action("ListStockAdjustment", "MaterialStockAdj", new { Code = "STR" });


                    //toolboxVM.CloseBtn.Visible = true;
                    //toolboxVM.CloseBtn.Text = "Close";
                    //toolboxVM.CloseBtn.Title = "Close";
                    //toolboxVM.CloseBtn.Event = "closeNav();";

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
                    toolboxVM.ListBtn.Href = Url.Action("ListStockAdjustment", "MaterialStockAdj", new { Code = "STR" });


                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion
    }
}