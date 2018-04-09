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
    public class UnitController : Controller
    {
        AppConst _appConst = new AppConst();
        private Common _common = new Common();
        private IUnitBusiness _unitBusiness;

        #region Constructor Injection
        public UnitController(IUnitBusiness unitBusiness)
        {
            _unitBusiness = unitBusiness;
        }
        #endregion Constructor Injection
        // GET: Unit
        public ActionResult Index()
        {
            return View();
        }

        #region UnitDropdown
        public ActionResult UnitDropdown(UnitViewModel unitVM)
        {
            unitVM.UnitCode = unitVM.Code;
            unitVM.UnitSelectList = _unitBusiness.GetUnitForSelectList();
            return PartialView("_UnitDropdown", unitVM);
        }
        #endregion UnitDropdown

    }
}