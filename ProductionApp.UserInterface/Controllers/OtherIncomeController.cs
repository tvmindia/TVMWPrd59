using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.UserInterface.Models;
using ProductionApp.UserInterface.SecurityFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Controllers
{
    public class OtherIncomeController : Controller
    {
        // GET: OtherIncome
        #region Constructor Injection
        private IOtherIncomeBusiness _otherIncomeBusiness;
        private IChartOfAccountBusiness _chartOfAccountBusiness;
        AppConst _appConst = new AppConst();
        private Common _common = new Common();
        public OtherIncomeController(IOtherIncomeBusiness otherIncomeBusiness, IChartOfAccountBusiness chartOfAccountBusiness)
        {
            _otherIncomeBusiness = otherIncomeBusiness;
            _chartOfAccountBusiness = chartOfAccountBusiness;
        }
        #endregion Constructor Injection

        #region Views
        [AuthSecurityFilter(ProjectObject = "OtherIncome", Mode = "R")]
        public ActionResult ViewOtherIncome(string code)
        {
            ViewBag.SysModuleCode = code;
            OtherIncomeAdvanceSearchViewModel otherIncomeAdvanceSearchVM = new OtherIncomeAdvanceSearchViewModel()
            {
                ChartOfAccount = new ChartOfAccountViewModel()
                {
                    ChartOfAccountSelectList = _chartOfAccountBusiness.GetChartOfAccountForSelectList("INC")
                }
            };
            return View(otherIncomeAdvanceSearchVM);
        }

        [AuthSecurityFilter(ProjectObject = "OtherIncome", Mode = "R")]
        public ActionResult NewOtherIncome(string code, Guid? id)
        {
            ViewBag.SysModuleCode = code;
            OtherIncomeViewModel otherIncomeVM = new OtherIncomeViewModel()
            {
                ID = (id == null) ? Guid.Empty : (Guid)id,
                IsUpdate = (id == null) ? false : true,
                SelectList = _otherIncomeBusiness.GetAllAccountSubHeadForSelectList()
            };
            return View(otherIncomeVM);
        }
        #endregion Views

        #region GetAllOtherIncome
        [AuthSecurityFilter(ProjectObject = "OtherIncome", Mode = "R")]
        [HttpPost]
        public JsonResult GetAllOtherIncome(DataTableAjaxPostModel model,OtherIncomeAdvanceSearchViewModel otherIncomeAdvanceSearchVM)
        {
            otherIncomeAdvanceSearchVM.DataTablePaging.Start = model.start;
            otherIncomeAdvanceSearchVM.DataTablePaging.Length = (otherIncomeAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : otherIncomeAdvanceSearchVM.DataTablePaging.Length;
            List<OtherIncomeViewModel> otherIncomeList = Mapper.Map<List<OtherIncome>, List<OtherIncomeViewModel>>(_otherIncomeBusiness.GetAllOtherIncome(Mapper.Map<OtherIncomeAdvanceSearchViewModel, OtherIncomeAdvanceSearch>(otherIncomeAdvanceSearchVM)));
            if (otherIncomeAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = otherIncomeList.Count != 0 ? otherIncomeList[0].TotalCount : 0;
                int filteredResult = otherIncomeList.Count != 0 ? otherIncomeList[0].FilteredCount : 0;
                otherIncomeList = otherIncomeList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.None
            };
            return Json(new
            {
                draw = model.draw,
                recordsTotal = otherIncomeList.Count != 0 ? otherIncomeList[0].TotalCount : 0,
                recordsFiltered = otherIncomeList.Count != 0 ? otherIncomeList[0].FilteredCount : 0,
                data = otherIncomeList
            });
        }
        #endregion GetAllOtherIncome

        #region InsertUpdateOtherIncome
        [AuthSecurityFilter(ProjectObject = "OtherIncome", Mode = "R")]
        public string InsertUpdateOtherIncome(OtherIncomeViewModel otherIncomeVM)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                otherIncomeVM.Common = new CommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _common.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _common.GetCurrentDateTime(),
                };
                object result = _otherIncomeBusiness.InsertUpdateOtherIncome(Mapper.Map<OtherIncomeViewModel,OtherIncome>(otherIncomeVM));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result, Message = _appConst.InsertSuccess });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion InsertUpdateOtherIncome

        #region GetOtherIncome
        [AuthSecurityFilter(ProjectObject = "OtherIncome", Mode = "R")]
        public string GetOtherIncome(string id)
        {
            try
            {
                OtherIncomeViewModel otherIncomeVM = Mapper.Map<OtherIncome, OtherIncomeViewModel>(_otherIncomeBusiness.GetOtherIncome(Guid.Parse(id)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = otherIncomeVM, Message = _appConst.InsertSuccess });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion GetOtherIncome

        #region DeleteOtherIncome
        [AuthSecurityFilter(ProjectObject = "OtherIncome", Mode = "R")]
        public string DeleteOtherIncome(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    throw new Exception("ID Missing");
                }
                object result = _otherIncomeBusiness.DeleteOtherIncome(Guid.Parse(id));
                return JsonConvert.SerializeObject(new { Result = "OK", Record = result, Message = _appConst.DeleteSuccess });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Record = "", Message = ex.Message });
            }
        }
        #endregion DeleteOtherIncome

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "OtherIncome", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewOtherIncome", "OtherIncome", new { code = "ACC" });
                    toolboxVM.addbtn.Event = "";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "BindOrReloadOtherIncomeTable('Reset');";

                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "BindOrReloadOtherIncomeTable('Export');";

                    break;
                case "Edit":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewOtherIncome", "OtherIncome", new { code = "ACC" });
                    toolboxVM.addbtn.Event = "";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "Reset();";

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Href = Url.Action("ViewOtherIncome", "OtherIncome", new { code = "ACC" });
                    toolboxVM.ListBtn.Event = "";

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "Save()";

                    toolboxVM.deletebtn.Visible = true;
                    toolboxVM.deletebtn.Text = "Delete";
                    toolboxVM.deletebtn.Title = "Delete";
                    toolboxVM.deletebtn.Event = "Delete()";
                    break;
                case "Add":
                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Href = Url.Action("ViewOtherIncome", "OtherIncome", new { code = "ACC" });
                    toolboxVM.ListBtn.Event = "";

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "Save()";
                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }
        #endregion ButtonStyling
    }
}