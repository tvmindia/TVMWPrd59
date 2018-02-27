using AutoMapper;
using Newtonsoft.Json;
using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.UserInterface.Models;
using ProductionApp.UserInterface.SecurityFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Controllers
{
    public class DocumentApprovalController : Controller
    {

        Common _common = new Common();
        AppConst _appConst = new AppConst();
        // GET: DocumentApproval
        [AuthSecurityFilter(ProjectObject = "DocumentApproval", Mode = "R")]
        public ActionResult ViewPendingDocuments(string code)
        {
            ViewBag.SysModuleCode = code;
            return View();
        }
    }
}