using AutoMapper;
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
    public class SalesOrderController : Controller
    {
        // GET: SalesOrder
        private ISalesOrderBusiness _salesOrderBusiness;

        Common _common = new Common();
        AppConst _appConst = new AppConst();
        public SalesOrderController(ISalesOrderBusiness salesOrderBusiness)
        {
            _salesOrderBusiness = salesOrderBusiness;
        }
        public ActionResult AddSalesOrder(string code, Guid? id)
        {
            ViewBag.SysModuleCode = code;

            return View();
        }
        public ActionResult ListSalesOrder(string code)
        {
            ViewBag.SysModuleCode = code;

            return View();
        }

        #region GetAllSalesOrder
        [AuthSecurityFilter(ProjectObject = "SalesOrder", Mode = "R")]
        public JsonResult GetAllSalesOrder(DataTableAjaxPostModel model,SalesOrderAdvanceSearchViewModel SalesOrderAdvanceSearchVM)
        {
            SalesOrderAdvanceSearchVM.DataTablePaging.Start = model.start;
            SalesOrderAdvanceSearchVM.DataTablePaging.Length = (SalesOrderAdvanceSearchVM.DataTablePaging.Length == 0 ? model.length : SalesOrderAdvanceSearchVM.DataTablePaging.Length);
            List<SalesOrderViewModel> salesOrderList =Mapper.Map<List<SalesOrder>, List<SalesOrderViewModel>>(_salesOrderBusiness.GetAllSalesOrder(Mapper.Map<SalesOrderAdvanceSearchViewModel, SalesOrderAdvanceSearch>(SalesOrderAdvanceSearchVM)));
            return Json(new
            {
                draw = model.draw,
                recordsTotal =0,// salesOrderList.Count != 0 ? salesOrderList[0].TotalCount : 0,
            recordsFiltered = 0,//salesOrderList.Count != 0 ? salesOrderList[0].FilteredCount : 0,
                data = salesOrderList
            });
        }
        #endregion GetAllSalesOrder


        #region GetAllSalesOrderDetail
        [AuthSecurityFilter(ProjectObject = "SalesOrder", Mode = "R")]
        public JsonResult GetAllSalesOrderDetail(DataTableAjaxPostModel model, SalesOrderAdvanceSearchViewModel SalesOrderAdvanceSearchVM)
        {
            SalesOrderAdvanceSearchVM.DataTablePaging.Start = model.start;
            SalesOrderAdvanceSearchVM.DataTablePaging.Length = (SalesOrderAdvanceSearchVM.DataTablePaging.Length == 0 ? model.length : SalesOrderAdvanceSearchVM.DataTablePaging.Length);
            List<SalesOrderViewModel> salesOrderList = Mapper.Map<List<SalesOrder>, List<SalesOrderViewModel>>(_salesOrderBusiness.GetAllSalesOrder(Mapper.Map<SalesOrderAdvanceSearchViewModel, SalesOrderAdvanceSearch>(SalesOrderAdvanceSearchVM)));
            return Json(new
            {
                draw = model.draw,
                recordsTotal = 0,// salesOrderList.Count != 0 ? salesOrderList[0].TotalCount : 0,
                recordsFiltered = 0,//salesOrderList.Count != 0 ? salesOrderList[0].FilteredCount : 0,
                data = salesOrderList
            });
        }
        #endregion GetAllSalesOrderDetail

        #region ButtonStyling
        [HttpGet]
   //     [AuthSecurityFilter(ProjectObject = "SalesOrder", Mode = "")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("AddSalesOrder", "SalesOrder", new { Code = "PURCH" });
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetSalesOrderList();";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "ImportSalesOrderData();";
                    //---------------------------------------

                    break;
                case "Edit":

                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "New";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("AddSalesOrder", "SalesOrder", new { Code = "PURCH" });

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

                    //toolboxVM.SendForApprovalBtn.Visible = true;
                    //toolboxVM.SendForApprovalBtn.Text = "Send";
                    //toolboxVM.SendForApprovalBtn.Title = "Send For Approval";
                    //toolboxVM.SendForApprovalBtn.Event = "ShowSendForApproval('REQ');";

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Href = Url.Action("ListSalesOrder", "SalesOrder", new { Code = "PURCH" });

                    break;

                //case "Disable":

                //    toolboxVM.addbtn.Visible = true;
                //    toolboxVM.addbtn.Text = "New";
                //    toolboxVM.addbtn.Title = "Add New";
                //    toolboxVM.addbtn.Href = Url.Action("AddSalesOrder", "SalesOrder", new { Code = "PURCH" });

                //    toolboxVM.ListBtn.Visible = true;
                //    toolboxVM.ListBtn.Text = "List";
                //    toolboxVM.ListBtn.Title = "List";
                //    toolboxVM.ListBtn.Href = Url.Action("ListSalesOrder", "SalesOrder", new { Code = "PURCH" });

                //    break;
                case "Add":

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "Save();";

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Href = Url.Action("ViewRequisition", "Requisition", new { Code = "PURCH" });
                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion
    }
}