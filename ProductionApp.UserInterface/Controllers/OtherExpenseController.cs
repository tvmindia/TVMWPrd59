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
    public class OtherExpenseController : Controller
    {
        private IOtherExpenseBusiness _otherExpenseBusiness;
        private IChartOfAccountBusiness _chartOfAccountBusiness;
        Common _common = new Common();
        AppConst _appConst = new AppConst();
        public OtherExpenseController(IOtherExpenseBusiness otherExpenseBusiness, IChartOfAccountBusiness chartOfAccountBusiness)
        {
            _otherExpenseBusiness = otherExpenseBusiness;
            _chartOfAccountBusiness = chartOfAccountBusiness;
        }
        // GET: OtherExpense
        [AuthSecurityFilter(ProjectObject = "OtherExpense", Mode = "R")]
        public ActionResult NewOtherExpense(string code, Guid? id)
        {
            ViewBag.SysModuleCode = code;
            OtherExpenseViewModel otherExpenseVM = new OtherExpenseViewModel();
            otherExpenseVM.ID = id == null ? Guid.Empty : (Guid)id;
            otherExpenseVM.IsUpdate = id == null ? false : true;
            otherExpenseVM.ChartOfAccount = new ChartOfAccountViewModel();
            otherExpenseVM.ChartOfAccount.ChartOfAccountSelectList = _chartOfAccountBusiness.GetChartOfAccountForSelectList("EXP");
            otherExpenseVM.SelectList = _otherExpenseBusiness.GetAccountSubHeadForSelectList();
            return View(otherExpenseVM);
        }
        [AuthSecurityFilter(ProjectObject = "OtherExpense", Mode = "R")]
        public ActionResult ViewOtherExpense(string code)
        {
            ViewBag.SysModuleCode = code;
            return View();
        }

        #region GetAllOtherExpense
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "OtherExpense", Mode = "R")]
        public JsonResult GetAllOtherExpense(DataTableAjaxPostModel model, OtherExpenseAdvanceSearchViewModel otherExpenseAdvanceSearchVM)
        {
            otherExpenseAdvanceSearchVM.DataTablePaging.Start = model.start;
            otherExpenseAdvanceSearchVM.DataTablePaging.Length = (otherExpenseAdvanceSearchVM.DataTablePaging.Length == 0 ? model.length : otherExpenseAdvanceSearchVM.DataTablePaging.Length);
            List<OtherExpenseViewModel> otherExpenseList = Mapper.Map<List<OtherExpense>, List<OtherExpenseViewModel>>(_otherExpenseBusiness.GetAllOtherExpense(Mapper.Map<OtherExpenseAdvanceSearchViewModel, OtherExpenseAdvanceSearch>(otherExpenseAdvanceSearchVM)));
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.None
            };
            return Json(new
            {
                draw = model.draw,
                recordsTotal = otherExpenseList.Count != 0 ? otherExpenseList[0].TotalCount : 0,
                recordsFiltered = otherExpenseList.Count != 0 ? otherExpenseList[0].FilteredCount : 0,
                data = otherExpenseList

            });
        }
        #endregion GetAllOtherExpense

        #region InsertUpdateOtherExpense
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "OtherExpense", Mode = "R")]
        public string InsertUpdateOtherExpense(OtherExpenseViewModel otherExpense)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                otherExpense.Common = new CommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _common.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _common.GetCurrentDateTime(),
                };
                var result = _otherExpenseBusiness.InsertUpdateOtherExpense(Mapper.Map<OtherExpenseViewModel, OtherExpense>(otherExpense));
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion InsertUpdateOtherExpense

        #region GetOtherExpense
        [AuthSecurityFilter(ProjectObject = "OtherExpense", Mode = "R")]
        public string GetOtherExpense(string id)
        {
            try
            {
                OtherExpenseViewModel otherExpenseVM = new OtherExpenseViewModel();
                otherExpenseVM = Mapper.Map<OtherExpense, OtherExpenseViewModel>(_otherExpenseBusiness.GetOtherExpense(Guid.Parse(id)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = otherExpenseVM });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex });
            }
        }

        #endregion GetOtherExpense

        #region GetReversalReference
        [AuthSecurityFilter(ProjectObject = "OtherExpense", Mode = "R")]
        public string GetReversalReference(string accountCode)
        {
            try
            {
                List<OtherExpenseViewModel> otherExpenseList = Mapper.Map<List<OtherExpense>, List<OtherExpenseViewModel>>(_otherExpenseBusiness.GetReversalReference(accountCode));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = otherExpenseList });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex });
            }
        }

        #endregion GetReversalReference

        #region GetMaximumReducibleAmount
        [AuthSecurityFilter(ProjectObject = "OtherExpense", Mode = "R")]
        public string GetMaximumReducibleAmount(string refNumber)
        {
            try { 
            decimal reducibleAmount = _otherExpenseBusiness.GetMaximumReducibleAmount(refNumber);
            return JsonConvert.SerializeObject(new { Result = "OK", Records = reducibleAmount });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex });
            }
        }
        #endregion GetMaximumReducibleAmount

        #region DeleteOtherExpense
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "OtherExpense", Mode = "R")]
        public string DeleteOtherExpense(Guid id)
        {
            try
            {
                var result = _otherExpenseBusiness.DeleteOtherExpense(id);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }

        }
        #endregion DeleteOtherExpense

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "OtherExpense", Mode = "")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewOtherExpense", "OtherExpense", new { Code = "ACC" });
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "BindOrReloadOtherExpenseTable('Reset');";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "BindOrReloadOtherExpenseTable('Export');";
                    //---------------------------------------

                    break;
                case "Edit":

                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "New";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewOtherExpense", "OtherExpense", new { Code = "ACC" });

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "Save();";

                    toolboxVM.deletebtn.Visible = true;
                    toolboxVM.deletebtn.Text = "Delete";
                    toolboxVM.deletebtn.Title = "Delete";
                    toolboxVM.deletebtn.Event = "DeleteClick()";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "Reset();";

                    toolboxVM.SendForApprovalBtn.Visible = true;
                    toolboxVM.SendForApprovalBtn.Text = "Send";
                    toolboxVM.SendForApprovalBtn.Title = "Send For Approval";
                    toolboxVM.SendForApprovalBtn.Event = "ShowSendForApproval('OE');";

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Href = Url.Action("ViewOtherExpense", "OtherExpense", new { Code = "ACC" });

                    break;

                case "Disable":

                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "New";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewOtherExpense", "OtherExpense", new { Code = "ACC" });

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Href = Url.Action("ViewOtherExpense", "OtherExpense", new { Code = "ACC" });

                    break;
                case "Add":

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "Save();";

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Href = Url.Action("ViewOtherExpense", "OtherExpense", new { Code = "ACC" });
                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion
    }
}