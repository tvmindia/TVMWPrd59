using AutoMapper;
using Newtonsoft.Json;
using SAMTool.BusinessServices.Contracts;
using SAMTool.DataAccessObject.DTO;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.UserInterface.SecurityFilter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProductionApp.UserInterface.Models;

namespace ProductionApp.UserInterface.Controllers
{
    public class RolesController : Controller
    {
        private IRolesBusiness _rolesBusiness;
        private IApplicationBusiness _applicationBusiness;

        public RolesController(IRolesBusiness rolesBusiness, IApplicationBusiness applicationBusiness)
        {
            _rolesBusiness = rolesBusiness;
            _applicationBusiness = applicationBusiness;
        }


        // GET: Roles
        [AuthSecurityFilter(ProjectObject = "Roles", Mode = "R")]
        [HttpGet]
        public ActionResult Index(string code)
        {
            ViewBag.SysModuleCode = code;
            RolesViewModel _rolesObj = new RolesViewModel();
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
            _rolesObj.ApplicationList = selectListItem;
            return View(_rolesObj);
        }

        //#region GetAllRolesByApplication
        //[HttpGet] 
        //public string GetAllRolesByAppID(string AppId)
        //{
        //    try
        //    {
        //        List<RolesViewModel> rolesVMList = Mapper.Map<List<Roles>, List<RolesViewModel>>(_rolesBusiness.GetAllRoles());
        //        return JsonConvert.SerializeObject(new { Result = "OK", Records = rolesVMList });
        //    }
        //    catch (Exception ex)
        //    {
        //        return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}
        //#endregion GetAllRolesByApplication
       

