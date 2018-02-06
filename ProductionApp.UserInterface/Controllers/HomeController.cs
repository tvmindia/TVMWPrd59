﻿using AutoMapper;
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
        IDynamicUIBusiness _dynamicUIBusiness;
        public HomeController(IDynamicUIBusiness dynamicUIBusiness)
        {
            _dynamicUIBusiness = dynamicUIBusiness;
        }
        // GET: Home
        public ActionResult Index()
        {
            AppUA appUA = Session["AppUA"] as AppUA;
            HomeViewModel homeVm = new HomeViewModel();
            List<AMCSysModuleViewModel> SysModuleVMList = Mapper.Map<List<AMCSysModule>, List<AMCSysModuleViewModel>>(_dynamicUIBusiness.GetAllModule());
            homeVm.SysModuleVMObj = new AMCSysModuleViewModel {
                SysModuleList = SysModuleVMList
            };
            return View(homeVm);
        }
    }
}