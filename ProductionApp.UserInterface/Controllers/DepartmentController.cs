using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Controllers
{
    public class DepartmentController : Controller
    {
        AppConst _appConst = new AppConst();
        private Common _common = new Common();
        private IDepartmentBusiness _departmentBusiness;
        #region Constructor Injection
        public DepartmentController(IDepartmentBusiness departmentBusiness)
        {
            _departmentBusiness = departmentBusiness;
        }
        #endregion Constructor Injection
        public ActionResult Index()
        {
            return View();
        }
    }
}