        #region InsertUpdateRoles
        [AuthSecurityFilter(ProjectObject = "Roles", Mode = "W")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string InsertUpdateRoles(RolesViewModel rolesObj)
        {
            object result = null;
            if (ModelState.IsValid)
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                DataAccessObject.DTO.Common commonObj = new DataAccessObject.DTO.Common();
                if (rolesObj.ID == Guid.Empty)
                {
                    try
                    {
                        rolesObj.commonDetails = new CommonViewModel();
                        rolesObj.commonDetails.CreatedBy = appUA.UserName;
                        rolesObj.commonDetails.CreatedDate = commonObj.GetCurrentDateTime();
                        result = _rolesBusiness.InsertRoles(Mapper.Map<RolesViewModel, Roles>(rolesObj));
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
                        rolesObj.commonDetails = new CommonViewModel();
                        rolesObj.commonDetails.UpdatedBy = appUA.UserName;
                        rolesObj.commonDetails.UpdatedDate = commonObj.GetCurrentDateTime();
                        result = _rolesBusiness.UpdateRoles(Mapper.Map<RolesViewModel, Roles>(rolesObj));
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

        #region GetAllRoles
        [AuthSecurityFilter(ProjectObject = "Roles", Mode = "R")]
        [HttpGet]
        public string GetAllRoles()
        {
            try
            {
                List<RolesViewModel> rolesList = Mapper.Map<List<Roles>, List<RolesViewModel>>(_rolesBusiness.GetAllAppRoles(null));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = rolesList });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion GetAllRoles

        #region GetRolesDetailsByID
        [AuthSecurityFilter(ProjectObject = "Roles", Mode = "R")]
        [HttpGet]
        public string GetRolesDetailsByID(string Id)
        {
            try
            {

                RolesViewModel roleList = Mapper.Map<Roles, RolesViewModel>(_rolesBusiness.GetRolesDetailsByID(Id));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = roleList });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion GetRolesDetailsByID 

        #region DeleteRoles

        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "Roles", Mode = "D")]
        [ValidateAntiForgeryToken]
        public string DeleteRoles(RolesViewModel RolesObj)
        {
            object result = null;

            if (RolesObj.ID != Guid.Empty)
            {
                try
                {
                    result = _rolesBusiness.DeleteRoles(Mapper.Map<RolesViewModel, Roles>(RolesObj));
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                }
            }
            else
            {

            }

            return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
        }

        #endregion DeleteRoles


        #region ButtonStyling
        [AuthSecurityFilter(ProjectObject = "Roles", Mode = "R")]
        [HttpGet]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            Permission permission = Session["UserRights"] as Permission;
            ToolboxViewModel toolboxVMObj = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    if ((permission.SubPermissionList != null ? permission.SubPermissionList.First(s => s.Name == "ButtonAdd").AccessCode : string.Empty).Contains("R"))
                    {
                        toolboxVMObj.addbtn.Visible = true;
                    }
                    toolboxVMObj.addbtn.Text = "Add";
                    toolboxVMObj.addbtn.Title = "Add New";
                    toolboxVMObj.addbtn.Event = "Add();";

                    if ((permission.SubPermissionList != null ? permission.SubPermissionList.First(s => s.Name == "ButtonBack").AccessCode : string.Empty).Contains("R"))
                    {
                        toolboxVMObj.backbtn.Visible = true;
                    }
                    toolboxVMObj.backbtn.Text = "Back";
                    toolboxVMObj.backbtn.Title = "Back to list";
                    toolboxVMObj.backbtn.Event = "goHome()";

                    break;
                case "Edit":
                    if ((permission.SubPermissionList != null ? permission.SubPermissionList.First(s => s.Name == "ButtonBack").AccessCode : string.Empty).Contains("R"))
                    {
                        toolboxVMObj.backbtn.Visible = true;
                    }
                    toolboxVMObj.backbtn.Text = "Back";
                    toolboxVMObj.backbtn.Title = "Back to list";
                    toolboxVMObj.backbtn.Event = "Back()";

                    if ((permission.SubPermissionList != null ? permission.SubPermissionList.First(s => s.Name == "ButtonSave").AccessCode : string.Empty).Contains("R"))
                    {
                        toolboxVMObj.savebtn.Visible = true;
                    }
                    toolboxVMObj.savebtn.Text = "Save";
                    toolboxVMObj.savebtn.Title = "Save";
                    toolboxVMObj.savebtn.Event = "save();";

                    if ((permission.SubPermissionList != null ? permission.SubPermissionList.First(s => s.Name == "ButtonDelete").AccessCode : string.Empty).Contains("R"))
                    {
                        toolboxVMObj.deletebtn.Visible = true;
                    }
                    toolboxVMObj.deletebtn.Text = "Delete";
                    toolboxVMObj.deletebtn.Title = "Delete";
                    toolboxVMObj.deletebtn.Event = "DeleteClick();";

                    if ((permission.SubPermissionList != null ? permission.SubPermissionList.First(s => s.Name == "ButtonReset").AccessCode : string.Empty).Contains("R"))
                    {
                        toolboxVMObj.resetbtn.Visible = true;
                    }
                    toolboxVMObj.resetbtn.Text = "Reset";
                    toolboxVMObj.resetbtn.Title = "Reset";
                    toolboxVMObj.resetbtn.Event = "reset();";

                    break;
                case "Add":
                    if ((permission.SubPermissionList != null ? permission.SubPermissionList.First(s => s.Name == "ButtonBack").AccessCode : string.Empty).Contains("R"))
                    {
                        toolboxVMObj.backbtn.Visible = true;
                    }
                    toolboxVMObj.backbtn.Text = "Back";
                    toolboxVMObj.backbtn.Title = "Back to list";
                    toolboxVMObj.backbtn.Event = "Back()";

                    if ((permission.SubPermissionList != null ? permission.SubPermissionList.First(s => s.Name == "ButtonSave").AccessCode : string.Empty).Contains("R"))
                    {
                        toolboxVMObj.savebtn.Visible = true;
                    }
                    toolboxVMObj.savebtn.Text = "Save";
                    toolboxVMObj.savebtn.Title = "Save";
                    toolboxVMObj.savebtn.Event = "save();";

                    if ((permission.SubPermissionList != null ? permission.SubPermissionList.First(s => s.Name == "ButtonDelete").AccessCode : string.Empty).Contains("R"))
                    {
                        toolboxVMObj.deletebtn.Visible = true;
                    }
                    toolboxVMObj.deletebtn.Text = "Delete";
                    toolboxVMObj.deletebtn.Title = "Delete";
                    toolboxVMObj.deletebtn.Disable = true;
                    toolboxVMObj.deletebtn.Event = "DeleteClick()";

                    if ((permission.SubPermissionList != null ? permission.SubPermissionList.First(s => s.Name == "ButtonReset").AccessCode : string.Empty).Contains("R"))
                    {
                        toolboxVMObj.resetbtn.Visible = true;
                    }
                    toolboxVMObj.resetbtn.Text = "Reset";
                    toolboxVMObj.resetbtn.Title = "Reset";
                    toolboxVMObj.resetbtn.Event = "reset();";

                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVMObj);
        }

        #endregion
    }
}