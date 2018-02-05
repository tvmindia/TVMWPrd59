using AutoMapper;
using Newtonsoft.Json;
using SAMTool.BusinessServices.Contracts;
using SAMTool.DataAccessObject.DTO;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.UserInterface.SecurityFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProductionApp.UserInterface.Models;

namespace ProductionApp.UserInterface.Controllers
{
    public class UserController : Controller
    {
        private IUserBusiness _userBusiness;
        private IRolesBusiness _rolesBusiness;
        private IApplicationBusiness _applicationBusiness;

        public UserController(IUserBusiness userBusiness, IRolesBusiness rolesBusiness, IApplicationBusiness applicationBusiness)
        {
            _userBusiness = userBusiness;
            _rolesBusiness = rolesBusiness;
            _applicationBusiness = applicationBusiness;
        }

       [AuthSecurityFilter(ProjectObject = "User", Mode = "R")]
        [HttpGet]
        public ActionResult Index()
        {
            
            UserViewModel userobj = new UserViewModel();
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
            userobj.ApplicationList = selectListItem;
            //userobj.RoleList = Mapper.Map<List<Roles>, List<RolesViewModel>>(_rolesBusiness.GetAllAppRoles(null));
            return View(userobj);
        }
        [HttpGet]
        public ActionResult GetRolesView(string ID)
        {
            UserViewModel userobj = new UserViewModel();
            userobj.RoleList = Mapper.Map<List<Roles>, List<RolesViewModel>>(_rolesBusiness.GetAllAppRoles(Guid.Parse(ID)));
            return PartialView("_RoleList", userobj);
        }

        #region InsertUpdateUser
        [AuthSecurityFilter(ProjectObject = "User", Mode = "W")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string InsertUpdateUser(UserViewModel UserObj)
        {
            object result = null;
            if (ModelState.IsValid)
            {
                AppUA _appUA = Session["AppUA"] as AppUA;
                if (UserObj.ID == Guid.Empty)
                {
                    try
                    {
                        UserObj.commonDetails = new CommonViewModel();
                        UserObj.commonDetails.CreatedBy = _appUA.UserName;
                        UserObj.commonDetails.CreatedDate = _appUA.DateTime;
                        result = _userBusiness.InsertUser(Mapper.Map<UserViewModel, User>(UserObj));
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
                        UserObj.commonDetails = new CommonViewModel();
                        UserObj.commonDetails.UpdatedBy = _appUA.UserName;
                        UserObj.commonDetails.UpdatedDate = _appUA.DateTime;
                        result = _userBusiness.UpdateUser(Mapper.Map<UserViewModel, User>(UserObj));
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

        #region GetAllUsers
        [AuthSecurityFilter(ProjectObject = "User", Mode = "R")]
        [HttpGet]
        public string GetAllUsers()
        {
            try
            {
                List<UserViewModel> userList = Mapper.Map<List<User>, List<UserViewModel>>(_userBusiness.GetAllUsers());
                return JsonConvert.SerializeObject(new { Result = "OK", Records = userList });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion GetAllUsers

        #region GetUserDetailsByID
        [AuthSecurityFilter(ProjectObject = "User", Mode = "R")]
        [HttpGet]
        public string GetUserDetailsByID(string Id)
        {
            try
            {

                UserViewModel userList = Mapper.Map<User, UserViewModel>(_userBusiness.GetUserDetailsByID(Id));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = userList });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion GetUserDetailsByID

        //DeleteUser

        #region DeleteUser
        [AuthSecurityFilter(ProjectObject = "User", Mode = "D")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string DeleteUser(UserViewModel UserObj)
        {
            object result = null;
            if (UserObj.ID != Guid.Empty)
            {
                try
                {
                    result = _userBusiness.DeleteUser(Mapper.Map<UserViewModel, User>(UserObj));
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

        #endregion DeleteUser


        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "User", Mode = "R")]
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