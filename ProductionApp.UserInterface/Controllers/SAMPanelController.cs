using AutoMapper;
using SAMTool.BusinessServices.Contracts;
using SAMTool.DataAccessObject.DTO;
using ProductionApp.UserInterface.SecurityFilter;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ProductionApp.UserInterface.Models;


namespace ProductionApp.UserInterface.Controllers
{
    public class SAMPanelController : Controller
    {
      //  public string ReadAccess;
        IHomeBusiness _homeBusiness;
        public SAMPanelController(IHomeBusiness home)
        {
            _homeBusiness = home;
        }
    
        [AuthSecurityFilter(ProjectObject = "SAMPanel", Mode = "R")]
        public ActionResult Index(string code) 
        {
            ViewBag.SysModuleCode = code;
            // AppUA _appUA= Session["AppUA"] as AppUA;
            Permission permission = Session["UserRights"] as Permission;
              
            // string R = _permission.SubPermissionList.First(s => s.Name == "RHS").AccessCode;
            SAMPanelViewModel SAMPanelVMObj = new SAMPanelViewModel();
            List<SysMenuViewModel> SysMenuViewModelList = Mapper.Map<List<SysMenu>, List<SysMenuViewModel>>(_homeBusiness.GetAllSysLinks());
            if((permission.SubPermissionList!=null? permission.SubPermissionList.First(s => s.Name == "LHS").AccessCode:string.Empty).Contains("R"))
            {
                SAMPanelVMObj._LHSSysMenuViewModel = SysMenuViewModelList != null ? SysMenuViewModelList.Where(s => s.Type == "LHS").ToList() : new List<SysMenuViewModel>();
            }
            if ((permission.SubPermissionList != null ? permission.SubPermissionList.First(s => s.Name == "RHS").AccessCode : string.Empty).Contains("R"))
            {
                SAMPanelVMObj._RHSSysMenuViewModel = SysMenuViewModelList != null ? SysMenuViewModelList.Where(s => s.Type == "RHS").ToList() : new List<SysMenuViewModel>();
            }
            Session.Remove("UserRights");
            return View(SAMPanelVMObj);
        }
    }
}