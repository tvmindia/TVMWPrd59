using AutoMapper;
using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.UserInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Controllers
{
    public class HomeController : Controller
    {
        ISysModuleBusiness _sysModuleBusiness;
        public HomeController(ISysModuleBusiness sysModuleBusiness)
        {
            _sysModuleBusiness = sysModuleBusiness;
        }
        // GET: Home
        public ActionResult Index()
        {
            AppUA appUA = Session["AppUA"] as AppUA;
            HomeViewModel homeVm = new HomeViewModel();
            List<SysModuleViewModel> SysModuleVMList = Mapper.Map<List<SysModule>, List<SysModuleViewModel>>(_sysModuleBusiness.GetAllModule());
            homeVm.SysModuleVMObj = new SysModuleViewModel();
            homeVm.SysModuleVMObj.SysModuleList = SysModuleVMList;
            return View(homeVm);
        }
    }
}