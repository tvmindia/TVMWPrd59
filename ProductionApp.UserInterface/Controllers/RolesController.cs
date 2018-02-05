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
        public ActionResult Index()
        {
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
                AppUA _appUA = Session["AppUA"] as AppUA;
                if (rolesObj.ID == Guid.Empty)
                {
                    try
                    {
                        rolesObj.commonDetails = new CommonViewModel();
                        rolesObj.commonDetails.CreatedBy = _appUA.UserName;
                        rolesObj.commonDetails.CreatedDate = _appUA.DateTime;
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
                        rolesObj.commonDetails.UpdatedBy = _appUA.UserName;
                        rolesObj.commonDetails.UpdatedDate = _appUA.DateTime;
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
        public ActionResult ChangeButtonStyle(string ActionType)
        {
            Permission _permission = Session["UserRights"] as Permission;
            ToolboxViewModel ToolboxViewModelObj = new ToolboxViewModel();
            switch (ActionType)
            {
                case "List":
                    if ((_permission.SubPermissionList != null ? _permission.SubPermissionList.First(s => s.Name == "ButtonAdd").AccessCode : string.Empty).Contains("R"))
                    {
                        ToolboxViewModelObj.addbtn.Visible = true;
                    }
                    ToolboxViewModelObj.addbtn.Text = "Add";
                    ToolboxViewModelObj.addbtn.Title = "Add New";
                    ToolboxViewModelObj.addbtn.Event = "Add();";

                    if ((_permission.SubPermissionList != null ? _permission.SubPermissionList.First(s => s.Name == "ButtonBack").AccessCode : string.Empty).Contains("R"))
                    {
                        ToolboxViewModelObj.backbtn.Visible = true;
                    }
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back to list";
                    ToolboxViewModelObj.backbtn.Event = "goHome()";

                    break;
                case "Edit":
                    if ((_permission.SubPermissionList != null ? _permission.SubPermissionList.First(s => s.Name == "ButtonBack").AccessCode : string.Empty).Contains("R"))
                    {
                        ToolboxViewModelObj.backbtn.Visible = true;
                    }
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back to list";
                    ToolboxViewModelObj.backbtn.Event = "Back()";

                    if ((_permission.SubPermissionList != null ? _permission.SubPermissionList.First(s => s.Name == "ButtonSave").AccessCode : string.Empty).Contains("R"))
                    {
                        ToolboxViewModelObj.savebtn.Visible = true;
                    }
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save";
                    ToolboxViewModelObj.savebtn.Event = "save();";

                    if ((_permission.SubPermissionList != null ? _permission.SubPermissionList.First(s => s.Name == "ButtonDelete").AccessCode : string.Empty).Contains("R"))
                    {
                        ToolboxViewModelObj.deletebtn.Visible = true;
                    }
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete";
                    ToolboxViewModelObj.deletebtn.Event = "DeleteClick();";

                    if ((_permission.SubPermissionList != null ? _permission.SubPermissionList.First(s => s.Name == "ButtonReset").AccessCode : string.Empty).Contains("R"))
                    {
                        ToolboxViewModelObj.resetbtn.Visible = true;
                    }
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Title = "Reset";
                    ToolboxViewModelObj.resetbtn.Event = "reset();";

                    break;
                case "Add":
                    if ((_permission.SubPermissionList != null ? _permission.SubPermissionList.First(s => s.Name == "ButtonBack").AccessCode : string.Empty).Contains("R"))
                    {
                        ToolboxViewModelObj.backbtn.Visible = true;
                    }
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back to list";
                    ToolboxViewModelObj.backbtn.Event = "Back()";

                    if ((_permission.SubPermissionList != null ? _permission.SubPermissionList.First(s => s.Name == "ButtonSave").AccessCode : string.Empty).Contains("R"))
                    {
                        ToolboxViewModelObj.savebtn.Visible = true;
                    }
                    ToolboxViewModelObj.savebtn.Text = "Save";
                    ToolboxViewModelObj.savebtn.Title = "Save";
                    ToolboxViewModelObj.savebtn.Event = "save();";

                    if ((_permission.SubPermissionList != null ? _permission.SubPermissionList.First(s => s.Name == "ButtonDelete").AccessCode : string.Empty).Contains("R"))
                    {
                        ToolboxViewModelObj.deletebtn.Visible = true;
                    }
                    ToolboxViewModelObj.deletebtn.Text = "Delete";
                    ToolboxViewModelObj.deletebtn.Title = "Delete";
                    ToolboxViewModelObj.deletebtn.Disable = true;
                    ToolboxViewModelObj.deletebtn.Event = "DeleteClick()";

                    if ((_permission.SubPermissionList != null ? _permission.SubPermissionList.First(s => s.Name == "ButtonReset").AccessCode : string.Empty).Contains("R"))
                    {
                        ToolboxViewModelObj.resetbtn.Visible = true;
                    }
                    ToolboxViewModelObj.resetbtn.Text = "Reset";
                    ToolboxViewModelObj.resetbtn.Title = "Reset";
                    ToolboxViewModelObj.resetbtn.Event = "reset();";

                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", ToolboxViewModelObj);
        }

        #endregion
    }
}