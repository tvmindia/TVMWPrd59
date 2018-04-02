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
    public class SubComponentController : Controller
    {
        AppConst _appConst = new AppConst();
        private Common _common = new Common();
        private ISubComponentBusiness _subComponentBusiness;

        #region Constructor Injection
        public SubComponentController(ISubComponentBusiness subComponentBusiness)
        {
            _subComponentBusiness = subComponentBusiness;
        }
        #endregion Constructor Injection


        #region Index
        [AuthSecurityFilter(ProjectObject = "SubComponent", Mode = "R")]// GET: Stage
        public ActionResult Index(string code)
        {
            ViewBag.SysModuleCode = code;
            SubComponentAdvanceSearchViewModel subComponentAdvanceSearchVM = new SubComponentAdvanceSearchViewModel();
            return View(subComponentAdvanceSearchVM);
        }
        #endregion Index

        //#region CheckSubComponentCodeExist
        //[AcceptVerbs("Get", "Post")]
        //public ActionResult CheckSubComponentCodeExist(SubComponentViewModel subComponentVM)
        //{
        //    try
        //    {
        //        bool exists = subComponentVM.IsUpdate ? false : _subComponentBusiness.CheckSubComponentCodeExist(subComponentVM.Code);
        //        if (exists)
        //        {
        //            return Json("<p><span style='vertical-align: 2px'>Sub Component code already in use </span> <i class='fa fa-close' style='font-size:19px; color: red'></i></p>", JsonRequestBehavior.AllowGet);
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
        //#endregion CheckSubComponentCodeExist

        #region GetAllSubComponent
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "SubComponent", Mode = "R")]
        public JsonResult GetAllSubComponent(DataTableAjaxPostModel model, SubComponentAdvanceSearchViewModel subComponentAdvanceSearchVM)
        {
            try
            {
                //setting options to our model
                subComponentAdvanceSearchVM.DataTablePaging.Start = model.start;
                subComponentAdvanceSearchVM.DataTablePaging.Length = (subComponentAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : subComponentAdvanceSearchVM.DataTablePaging.Length;

                //bankAdvanceSearchVM.OrderColumn = model.order[0].column;
                //bankAdvanceSearchVM.OrderDir = model.order[0].dir;

                // action inside a standard controller
                List<SubComponentViewModel> subComponentVMList = Mapper.Map<List<SubComponent>, List<SubComponentViewModel>>(_subComponentBusiness.GetAllSubComponent(Mapper.Map<SubComponentAdvanceSearchViewModel, SubComponentAdvanceSearch>(subComponentAdvanceSearchVM)));
                if (subComponentAdvanceSearchVM.DataTablePaging.Length == -1)
                {
                    int totalResult = subComponentVMList.Count != 0 ? subComponentVMList[0].TotalCount : 0;
                    int filteredResult = subComponentVMList.Count != 0 ? subComponentVMList[0].FilteredCount : 0;
                    subComponentVMList = subComponentVMList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
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
                    recordsTotal = subComponentVMList.Count != 0 ? subComponentVMList[0].TotalCount : 0,
                    recordsFiltered = subComponentVMList.Count != 0 ? subComponentVMList[0].FilteredCount : 0,
                    data = subComponentVMList
                });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return Json(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion GetAllSubComponent

        #region InsertUpdateSubComponent
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "SubComponent", Mode = "R")]
        public string InsertUpdateSubComponent(SubComponentViewModel subComponentVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AppUA appUA = Session["AppUA"] as AppUA;
                    subComponentVM.Common = new CommonViewModel
                    {
                        CreatedBy = appUA.UserName,
                        CreatedDate = _common.GetCurrentDateTime(),
                        UpdatedBy = appUA.UserName,
                        UpdatedDate = _common.GetCurrentDateTime(),
                    };
                    var result = _subComponentBusiness.InsertUpdateSubComponent(Mapper.Map<SubComponentViewModel, SubComponent>(subComponentVM));
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
        #endregion InsertUpdateSubComponent

        #region MasterPartial
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SubComponent", Mode = "R")]
        public ActionResult MasterPartial(string masterCode)
        {
            SubComponentViewModel subComponentVM = string.IsNullOrEmpty(masterCode) ? new SubComponentViewModel() : Mapper.Map<SubComponent, SubComponentViewModel>(_subComponentBusiness.GetSubComponent(Guid.Parse(masterCode)));
            subComponentVM.IsUpdate = string.IsNullOrEmpty(masterCode) ? false : true;
            return PartialView("_AddSubComponentPartial", subComponentVM);
        }
        #endregion MasterPartial

        #region DeleteSubComponent
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SubComponent", Mode = "R")]
        public string DeleteSubComponent(Guid id)
        {
            try
            {
                var result = _subComponentBusiness.DeleteSubComponent(id);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }

        }
        #endregion DeleteSubComponent

        #region SubComponentDropdown
        public ActionResult SubComponentDropdown(SubComponentViewModel subComponentVM)
        {
            try
            {
                List<SelectListItem> selectListItem = new List<SelectListItem>();
                subComponentVM.SelectList = new List<SelectListItem>();
                List<SubComponentViewModel> subComponentList = Mapper.Map<List<SubComponent>, List<SubComponentViewModel>>(_subComponentBusiness.GetSubComponentForSelectList());
                if (subComponentList != null)
                {
                    foreach (SubComponentViewModel subComponent in subComponentList)
                    {
                        selectListItem.Add(new SelectListItem
                        {
                            Text = subComponent.Description,
                            Value = subComponent.ID.ToString(),
                            Selected = false
                        });
                    }
                }
                subComponentVM.SelectList = selectListItem;
                return PartialView("_SubComponentDropdown", subComponentVM);
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return Json(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion SubComponentDropdown

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "SubComponent", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddSubComponentMaster('MSTR')";
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetSubComponentList();";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "ImportSubComponentData();";
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