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
    public class ManageAccessController : Controller
    {
        Const c = new Const();
        private IApplicationBusiness _applicationBusiness;
        private IManageAccessBusiness _manageAccessBusiness;
        private IRolesBusiness _rolesBusiness;
        private IAppObjectBusiness __appObjectBusiness;
        public ManageAccessController(IApplicationBusiness applicationBusiness, IManageAccessBusiness manageAccessBusiness, IRolesBusiness rolesBusiness, IAppObjectBusiness appObjectBusiness)
        {
            _applicationBusiness = applicationBusiness;
            _manageAccessBusiness = manageAccessBusiness;
            _rolesBusiness = rolesBusiness;
            __appObjectBusiness = appObjectBusiness;
        }
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ManageAccess", Mode = "R")]
        public ActionResult Index(string code)
        {
            ViewBag.SysModuleCode = code;
            ManageAccessViewModel _manageAccessViewModelObj = new ManageAccessViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            selectListItem = new List<SelectListItem>();
            string Appid = Request.QueryString["Appid"] != null ? Request.QueryString["Appid"].ToString() : "";
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
            _manageAccessViewModelObj.ApplicationList = selectListItem;
            selectListItem = new List<SelectListItem>();
            List<RolesViewModel> RoleList = null;
            if (Appid != "" && Appid != null)
            {
                _manageAccessViewModelObj.AppObjectObj = new AppObjectViewModel();
                _manageAccessViewModelObj.AppObjectObj.AppID = Guid.Parse(Appid);
                RoleList = Mapper.Map<List<Roles>, List<RolesViewModel>>(_rolesBusiness.GetAllAppRoles(Guid.Parse(Appid)));
                foreach (RolesViewModel Appl in RoleList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = Appl.RoleName,
                        Value = Appl.ID.ToString(),
                        Selected = false
                    });
                }
                _manageAccessViewModelObj.RoleList = selectListItem;
                _manageAccessViewModelObj.RoleID = Guid.Parse(RoleList[0].ID.ToString());
            }
            else
            {
                RoleList = Mapper.Map<List<Roles>, List<RolesViewModel>>(_rolesBusiness.GetAllAppRoles(Guid.Empty));
                foreach (RolesViewModel Appl in RoleList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = Appl.RoleName,
                        Value = Appl.ID.ToString(),
                        Selected = false
                    });
                }
                _manageAccessViewModelObj.RoleList = selectListItem;
            }
            return View(_manageAccessViewModelObj);
        }

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ManageAccess", Mode = "R")]
        public ActionResult SubobjectIndex(string id)
        {
            ViewBag.objectID = id;
            string Appid = Request.QueryString["appId"].ToString();
            ViewBag.AppID = Appid;
            ManageSubObjectAccessViewModel _manageSubObjectAccessViewModelObj = new ManageSubObjectAccessViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            selectListItem = new List<SelectListItem>();
            List<AppObjectViewModel> List = Mapper.Map<List<AppObject>, List<AppObjectViewModel>>(__appObjectBusiness.GetAllAppObjects(Guid.Parse(Appid)));
            foreach (AppObjectViewModel Appl in List)
            {
                if (Appl.ID == Guid.Parse(id))
                {
                    selectListItem.Add(new SelectListItem
                    {

                        Text = Appl.ObjectName,
                        Value = Appl.ID.ToString(),
                        Selected = true
                    });
                }
                else
                {
                    selectListItem.Add(new SelectListItem
                    {

                        Text = Appl.ObjectName,
                        Value = Appl.ID.ToString(),
                        Selected = false
                    });
                }

            }
            _manageSubObjectAccessViewModelObj.ObjectList = selectListItem;
            selectListItem = new List<SelectListItem>();
            List<RolesViewModel> RoleList = Mapper.Map<List<Roles>, List<RolesViewModel>>(_rolesBusiness.GetAllAppRoles(Guid.Parse(Appid)));
            foreach (RolesViewModel Appl in RoleList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = Appl.RoleName,
                    Value = Appl.ID.ToString(),
                    Selected = false
                });
            }
            _manageSubObjectAccessViewModelObj.RoleList = selectListItem;
            _manageSubObjectAccessViewModelObj.AppObjectObj = new AppObjectViewModel();
            _manageSubObjectAccessViewModelObj.AppObjectObj.ID = Guid.Parse(ViewBag.objectID != null ? ViewBag.objectID : Guid.Empty);
            return View(_manageSubObjectAccessViewModelObj);
        }
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ManageAccess", Mode = "R")]
        public string GetAllAppRoles(string AppID)
        {
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            selectListItem = new List<SelectListItem>();
            List<RolesViewModel> RoleList = Mapper.Map<List<Roles>, List<RolesViewModel>>(_rolesBusiness.GetAllAppRoles(Guid.Parse(AppID)));
            foreach (RolesViewModel Appl in RoleList)
            {
                selectListItem.Add(new SelectListItem
                {
                    Text = Appl.RoleName,
                    Value = Appl.ID.ToString(),
                    Selected = false
                });
            }
            return JsonConvert.SerializeObject(new { Result = "OK", Records = selectListItem });
        }
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ManageAccess", Mode = "R")]
        public string GetAllObjectAccess(string AppID, string RoleID)
        {
            List<ManageAccessViewModel> ItemList = Mapper.Map<List<ManageAccess>, List<ManageAccessViewModel>>(_manageAccessBusiness.GetAllObjectAccess((AppID != "" ? Guid.Parse(AppID) : Guid.Empty), (RoleID != "" ? Guid.Parse(RoleID) : Guid.Empty)));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = ItemList });

        }
       
        [AuthSecurityFilter(ProjectObject = "ManageAccess", Mode = "W")]
        [HttpPost]
        public string AddAccessChanges(ManageAccessViewModel manageAccessVM)
        {
          
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                DataAccessObject.DTO.Common commonObj = new DataAccessObject.DTO.Common();
                
                manageAccessVM.commonObj = new CommonViewModel();
                manageAccessVM.commonObj.CreatedBy = appUA.UserName;
                manageAccessVM.commonObj.CreatedDate = commonObj.GetCurrentDateTime();
                foreach (ManageAccessViewModel manageAccessVMObj in manageAccessVM.ManageAccessList)
                {
                    manageAccessVMObj.commonObj = new CommonViewModel();
                    manageAccessVMObj.commonObj = manageAccessVM.commonObj;
                }
                ManageAccessViewModel result = Mapper.Map<ManageAccess, ManageAccessViewModel>(_manageAccessBusiness.AddAccessChanges(Mapper.Map<List<ManageAccessViewModel>, List<ManageAccess>>(manageAccessVM.ManageAccessList)));
                return JsonConvert.SerializeObject(new { Result = "OK", Message = c.InsertSuccess, Records = result });
                // }

            }
            catch (Exception ex)
            {

                ConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
           
        }

        [AuthSecurityFilter(ProjectObject = "ManageAccess", Mode = "R")]
        [HttpGet]
        public string GetAllSubObjectAccess(string ObjectID, string RoleID)
        {
            List<ManageSubObjectAccessViewModel> ItemList = Mapper.Map<List<ManageSubObjectAccess>, List<ManageSubObjectAccessViewModel>>(_manageAccessBusiness.GetAllSubObjectAccess((ObjectID != "" ? Guid.Parse(ObjectID) : Guid.Empty), (RoleID != "" ? Guid.Parse(RoleID) : Guid.Empty)));
            return JsonConvert.SerializeObject(new { Result = "OK", Records = ItemList });

        }

        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "ManageAccess", Mode = "W")]
        public string AddSubObjectAccessChanges(ManageSubObjectAccessViewModel manageSubObjectAccessViewModelObj)
        {
            
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                DataAccessObject.DTO.Common commonObj = new DataAccessObject.DTO.Common();
                //if (ModelState.IsValid)
                // {
                manageSubObjectAccessViewModelObj.commonObj = new CommonViewModel();
                manageSubObjectAccessViewModelObj.commonObj.CreatedBy = appUA.UserName;
                manageSubObjectAccessViewModelObj.commonObj.CreatedDate = commonObj.GetCurrentDateTime();
                foreach (ManageSubObjectAccessViewModel ManageSubObjectAccessObj in manageSubObjectAccessViewModelObj.ManageSubObjectAccessList)
                {
                    ManageSubObjectAccessObj.commonObj = new CommonViewModel();
                    ManageSubObjectAccessObj.commonObj = manageSubObjectAccessViewModelObj.commonObj;
                }
                ManageSubObjectAccessViewModel r = Mapper.Map<ManageSubObjectAccess, ManageSubObjectAccessViewModel>(_manageAccessBusiness.AddSubObjectAccessChanges(Mapper.Map<List<ManageSubObjectAccessViewModel>, List<ManageSubObjectAccess>>(manageSubObjectAccessViewModelObj.ManageSubObjectAccessList)));
                return JsonConvert.SerializeObject(new { Result = "OK", Message = c.InsertSuccess, Records = r });
                //}

            }
            catch (Exception ex)
            {

                ConstMessage cm = c.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        
        }
        #region ButtonStyling
        [AuthSecurityFilter(ProjectObject = "ManageAccess", Mode = "R")]
        [HttpGet]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            Permission permission = Session["UserRights"] as Permission;
            ToolboxViewModel toolboxVMObj = new ToolboxViewModel();
            switch (actionType)
            {
                case "Default":

                    if ((permission.SubPermissionList != null ? permission.SubPermissionList.First(s => s.Name == "ButtonBack").AccessCode : string.Empty).Contains("R"))
                    {
                        toolboxVMObj.backbtn.Visible = true;
                    }
                    toolboxVMObj.backbtn.Text = "Back";
                    toolboxVMObj.backbtn.Title = "Back";
                    toolboxVMObj.backbtn.Event = "GobackMangeAccess()";

                    if ((permission.SubPermissionList != null ? permission.SubPermissionList.First(s => s.Name == "ButtonSave").AccessCode : string.Empty).Contains("R"))
                    {
                        toolboxVMObj.savebtn.Visible = true;
                    }

                    toolboxVMObj.savebtn.Disable = true;
                    toolboxVMObj.savebtn.Text = "Save";
                    toolboxVMObj.savebtn.DisableReason = "No changes yet";

                    if ((permission.SubPermissionList != null ? permission.SubPermissionList.First(s => s.Name == "ButtonReset").AccessCode : string.Empty).Contains("R"))
                    {
                        toolboxVMObj.resetbtn.Visible = true;
                    }
                    toolboxVMObj.resetbtn.Disable = true;
                    toolboxVMObj.resetbtn.Text = "Reset";
                    toolboxVMObj.resetbtn.DisableReason = "No changes yet";
                    break;
                case "Checked":

                    if ((permission.SubPermissionList != null ? permission.SubPermissionList.First(s => s.Name == "ButtonBack").AccessCode : string.Empty).Contains("R"))
                    {
                        toolboxVMObj.backbtn.Visible = true;
                    }
                    toolboxVMObj.backbtn.Text = "Back";
                    toolboxVMObj.backbtn.Title = "Back";
                    toolboxVMObj.backbtn.Event = "GobackMangeAccess()";

                    if ((permission.SubPermissionList != null ? permission.SubPermissionList.First(s => s.Name == "ButtonSave").AccessCode : string.Empty).Contains("R"))
                    {
                        toolboxVMObj.savebtn.Visible = true;
                    }
                    toolboxVMObj.savebtn.Title = "Update";
                    toolboxVMObj.savebtn.Text = "Save";
                    toolboxVMObj.savebtn.Event = "SaveChanges();";

                    if ((permission.SubPermissionList != null ? permission.SubPermissionList.First(s => s.Name == "ButtonReset").AccessCode : string.Empty).Contains("R"))
                    {
                        toolboxVMObj.resetbtn.Visible = true;
                    }
                    toolboxVMObj.resetbtn.Title = "Reset Changes";
                    toolboxVMObj.resetbtn.Text = "Reset";
                    toolboxVMObj.resetbtn.Event = "Reset();";
                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVMObj);
        }

        #endregion
    }
}