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
    public class StageController : Controller
    {
        AppConst _appConst = new AppConst();
        private Common _common = new Common();
        private IStageBusiness _stageBusiness;

        #region Constructor Injection
        public StageController(IStageBusiness stageBusiness)
        {
            _stageBusiness = stageBusiness;
        }
        #endregion Constructor Injection

        #region Index
        [AuthSecurityFilter(ProjectObject = "Stage", Mode = "R")]// GET: Stage
        public ActionResult Index(string code)
        {
            ViewBag.SysModuleCode = code;
            StageAdvanceSearchViewModel approverAdvanceSearchVM = new StageAdvanceSearchViewModel();
            return View(approverAdvanceSearchVM);
        }
        #endregion Index

        #region GetAllStage
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "Stage", Mode = "R")]
        public JsonResult GetAllStage(DataTableAjaxPostModel model, StageAdvanceSearchViewModel stageAdvanceSearchVM)
        {
            try
            {
                //setting options to our model
                stageAdvanceSearchVM.DataTablePaging.Start = model.start;
                stageAdvanceSearchVM.DataTablePaging.Length = (stageAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : stageAdvanceSearchVM.DataTablePaging.Length;

                // action inside a standard controller
                List<StageViewModel> stageVMList = Mapper.Map<List<Stage>, List<StageViewModel>>(_stageBusiness.GetAllStage(Mapper.Map<StageAdvanceSearchViewModel, StageAdvanceSearch>(stageAdvanceSearchVM)));
                if (stageAdvanceSearchVM.DataTablePaging.Length == -1)
                {
                    int totalResult = stageVMList.Count != 0 ? stageVMList[0].TotalCount : 0;
                    int filteredResult = stageVMList.Count != 0 ? stageVMList[0].FilteredCount : 0;
                    stageVMList = stageVMList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
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
                    recordsTotal = stageVMList.Count != 0 ? stageVMList[0].TotalCount : 0,
                    recordsFiltered = stageVMList.Count != 0 ? stageVMList[0].FilteredCount : 0,
                    data = stageVMList
                });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return Json(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion GetAllStage

        #region InsertUpdateStage
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "Stage", Mode = "R")]
        public string InsertUpdateStage(StageViewModel stageVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AppUA appUA = Session["AppUA"] as AppUA;
                    stageVM.Common = new CommonViewModel
                    {
                        CreatedBy = appUA.UserName,
                        CreatedDate = _common.GetCurrentDateTime(),
                        UpdatedBy = appUA.UserName,
                        UpdatedDate = _common.GetCurrentDateTime(),
                    };
                    var result = _stageBusiness.InsertUpdateStage(Mapper.Map<StageViewModel, Stage>(stageVM));
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
                }
                catch (Exception ex)
                {
                    AppConstMessage cm = _appConst.GetMessage(ex.Message);
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
                }
            }
            else
            {
                List<string> modelErrors = new List<string>();
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var modelError in modelState.Errors)
                    {
                        modelErrors.Add(modelError.ErrorMessage);
                    }
                }
                return JsonConvert.SerializeObject(new { Result = "VALIDATION", Message = string.Join(",", modelErrors) });
            }
        }
        #endregion InsertUpdateRawMaterial

        #region MasterPartial
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Stage", Mode = "R")]
        public ActionResult MasterPartial(string masterCode)
        {
            StageViewModel stageVM = string.IsNullOrEmpty(masterCode) ? new StageViewModel() : Mapper.Map<Stage, StageViewModel>(_stageBusiness.GetStage(Guid.Parse(masterCode)));
            stageVM.IsUpdate = string.IsNullOrEmpty(masterCode) ? false : true;
            return PartialView("_AddStagePartial", stageVM);
        }
        #endregion MasterPartial

        #region DeleteStage
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Stage", Mode = "R")]
        public string DeleteStage(Guid id)
        {
            try
            {
                var result = _stageBusiness.DeleteStage(id);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }

        }
        #endregion DeleteStage

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Stage", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddStageMaster('MSTR')";
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetStageList();";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "ImportStageData();";
                    //---------------------------------------
                    break;

                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }
        #endregion ButtonStyling

    }
}