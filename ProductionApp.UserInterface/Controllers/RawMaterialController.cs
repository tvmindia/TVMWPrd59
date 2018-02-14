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
    public class RawMaterialController : Controller
    {
        private IRawMaterialBusiness _rawMaterialBusiness;
        public RawMaterialController(IRawMaterialBusiness rawMaterialBusiness)
        {
            _rawMaterialBusiness = rawMaterialBusiness;
        }
        // GET: RawMaterial
        public ActionResult Index(string code)
        {
            ViewBag.SysModuleCode = code;
            RawMaterialAdvanceSearchViewModel rawMaterialAdvanceSearchVM = new RawMaterialAdvanceSearchViewModel();
            return View(rawMaterialAdvanceSearchVM);
        }

        [HttpPost]
        public JsonResult GetAllRawMaterial(DataTableAjaxPostModel model,RawMaterialAdvanceSearchViewModel rawMaterialAdvanceSearchVM)
        {
            //setting options to our model
            rawMaterialAdvanceSearchVM.DataTablePaging.Start = model.start;
            rawMaterialAdvanceSearchVM.DataTablePaging.Length = (rawMaterialAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : rawMaterialAdvanceSearchVM.DataTablePaging.Length;

            //bankAdvanceSearchVM.OrderColumn = model.order[0].column;
            //bankAdvanceSearchVM.OrderDir = model.order[0].dir;

            // action inside a standard controller
            List<RawMaterialViewModel> rawMaterialObjList = Mapper.Map<List<RawMaterial>, List<RawMaterialViewModel>>(_rawMaterialBusiness.GetAllRawMaterial(Mapper.Map<RawMaterialAdvanceSearchViewModel, RawMaterialAdvanceSearch>(rawMaterialAdvanceSearchVM)));

            var settings = new JsonSerializerSettings
            {
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
            };
            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = rawMaterialObjList.Count != 0 ? rawMaterialObjList[0].TotalCount : 0,
                recordsFiltered = rawMaterialObjList.Count != 0 ? rawMaterialObjList[0].FilteredCount : 0,
                data = rawMaterialObjList
            });
        }

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
                    toolboxVM.addbtn.Event = "AddRawMaterialMaster()";
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetRawMaterialList();";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "ImportRawMaterialData();";
                    //---------------------------------------

                    break;
                case "Edit":

                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "New";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "openNav();";

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save RawMaterial";
                    toolboxVM.savebtn.Event = "Save();";

                    toolboxVM.deletebtn.Visible = true;
                    toolboxVM.deletebtn.Text = "Delete";
                    toolboxVM.deletebtn.Title = "Delete RawMaterial";
                    toolboxVM.deletebtn.Event = "Delete()";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "Reset();";

                    toolboxVM.CloseBtn.Visible = true;
                    toolboxVM.CloseBtn.Text = "Close";
                    toolboxVM.CloseBtn.Title = "Close";
                    toolboxVM.CloseBtn.Event = "closeNav();";

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
                    toolboxVM.ListBtn.Href = Url.Action("Index", "RawMaterial", new { Code = "MSTR" });


                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion
    }
}