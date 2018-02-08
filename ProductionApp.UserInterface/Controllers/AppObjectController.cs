using AutoMapper;
using Newtonsoft.Json;
using SAMTool.BusinessServices.Contracts;
using SAMTool.DataAccessObject.DTO;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.UserInterface.SecurityFilter;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ProductionApp.UserInterface.Models;

namespace ProductionApp.UserInterface.Controllers
{
    public class AppObjectController : Controller
    {
        Const _constant = new Const();
        IApplicationBusiness _applicationBusiness;
        IAppObjectBusiness _appObjectBusiness;
        public AppObjectController(IApplicationBusiness applicationBusiness, IAppObjectBusiness appObjectBusiness)
        {
            _applicationBusiness = applicationBusiness;
            _appObjectBusiness = appObjectBusiness;
        }
        // GET: AppObject
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "AppObject", Mode = "R")]
        public ActionResult Index()
        {
            if (Request.QueryString["appId"] != null)
            {
                string Appid = Request.QueryString["appId"].ToString();
                ViewBag.AppID = Appid;
            }

            AppObjectViewModel appObjectVM = new AppObjectViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            selectListItem = new List<SelectListItem>();
            List<ApplicationViewModel> ApplicationList = Mapper.Map<List<Application>, List<ApplicationViewModel>>(_applicationBusiness.GetAllApplication());
            foreach (ApplicationViewModel Appl in ApplicationList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = Appl.Name,
                    Value = Appl.ID.ToString(),
                    Selected = false
                });
            }
            appObjectVM.ApplicationList = selectListItem;
            return View(appObjectVM);
        }
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "AppObject", Mode = "R")]
        public ActionResult Subobjects(string id)
        {
            ViewBag.objectID = id;
            string Appid = Request.QueryString["appId"].ToString();
            ViewBag.AppID = Appid;

            AppSubobjectViewmodel appObjectVM = new AppSubobjectViewmodel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            selectListItem = new List<SelectListItem>();
            List<ApplicationViewModel> applicationVMList = Mapper.Map<List<Application>, List<ApplicationViewModel>>(_applicationBusiness.GetAllApplication());
            foreach (ApplicationViewModel applicationVMObj in applicationVMList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = applicationVMObj.Name,
                    Value = applicationVMObj.ID.ToString(),
                    Selected = false
                });
            }
            appObjectVM.ApplicationList = selectListItem;

            selectListItem = new List<SelectListItem>();
            List<AppObjectViewModel> List = Mapper.Map<List<AppObject>, List<AppObjectViewModel>>(_appObjectBusiness.GetAllAppObjects(Guid.Parse(Appid)));
            foreach (AppObjectViewModel appObjectVMObj in List)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = appObjectVMObj.ObjectName,
                    Value = appObjectVMObj.ID.ToString(),
                    Selected = false
                });
            }
            appObjectVM.ObjectList = selectListItem;

            return View(appObjectVM);
        }


        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "AppObject", Mode = "R")]
        public string GetAllAppObjects(string id)
        {
            List<AppObjectViewModel> appObjectVMList = Mapper.Map<List<AppObject>, List<AppObjectViewModel>>(_appObjectBusiness.GetAllAppObjects(Guid.Parse(id)));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = appObjectVMList });

        }
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "AppObject", Mode = "D")]
        public string DeleteObject(AppObjectViewModel appObjectVM)
        {
            try
            {
                AppObjectViewModel appObjectVMObj = Mapper.Map<AppObject, AppObjectViewModel>(_appObjectBusiness.DeleteObject(Mapper.Map<AppObjectViewModel, AppObject>(appObjectVM)));
                return JsonConvert.SerializeObject(new { Result = "OK", Message = _constant.DeleteSuccess, Records = appObjectVMObj });
            }
            catch (Exception ex)
            {
                ConstMessage cm = _constant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "AppObject", Mode = "W")]
        public string InserUpdateObject(AppObjectViewModel appObjectVM)
        {
            string result = "";
            try
            {
                if (ModelState.IsValid)
                {
                    AppUA appUA = Session["AppUA"] as AppUA;
                    DataAccessObject.DTO.Common commonObj = new DataAccessObject.DTO.Common();
                    appObjectVM.commonDetails = new CommonViewModel();
                    appObjectVM.commonDetails.CreatedBy = appUA.UserName;
                    appObjectVM.commonDetails.CreatedDate = commonObj.GetCurrentDateTime();
                    appObjectVM.commonDetails.UpdatedBy = appObjectVM.commonDetails.CreatedBy;
                    appObjectVM.commonDetails.UpdatedDate = appObjectVM.commonDetails.CreatedDate;
                    AppObjectViewModel appObjectVMObj = Mapper.Map<AppObject, AppObjectViewModel>(_appObjectBusiness.InsertUpdate(Mapper.Map<AppObjectViewModel, AppObject>(appObjectVM)));
                    return JsonConvert.SerializeObject(new { Result = "OK", Message = _constant.InsertSuccess, Records = appObjectVMObj });
                }
            }
            catch (Exception ex)
            {
                ConstMessage cm = _constant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
            return result;
        }

        //-----------------Sub-Object Methods-------------------
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "AppObject", Mode = "W")]
        public string InserUpdateSubobject(AppSubobjectViewmodel appSubObjectVM)
        {
            string result = "";
            try
            {
                if (ModelState.IsValid)
                {
                    AppUA appUA = Session["AppUA"] as AppUA;
                    DataAccessObject.DTO.Common commonObj = new DataAccessObject.DTO.Common();
                    appSubObjectVM.commonDetails = new CommonViewModel();
                    appSubObjectVM.commonDetails.CreatedBy = appUA.UserName;
                    appSubObjectVM.commonDetails.CreatedDate = commonObj.GetCurrentDateTime();
                    appSubObjectVM.commonDetails.UpdatedBy = appSubObjectVM.commonDetails.CreatedBy;
                    appSubObjectVM.commonDetails.UpdatedDate = appSubObjectVM.commonDetails.CreatedDate;
                    AppSubobjectViewmodel appSubObjectVMObj = Mapper.Map<AppSubobject, AppSubobjectViewmodel>(_appObjectBusiness.InsertUpdateSubObject(Mapper.Map<AppSubobjectViewmodel, AppSubobject>(appSubObjectVM)));
                    return JsonConvert.SerializeObject(new { Result = "OK", Message = _constant.InsertSuccess, Records = appSubObjectVMObj });
                }
            }
            catch (Exception ex)
            {

                ConstMessage cm = _constant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
            return result;
        }
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "AppObject", Mode = "R")]
        public string GetAllAppSubObjects(string ID)
        {
            List<AppSubobjectViewmodel> appSubObjectVMObjList = Mapper.Map<List<AppSubobject>, List<AppSubobjectViewmodel>>(_appObjectBusiness.GetAllAppSubObjects(ID));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = appSubObjectVMObjList });

        }
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "AppObject", Mode = "D")]
        public string DeleteSubObject(AppSubobjectViewmodel appSubObjectVM)
        {
            try
            {
                AppSubobjectViewmodel appSubObjectVMObj = Mapper.Map<AppSubobject, AppSubobjectViewmodel>(_appObjectBusiness.DeleteSubObject(Mapper.Map<AppSubobjectViewmodel, AppSubobject>(appSubObjectVM)));
                return JsonConvert.SerializeObject(new { Result = "OK", Message = _constant.DeleteSuccess, Records = appSubObjectVMObj });
            }
            catch (Exception ex)
            {
                ConstMessage cm = _constant.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "AppObject", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVMObj = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    
                    toolboxVMObj.addbtn.Visible = true;
                    toolboxVMObj.addbtn.Disable = true;
                    toolboxVMObj.addbtn.DisableReason = "No Application selected";
                    toolboxVMObj.addbtn.Text = "Add";
                    toolboxVMObj.addbtn.Title = "Add New";
                    toolboxVMObj.addbtn.Event = "AddNewObject()";

                   
                    toolboxVMObj.backbtn.Visible = true;
                    toolboxVMObj.backbtn.Text = "Back";
                    toolboxVMObj.backbtn.Title = "Back to list";
                    toolboxVMObj.backbtn.Event = "goHome()";
                    break;
                case "select":
                    toolboxVMObj.addbtn.Visible = true;
                    toolboxVMObj.addbtn.Text = "Add";
                    toolboxVMObj.addbtn.Title = "Add New";
                    toolboxVMObj.addbtn.Event = "AddNewObject()";

                   
                    toolboxVMObj.backbtn.Visible = true;
                    toolboxVMObj.backbtn.Text = "Back";
                    toolboxVMObj.backbtn.Title = "Back to list";
                    toolboxVMObj.backbtn.Event = "goHome()";

                    break;
                case "Edit":
                    
                    toolboxVMObj.addbtn.Visible = true;
                    toolboxVMObj.addbtn.Text = "Add";
                    toolboxVMObj.addbtn.Title = "Add New";
                    toolboxVMObj.addbtn.Event = "AddNewObject()";

                   
                    toolboxVMObj.backbtn.Visible = true;
                    toolboxVMObj.backbtn.Text = "Back";
                    toolboxVMObj.backbtn.Title = "Back to list";
                    toolboxVMObj.backbtn.Event = "goback()";

                  
                    toolboxVMObj.savebtn.Visible = true;
                    toolboxVMObj.savebtn.Title = "Save Object";
                    toolboxVMObj.savebtn.Text = "Save";
                    toolboxVMObj.savebtn.Event = "$('#btnSave').trigger('click');";

                    
                    toolboxVMObj.resetbtn.Visible = true;
                    toolboxVMObj.resetbtn.Title = "Reset Object";
                    toolboxVMObj.resetbtn.Text = "Reset";
                    toolboxVMObj.resetbtn.Event = "Reset()";

                    break;
                case "AddSub":
                   
                    toolboxVMObj.backbtn.Visible = true;
                    toolboxVMObj.backbtn.Text = "Back";
                    toolboxVMObj.backbtn.Title = "Back to list";
                    toolboxVMObj.backbtn.Event = "GoBack()";

                   
                    toolboxVMObj.savebtn.Visible = true;
                    toolboxVMObj.savebtn.Title = "Save Object";
                    toolboxVMObj.savebtn.Text = "Save";
                    toolboxVMObj.savebtn.Event = "$('#btnSave').trigger('click');";

                  
                    toolboxVMObj.resetbtn.Visible = true;
                    toolboxVMObj.resetbtn.Title = "Reset Object";
                    toolboxVMObj.resetbtn.Text = "Reset";
                    toolboxVMObj.resetbtn.Event = "Reset()";
                    break;
                case "tab1":

                    break;
                case "tab2":

                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVMObj);
        }

        #endregion ButtonStyling
    }
}