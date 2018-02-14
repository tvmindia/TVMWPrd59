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
    public class PrivilegesController : Controller
    {
        private IPrivilegesBusiness _privillegesBusiness;
        private IApplicationBusiness _applicationBusiness;
        private IRolesBusiness _rolesBusiness;
        public PrivilegesController(IPrivilegesBusiness privillegesBusiness, IApplicationBusiness applicationBusiness, IRolesBusiness rolesBusiness)
        {
            _privillegesBusiness = privillegesBusiness;
            _applicationBusiness = applicationBusiness;
            _rolesBusiness = rolesBusiness;
        }

        // GET: Privileges
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Privileges", Mode = "R")]
        public ActionResult Index(string code)
        {
            ViewBag.SysModuleCode = code;
            PrivilegesViewModel _privillegesObj = new PrivilegesViewModel();
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
            _privillegesObj.ApplicationList = selectListItem;

            selectListItem = new List<SelectListItem>();
            List<RolesViewModel> RoleList = Mapper.Map<List<Roles>, List<RolesViewModel>>(_rolesBusiness.GetAllAppRoles(null));
            foreach (RolesViewModel Appl in RoleList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = Appl.RoleName,
                    Value = Appl.ID.ToString(),
                    Selected = false
                });
            }
            _privillegesObj.RoleList = selectListItem;
            return View(_privillegesObj);
        }

        [AuthSecurityFilter(ProjectObject = "PrivilegesView", Mode = "R")]
        public ActionResult PrivilegesView(string code)
        {
            ViewBag.SysModuleCode = code;
            return View();
        }


        #region InsertUpdatePrivileges

        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "Privileges", Mode = "W")]
        [ValidateAntiForgeryToken]
        public string InsertUpdatePrivileges(PrivilegesViewModel PrivilegesObj)
        {
            object result = null;
            if (ModelState.IsValid)
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                DataAccessObject.DTO.Common commonObj = new DataAccessObject.DTO.Common();
                if (PrivilegesObj.ID == Guid.Empty)
                {
                    try
                    {
                        PrivilegesObj.commonDetails = new CommonViewModel();
                        PrivilegesObj.commonDetails.CreatedBy = appUA.UserName;
                        PrivilegesObj.commonDetails.CreatedDate = commonObj.GetCurrentDateTime();
                        result = _privillegesBusiness.InsertPrivileges(Mapper.Map<PrivilegesViewModel, Privileges>(PrivilegesObj));

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
                        PrivilegesObj.commonDetails = new CommonViewModel();
                        PrivilegesObj.commonDetails.UpdatedBy = appUA.UserName;
                        PrivilegesObj.commonDetails.UpdatedDate = commonObj.GetCurrentDateTime();
                        result = _privillegesBusiness.UpdatePrivileges(Mapper.Map<PrivilegesViewModel, Privileges>(PrivilegesObj));
                    }
                    catch (Exception ex)
                    {
                        return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
                    }
                }
            }
            return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
        }

        #endregion InsertUpdatePrivileges
        #region GetAllPrivilegesForPV
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "PrivilegesView", Mode = "R")]
        public string GetAllPrivilegesForPV()
        {
            try
            {

                List<PrivilegesViewModel> List = Mapper.Map<List<Privileges>, List<PrivilegesViewModel>>(_privillegesBusiness.GetAllPrivileges());
                return JsonConvert.SerializeObject(new { Result = "OK", Records = List });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion GetAllPrivilegesForPV

        #region GetAllPrivileges
        [AuthSecurityFilter(ProjectObject = "Privileges", Mode = "R")]
        [HttpGet]
        public string GetAllPrivileges()
        {
            try
            {

                List<PrivilegesViewModel> List = Mapper.Map<List<Privileges>, List<PrivilegesViewModel>>(_privillegesBusiness.GetAllPrivileges());
                return JsonConvert.SerializeObject(new { Result = "OK", Records = List });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion GetAllPrivileges

        #region GetPrivilegesDetailsByID
        [AuthSecurityFilter(ProjectObject = "Privileges", Mode = "R")]
        [HttpGet]
        public string GetPrivilegesDetailsByID(string Id)
        {
            try
            {
                PrivilegesViewModel List = Mapper.Map<Privileges, PrivilegesViewModel>(_privillegesBusiness.GetPrivilegesDetailsByID(Id));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = List });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion GetPrivilegesDetailsByID


        #region DeletePrivileges
        [AuthSecurityFilter(ProjectObject = "Privileges", Mode = "D")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string DeletePrivileges(PrivilegesViewModel privilegesObj)
        {
            object result = null;

            if (privilegesObj.ID != Guid.Empty)
            {
                try
                {
                    result = _privillegesBusiness.DeletePrivileges(Mapper.Map<PrivilegesViewModel, Privileges>(privilegesObj));
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

        #endregion DeletePrivileges

        #region ChangeButtonStyleForVP
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "PrivilegesView", Mode = "R")]
        public ActionResult ChangeButtonStyleForVP(string ActionType)
        {
            Permission _permission = Session["UserRights"] as Permission;
            ToolboxViewModel ToolboxViewModelObj = new ToolboxViewModel();
            switch (ActionType)
            {
                case "GoBack":
                    if ((_permission.SubPermissionList != null ? _permission.SubPermissionList.First(s => s.Name == "ButtonBack").AccessCode : string.Empty).Contains("R"))
                    {
                        ToolboxViewModelObj.backbtn.Visible = true;
                    }
                    ToolboxViewModelObj.backbtn.Text = "Back";
                    ToolboxViewModelObj.backbtn.Title = "Back to list";
                    ToolboxViewModelObj.backbtn.Event = "goHome()";
                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", ToolboxViewModelObj);
        }
        #endregion ChangeButtonStyleForVP
        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Privileges", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVMObj = new ToolboxViewModel();
            Permission permission = Session["UserRights"] as Permission;
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
                case "GoBack":
                    if ((permission.SubPermissionList != null ? permission.SubPermissionList.First(s => s.Name == "ButtonBack").AccessCode : string.Empty).Contains("R"))
                    {
                        toolboxVMObj.backbtn.Visible = true;
                    }
                    toolboxVMObj.backbtn.Text = "Back";
                    toolboxVMObj.backbtn.Title = "Back to list";
                    toolboxVMObj.backbtn.Event = "goHome()";
                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVMObj);
        }

        #endregion
    }
}