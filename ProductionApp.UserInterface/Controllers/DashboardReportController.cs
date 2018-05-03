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
    public class DashboardReportController : Controller
    {
        #region Constructor Injection  
        AppConst _appConst = new AppConst();
        IReportBusiness _reportBusiness;
        IEmployeeBusiness _employeeBusiness;
        IRequisitionBusiness _requisitionBusiness;
        ICommonBusiness _commonBusiness;

        public DashboardReportController(IReportBusiness reportBusiness,IEmployeeBusiness employeeBusiness,IRequisitionBusiness requisitionBusiness,ICommonBusiness commonBusiness)
        {
            _reportBusiness = reportBusiness;
            _employeeBusiness = employeeBusiness;
            _requisitionBusiness = requisitionBusiness;
            _commonBusiness = commonBusiness;
        }
        #endregion Constructor Injection

        // GET: Inventory
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "DashboardReport", Mode = "R")]
        public ActionResult Index(string Code, string searchTerm)
        {
            ViewBag.SysModuleCode = Code;
            AMCSysReportViewModel AMCSysReport = new AMCSysReportViewModel();            
            AMCSysReport.AMCSysReportList= Mapper.Map<List<AMCSysReport>, List<AMCSysReportViewModel>>(_reportBusiness.GetAllReport(searchTerm));
            AMCSysReport.AMCSysReportList = AMCSysReport.AMCSysReportList != null ? AMCSysReport.AMCSysReportList.OrderBy(s => s.GroupOrder).ToList() : null;
            return View(AMCSysReport);
        }

        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "DashboardReport", Mode = "R")]
        public ActionResult RequisitionSummaryReport(string Code)
        {
            ViewBag.SysModuleCode = Code;
            
            RequisitionSummaryReportViewModel requisitionSummaryReportVM = new RequisitionSummaryReportViewModel();           
            requisitionSummaryReportVM.Employee = new EmployeeViewModel();
            requisitionSummaryReportVM.Employee.SelectList = _employeeBusiness.GetEmployeeSelectList();
            return View(requisitionSummaryReportVM);
        }

        #region GetRequisitionSummaryReport
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "DashboardReport", Mode = "R")]
        public JsonResult GetRequisitionSummaryReport(DataTableAjaxPostModel model, RequisitionSummaryReportViewModel requisitionSummaryVM)
        {  
            Common con = new Common();
            DateTime dt = con.GetCurrentDateTime();
            if (requisitionSummaryVM != null)
            {
                if (requisitionSummaryVM.DateFilter == "30")
                {
                    requisitionSummaryVM.FromDate = dt.AddDays(-30).ToString("dd-MMM-yyyy");
                    requisitionSummaryVM.ToDate = dt.ToString("dd-MMM-yyyy");
                }
                if (requisitionSummaryVM.DateFilter == "60")
                {
                    requisitionSummaryVM.FromDate = dt.AddDays(-60).ToString("dd-MMM-yyyy");
                    requisitionSummaryVM.ToDate = dt.ToString("dd-MMM-yyyy");
                }
                if (requisitionSummaryVM.DateFilter == "90")
                {
                    requisitionSummaryVM.FromDate = dt.AddDays(-90).ToString("dd-MMM-yyyy");
                    requisitionSummaryVM.ToDate = dt.ToString("dd-MMM-yyyy");
                }
            }
            requisitionSummaryVM.DataTablePaging.Start = model.start;
            requisitionSummaryVM.DataTablePaging.Length = (requisitionSummaryVM.DataTablePaging.Length == 0 ? model.length : requisitionSummaryVM.DataTablePaging.Length);

            List<RequisitionViewModel> requisitionOrderList = Mapper.Map<List<Requisition>, List<RequisitionViewModel>>(_reportBusiness.GetRequisitionSummaryReport(Mapper.Map<RequisitionSummaryReportViewModel, RequisitionSummaryReport>(requisitionSummaryVM)));

            if (requisitionSummaryVM.DataTablePaging.Length == -1)
            {
                int totalResult = requisitionOrderList.Count != 0 ? requisitionOrderList[0].TotalCount : 0;
                int filteredResult = requisitionOrderList.Count != 0 ? requisitionOrderList[0].FilteredCount : 0;
                requisitionOrderList = requisitionOrderList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }
            var settings = new JsonSerializerSettings
            {
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
            };

            return Json(new
            {
                draw = model.draw,
                recordsTotal = requisitionOrderList.Count != 0 ? requisitionOrderList[0].TotalCount : 0,
                recordsFiltered = requisitionOrderList.Count != 0 ? requisitionOrderList[0].FilteredCount : 0,
                data = requisitionOrderList
            });
        }
        #endregion GetRequisitionSummaryReport


        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "dashboardReport", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":                   
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetReportList();";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "ExportReportData();";
                    //---------------------------------------
                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Href = Url.Action("Index", "DashboardReport", new { Code = "RPT" });
                    break;                
               
              
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion

    }
}