using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.UserInterface.Models;

namespace ProductionApp.UserInterface.Controllers
{
    public class DynamicUIController : Controller
    {
        // GET: DynamicUI
        private IDynamicUIBusiness _dynamicUIBusiness;
        public DynamicUIController(IDynamicUIBusiness dynamicUIBusiness )
        {
            _dynamicUIBusiness = dynamicUIBusiness;
           
        }

        public ActionResult _MenuNavBar(string Code)
        {
            List<AMCSysMenu> menulist = _dynamicUIBusiness.GetAllMenu(string.IsNullOrEmpty(Code)?"":Code);
            DynamicUIViewModel dUIObj = new DynamicUIViewModel();
            dUIObj.AMCSysMenuViewModelList = Mapper.Map<List<AMCSysMenu>, List<AMCSysMenuViewModel>>(menulist);
            return View(dUIObj);
        }


        public ActionResult Index()
        {
            return View();
        }


        public ActionResult UnderConstruction(string code) {
            ViewBag.SysModuleCode = code;
            return View();
        }
    }
}