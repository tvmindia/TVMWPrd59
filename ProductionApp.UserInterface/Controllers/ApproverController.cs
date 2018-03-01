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
    public class ApproverController : Controller
    {
        AppConst _appConst = new AppConst();
        private Common _common = new Common();
        private IApproverBusiness _approverBusiness;

        #region Constructor Injection
        public ApproverController(IApproverBusiness approverBusiness)
        {
            _approverBusiness = approverBusiness;
        }
        #endregion Constructor Injection

        #region Index
        [AuthSecurityFilter(ProjectObject = "Approver", Mode = "R")]// GET: Approver
        public ActionResult Index(string code)
        {
            ViewBag.SysModuleCode = code;
            ApproverAdvanceSearchViewModel approverAdvanceSearchVM = new ApproverAdvanceSearchViewModel();
            return View(approverAdvanceSearchVM);
        }
        #endregion Index

        #region GetAllApprover
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "Approver", Mode = "R")]
        public JsonResult GetAllApprover(DataTableAjaxPostModel model, ApproverAdvanceSearchViewModel approverAdvanceSearchVM)
        {
            try
            {
                //setting options to our model
                approverAdvanceSearchVM.DataTablePaging.Start = model.start;
                approverAdvanceSearchVM.DataTablePaging.Length = (approverAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : approverAdvanceSearchVM.DataTablePaging.Length;

                // action inside a standard controller
                List<ApproverViewModel> approverVMList = Mapper.Map<List<Approver>, List<ApproverViewModel>>(_approverBusiness.GetAllApprover(Mapper.Map<ApproverAdvanceSearchViewModel, ApproverAdvanceSearch>(approverAdvanceSearchVM)));
                if (approverAdvanceSearchVM.DataTablePaging.Length == -1)
                {
                    int totalResult = approverVMList.Count != 0 ? approverVMList[0].TotalCount : 0;
                    int filteredResult = approverVMList.Count != 0 ? approverVMList[0].FilteredCount : 0;
                    approverVMList = approverVMList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
                }
                var settings = new JsonSerializerSettings
                {
                    //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Formatting = Formatting.None
                };
                return Json(new
                {
                    // this is what datatables wants sending back
                    draw = model.draw,
                    recordsTotal = approverVMList.Count != 0 ? approverVMList[0].TotalCount : 0,
                    recordsFiltered = approverVMList.Count != 0 ? approverVMList[0].FilteredCount : 0,
                    data = approverVMList
                });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return Json(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion GetAllRawMaterial

        #region MasterPartial
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Approver", Mode = "R")]
        public ActionResult MasterPartial(string masterCode)
        {
            ApproverViewModel approverVM = string.IsNullOrEmpty(masterCode) ? new ApproverViewModel() : Mapper.Map<Approver, ApproverViewModel>(_approverBusiness.GetApprover(Guid.Parse(masterCode)));
            approverVM.IsUpdate = string.IsNullOrEmpty(masterCode) ? false : true;
            if (string.IsNullOrEmpty(masterCode))
            {
                approverVM.IsActive = true;
                approverVM.IsDefault = true;
                
            }
            //--For Manging disabled checkbox IsDefault--//
            approverVM.IsDefaultString = approverVM.IsDefault?"true":"false";
            return PartialView("_AddApproverPartial", approverVM);
        }
        #endregion MasterPartial

        #region InsertUpdateApprover
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "Approver", Mode = "R")]
        public string InsertUpdateApprover(ApproverViewModel approverVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //--For Manging disabled checkbox IsDefault--//
                    if (!approverVM.IsDefault)
                    {
                        approverVM.IsDefault = bool.Parse(approverVM.IsDefaultString);
                    }
                    AppUA appUA = Session["AppUA"] as AppUA;
                    approverVM.Common = new CommonViewModel
                    {
                        CreatedBy = appUA.UserName,
                        CreatedDate = _common.GetCurrentDateTime(),
                        UpdatedBy = appUA.UserName,
                        UpdatedDate = _common.GetCurrentDateTime(),
                    };

                    var result = _approverBusiness.InsertUpdateApprover(Mapper.Map<ApproverViewModel, Approver>(approverVM));
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
                }
                catch (Exception ex)
                {
                    AppConstMessage cm = _appConst.GetMessage(ex.Message);
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
                }
            }
            else
            {
                List<string> modelErrors = new List<string>();
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var modelError in modelState.Errors)
                    {
                        modelErrors.Add(modelError.ErrorMessage);
                    }
                }
                return JsonConvert.SerializeObject(new { Result = "VALIDATION", Message = string.Join(",", modelErrors) });
            }
        }
        #endregion InsertUpdateApprover

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Approver", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddApproverMaster('MSTR')";
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetApproverList();";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "ImportApproverData();";
                    //---------------------------------------
                    break;

                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }
        #endregion ButtonStyling

    }
}