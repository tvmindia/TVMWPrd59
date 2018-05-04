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
    public class ApprovalStatusController : Controller
    {
        // GET: ApprovalStatus
        AppConst _appConst = new AppConst();
        private Common _common = new Common();
        private IApprovalStatusBusiness _approvalStatusBusiness;
        #region Constructor Injection
        public ApprovalStatusController(IApprovalStatusBusiness approvalStatusBusiness)
        {
            _approvalStatusBusiness = approvalStatusBusiness;
        }
        #endregion Constructor Injection
        #region ApprovalStatusDropdown
        public ActionResult ApprovalStatusDropdown()
        {
            ApprovalStatusViewModel statusVM = new ApprovalStatusViewModel();
            statusVM.StatusSelectList = _approvalStatusBusiness.GetApprovalStatusForSelectList();
            return PartialView("_ApprovalStatusDropdown", statusVM);
        }
        #endregion UnitDropdown
    }
}