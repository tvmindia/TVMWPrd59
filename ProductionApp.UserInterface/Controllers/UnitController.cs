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
            //UnitViewModel unitVM = new UnitViewModel();
            unitVM.UnitCode = unitVM.Code;
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            unitVM.SelectList = new List<SelectListItem>();
            List<UnitViewModel> unitList = Mapper.Map<List<Unit>, List<UnitViewModel>>(_unitBusiness.GetUnitForSelectList());
            if (unitList != null)
                foreach (UnitViewModel unit in unitList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = unit.Description,
                        Value = unit.Code.ToString(),
                        Selected = false
                    });
                }
            unitVM.SelectList = selectListItem;
            return PartialView("_UnitDropdown", unitVM);
        }
        #endregion UnitDropdown

    }
}