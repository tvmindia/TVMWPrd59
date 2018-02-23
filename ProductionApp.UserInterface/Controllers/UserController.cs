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
        public ActionResult Index(string code)
        {
            ViewBag.SysModuleCode = code;
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
                AppUA appUA = Session["AppUA"] as AppUA;
                DataAccessObject.DTO.Common commonObj = new DataAccessObject.DTO.Common();
                if (UserObj.ID == Guid.Empty)
                {
                    try
                    {
                        UserObj.commonDetails = new CommonViewModel();
                        UserObj.commonDetails.CreatedBy = appUA.UserName;
                        UserObj.commonDetails.CreatedDate = commonObj.GetCurrentDateTime();
                        result = _userBusiness.InsertUser(Mapper.Map<UserViewModel, SAMTool.DataAccessObject.DTO.User>(UserObj));
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
                        UserObj.commonDetails.UpdatedBy = appUA.UserName;
                        UserObj.commonDetails.UpdatedDate = commonObj.GetCurrentDateTime();
                        result = _userBusiness.UpdateUser(Mapper.Map<UserViewModel, SAMTool.DataAccessObject.DTO.User>(UserObj));
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
                List<UserViewModel> userList = Mapper.Map<List<SAMTool.DataAccessObject.DTO.User>, List<UserViewModel>>(_userBusiness.GetAllUsers());
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

                UserViewModel userList = Mapper.Map<SAMTool.DataAccessObject.DTO.User, UserViewModel>(_userBusiness.GetUserDetailsByID(Id));
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
                    result = _userBusiness.DeleteUser(Mapper.Map<UserViewModel, SAMTool.DataAccessObject.DTO.User>(UserObj));
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

        #region UserDropdown
        public ActionResult UserDropdown(UserViewModel userVM)
        {
            userVM.UserID = userVM.ID;
            //UserViewModel userVM = new UserViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            userVM.userList = new List<SelectListItem>();
            List<UserViewModel> userList = Mapper.Map<List<SAMTool.DataAccessObject.DTO.User>, List<UserViewModel>>(_userBusiness.GetAllUsers());
            if (userList != null)
                foreach (UserViewModel user in userList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = user.LoginName,
                        Value = user.ID.ToString(),
                        Selected = false
                    });
                }
            userVM.userList = selectListItem;
            return PartialView("_UserDropdown", userVM);
        }
        #endregion UserDropdown

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "User", Mode = "R")]
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