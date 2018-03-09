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

    }
}