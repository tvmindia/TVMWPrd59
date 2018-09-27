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
    public class ChartOfAccountController : Controller
    {
        AppConst _appConst = new AppConst();
        private Common _common = new Common();
        private IChartOfAccountBusiness _chartOfAccountBusiness;
        //private IUnitBusiness _unitBusiness;
        //private IProductCategoryBusiness _productCategoryBusiness;

        #region Constructor Injection
        public ChartOfAccountController(IChartOfAccountBusiness chartOfAccountBusiness)//, IUnitBusiness unitBusiness, IProductCategoryBusiness productCategoryBusiness)
        {
            _chartOfAccountBusiness = chartOfAccountBusiness;
            //_unitBusiness = unitBusiness;
            //_productCategoryBusiness = productCategoryBusiness;
        }
        #endregion Constructor Injection

        #region Index
        [AuthSecurityFilter(ProjectObject = "ChartOfAccount", Mode = "R")]
        public ActionResult Index(string code)
        {
            ViewBag.SysModuleCode = code;
            ChartOfAccountAdvanceSearchViewModel chartOfAccountAdvanceSearchVM = new ChartOfAccountAdvanceSearchViewModel();
            //chartOfAccountAdvanceSearchVM.Unit = new UnitViewModel();
            //chartOfAccountAdvanceSearchVM.Unit.UnitSelectList = _unitBusiness.GetUnitForSelectList();
            return View(chartOfAccountAdvanceSearchVM);
        }
        #endregion Index

        #region GetAllChartOfAccount
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "ChartOfAccount", Mode = "R")]
        public JsonResult GetAllChartOfAccount(DataTableAjaxPostModel model, ChartOfAccountAdvanceSearchViewModel chartOfAccountAdvanceSearchVM)
        {
            try
            {
                //setting options to our model
                chartOfAccountAdvanceSearchVM.DataTablePaging.Start = model.start;
                chartOfAccountAdvanceSearchVM.DataTablePaging.Length = (chartOfAccountAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : chartOfAccountAdvanceSearchVM.DataTablePaging.Length;

                // action inside a standard controller
                List<ChartOfAccountViewModel> chartOfAccountVMList = Mapper.Map<List<ChartOfAccount>, List<ChartOfAccountViewModel>>(_chartOfAccountBusiness.GetAllChartOfAccount(Mapper.Map<ChartOfAccountAdvanceSearchViewModel, ChartOfAccountAdvanceSearch>(chartOfAccountAdvanceSearchVM)));
                if (chartOfAccountAdvanceSearchVM.DataTablePaging.Length == -1)
                {
                    int totalResult = chartOfAccountVMList.Count != 0 ? chartOfAccountVMList[0].TotalCount : 0;
                    int filteredResult = chartOfAccountVMList.Count != 0 ? chartOfAccountVMList[0].FilteredCount : 0;
                    chartOfAccountVMList = chartOfAccountVMList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
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
                    recordsTotal = chartOfAccountVMList.Count != 0 ? chartOfAccountVMList[0].TotalCount : 0,
                    recordsFiltered = chartOfAccountVMList.Count != 0 ? chartOfAccountVMList[0].FilteredCount : 0,
                    data = chartOfAccountVMList
                });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return Json(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion GetAllChartOfAccount

        //#region CheckChartOfAccountTypeExist
        //public ActionResult CheckChartOfAccountTypeExist(ChartOfAccountViewModel chartOfAccountVM)
        //{
        //    bool exists = _chartOfAccountBusiness.CheckChartOfAccountTypeExist(Mapper.Map<ChartOfAccountViewModel, ChartOfAccount>(chartOfAccountVM));
        //    if (exists)
        //    {
        //        return Json("<p><span style='vertical-align: 2px'>Product code is in use </span> <i class='fa fa-close' style='font-size:19px; color: red'></i></p>", JsonRequestBehavior.AllowGet);
        //    }
        //    //var result = new { success = true, message = "Success" };
        //    return Json(true, JsonRequestBehavior.AllowGet);
        //}
        //#endregion CheckChartOfAccountTypeExist

        #region InsertUpdateChartOfAccount
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "ChartOfAccount", Mode = "W")]
        public string InsertUpdateChartOfAccount(ChartOfAccountViewModel chartOfAccountVM)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                chartOfAccountVM.Common = new CommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _common.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _common.GetCurrentDateTime(),
                };
                var result = _chartOfAccountBusiness.InsertUpdateChartOfAccount(Mapper.Map<ChartOfAccountViewModel, ChartOfAccount>(chartOfAccountVM));
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion InsertUpdateChartOfAccount

        #region MasterPartial
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ChartOfAccount", Mode = "R")]
        public ActionResult MasterPartial(string masterCode)
        {
            ChartOfAccountViewModel chartOfAccountVM = string.IsNullOrEmpty(masterCode) ? new ChartOfAccountViewModel() : Mapper.Map<ChartOfAccount, ChartOfAccountViewModel>(_chartOfAccountBusiness.GetChartOfAccount(masterCode));
            chartOfAccountVM.IsUpdate = string.IsNullOrEmpty(masterCode) ? false : true;
            //productVM.ChartOfAccountCategory = new ChartOfAccountViewModel();
            //productVM.ChartOfAccountCategory.ProductCategorySelectList = _chartOfAccountBusiness.GetChartOfAccountForSelectList();
           // productVM.Unit = new UnitViewModel();
           // productVM.Unit.UnitSelectList = _unitBusiness.GetUnitForSelectList();
            return PartialView("_AddChartOfAccountPartial", chartOfAccountVM);
        }
        #endregion MasterPartial

       

        #region ChartOfAccountDropdown
        public ActionResult ChartOfAccountDropdown(string required,string type)
        {
            ViewBag.IsRequired = required;
            ChartOfAccountViewModel chartOfAccountVM = new ChartOfAccountViewModel()
            {
                ChartOfAccountSelectList = _chartOfAccountBusiness.GetChartOfAccountForSelectList(type)
            };
            return PartialView("_ChartOfAccountDropdown", chartOfAccountVM);
        }
        #endregion ChartOfAccountDropdown

        #region DeleteChartOfAccount
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ChartOfAccount", Mode = "D")]
        public string DeleteChartOfAccount(string code)
        {
            try
            {
                var result = _chartOfAccountBusiness.DeleteChartOfAccount(code);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }

        }
        #endregion DeleteChartOfAccount

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ChartOfAccount", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddChartOfAccountMaster('MSTR')";
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetChartOfAccountList();";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "ImportChartOfAccountData();";
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