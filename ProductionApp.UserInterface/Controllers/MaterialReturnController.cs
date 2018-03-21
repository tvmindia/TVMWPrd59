using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Controllers
{
    public class MaterialReturnController : Controller
    {
        IMaterialReturnBusiness _materialReturnBusiness;
        AppConst _appConst = new AppConst();
        public MaterialReturnController(IMaterialReturnBusiness materialReturnBusiness)
        {
            _materialReturnBusiness = materialReturnBusiness;
        }
        public ActionResult ViewMaterialReturn(string code)
        {
            ViewBag.SysModuleCode = code;
            return View();
        }
        public ActionResult NewMaterialReturn(string code)
        {
            ViewBag.SysModuleCode = code;
            return View();
        }
    }
}