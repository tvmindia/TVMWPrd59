using AutoMapper;
using Newtonsoft.Json;
using SAMTool.BusinessServices.Contracts;
using SAMTool.DataAccessObject.DTO;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.UserInterface.SecurityFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ProductionApp.UserInterface.Models;

namespace ProductionApp.UserInterface.Controllers
{
    public class ApplicationController : Controller
    {
        private IApplicationBusiness _applicationBusiness;
        public ApplicationController(IApplicationBusiness applicationBusiness)
        {
            _applicationBusiness = applicationBusiness;
        }

        // GET: Application
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Application", Mode = "R")]
        public ActionResult Index()
        {
            return View();
        }

        #region InsertUpdateApplication
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "Application", Mode = "W")]
        public string InsertUpdateApplication(ApplicationViewModel applicationVM)
        {
            object result = null;
            if (ModelState.IsValid)
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                DataAccessObject.DTO.Common commonObj = new DataAccessObject.DTO.Common();
                if (applicationVM.ID == Guid.Empty)
                {
                    try
                    {
                        applicationVM.commonDetails = new CommonViewModel();
                        applicationVM.commonDetails.CreatedBy = appUA.UserName;
                        applicationVM.commonDetails.CreatedDate = commonObj.GetCurrentDateTime();
                        result = _applicationBusiness.InsertApplication(Mapper.Map<ApplicationViewModel, Application>(applicationVM));
                    }
                    catch (Exception ex)
                    {
                        return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                    }
                }
                else
                {
                    try
                    {
                        applicationVM.commonDetails = new CommonViewModel();
                        applicationVM.commonDetails.UpdatedBy = appUA.UserName;
                        applicationVM.commonDetails.UpdatedDate = commonObj.GetCurrentDateTime();
                        result = _applicationBusiness.UpdateApplication(Mapper.Map<ApplicationViewModel, Application>(applicationVM));
                    }
                    catch (Exception ex)
                    {
                        return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                    }
                }
            }
            return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
        }

        #endregion InsertUpdateEvent

        #region GetAllApplication
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Application", Mode = "R")]
        public string GetAllApplication()
        {
            try
            {

                List<ApplicationViewModel> applicationVMList = Mapper.Map<List<Application>, List<ApplicationViewModel>>(_applicationBusiness.GetAllApplication());
                return JsonConvert.SerializeObject(new { Result = "OK", Records = applicationVMList });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion GetAllApplication

        #region DeleteApplication
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "Application", Mode = "D")]
        public string DeleteApplication(ApplicationViewModel applicationVM)
        {
            object result = null;
            if (applicationVM.ID != Guid.Empty)
            {
                try
                {
                    result = _applicationBusiness.DeleteApplication(Mapper.Map<ApplicationViewModel, Application>(applicationVM));
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                }
            }
            return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
        }
        #endregion DeleteApplication

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Application", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            Permission permission = Session["UserRights"] as Permission;
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    if ((permission.SubPermissionList != null ? permission.SubPermissionList.First(s => s.Name == "ButtonAdd").AccessCode : string.Empty).Contains("R"))
                    {
                        toolboxVM.addbtn.Visible = true;
                    }
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "Add();";


                    if ((permission.SubPermissionList != null ? permission.SubPermissionList.First(s => s.Name == "ButtonBack").AccessCode : string.Empty).Contains("R"))
                    {
                        toolboxVM.backbtn.Visible = true;
                    }
                    toolboxVM.backbtn.Text = "Back";
                    toolboxVM.backbtn.Title = "Back to list";
                    toolboxVM.backbtn.Event = "goHome()";

                    break;
                case "Edit":
                    if ((permission.SubPermissionList != null ? permission.SubPermissionList.First(s => s.Name == "ButtonBack").AccessCode : string.Empty).Contains("R"))
                    {
                        toolboxVM.backbtn.Visible = true;
                    }
                    toolboxVM.backbtn.Text = "Back";
                    toolboxVM.backbtn.Title = "Back to list";
                    toolboxVM.backbtn.Event = "Back()";

                    if ((permission.SubPermissionList != null ? permission.SubPermissionList.First(s => s.Name == "ButtonSave").AccessCode : string.Empty).Contains("R"))
                    {
                        toolboxVM.savebtn.Visible = true;
                    }
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "save();";

                    if ((permission.SubPermissionList != null ? permission.SubPermissionList.First(s => s.Name == "ButtonDelete").AccessCode : string.Empty).Contains("R"))
                    {
                        toolboxVM.deletebtn.Visible = true;
                    }
                    toolboxVM.deletebtn.Text = "Delete";
                    toolboxVM.deletebtn.Title = "Delete";
                    toolboxVM.deletebtn.Event = "DeleteClick();";

                    if ((permission.SubPermissionList != null ? permission.SubPermissionList.First(s => s.Name == "ButtonReset").AccessCode : string.Empty).Contains("R"))
                    {
                        toolboxVM.resetbtn.Visible = true;
                    }
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "reset();";

                    break;
                case "Add":
                    if ((permission.SubPermissionList != null ? permission.SubPermissionList.First(s => s.Name == "ButtonBack").AccessCode : string.Empty).Contains("R"))
                    {
                        toolboxVM.backbtn.Visible = true;
                    }
                    toolboxVM.backbtn.Text = "Back";
                    toolboxVM.backbtn.Title = "Back to list";
                    toolboxVM.backbtn.Event = "Back()";

                    if ((permission.SubPermissionList != null ? permission.SubPermissionList.First(s => s.Name == "ButtonSave").AccessCode : string.Empty).Contains("R"))
                    {
                        toolboxVM.savebtn.Visible = true;
                    }
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "save();";

                    if ((permission.SubPermissionList != null ? permission.SubPermissionList.First(s => s.Name == "ButtonDelete").AccessCode : string.Empty).Contains("R"))
                    {
                        toolboxVM.deletebtn.Visible = true;
                    }
                    toolboxVM.deletebtn.Text = "Delete";
                    toolboxVM.deletebtn.Title = "Delete";
                    toolboxVM.deletebtn.Disable = true;
                    toolboxVM.deletebtn.Event = "DeleteClick()";

                    if ((permission.SubPermissionList != null ? permission.SubPermissionList.First(s => s.Name == "ButtonReset").AccessCode : string.Empty).Contains("R"))
                    {
                        toolboxVM.resetbtn.Visible = true;
                    }
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "reset();";

                    break;
              
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion ButtonStyling
    }
}