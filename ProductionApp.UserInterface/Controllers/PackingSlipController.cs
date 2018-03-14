﻿using AutoMapper;
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
    public class PackingSlipController : Controller
    {
        // GET: PackingSlip
        private IPackingSlipBusiness _packingSlipBusiness;
        private IEmployeeBusiness _employeeBusiness;
        private ISalesOrderBusiness _salesOrderBusiness;
        Common _common = new Common();
        AppConst _appConst = new AppConst();
        public PackingSlipController(IEmployeeBusiness employeeBusiness, IPackingSlipBusiness packingSlipBusiness, ISalesOrderBusiness salesOrderBusiness)
        {
            _employeeBusiness = employeeBusiness;
            _packingSlipBusiness = packingSlipBusiness;
            _salesOrderBusiness = salesOrderBusiness;
        }

        #region AddPackingSlip
        public ActionResult AddPackingSlip(string code, Guid? id)
        {
            ViewBag.SysModuleCode = code;
            PackingSlipViewModel PackingSlipVM = new PackingSlipViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            PackingSlipVM.Employee = new EmployeeViewModel();
            PackingSlipVM.Employee.SelectList = new List<SelectListItem>();
            List<EmployeeViewModel> employeeList = Mapper.Map<List<Employee>, List<EmployeeViewModel>>(_employeeBusiness.GetEmployeeForSelectList());
            if (employeeList != null)
            {
                foreach (EmployeeViewModel Emp in employeeList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = Emp.Name,
                        Value = Emp.ID.ToString(),
                        Selected = false,
                    });

                }
            }
            PackingSlipVM.Employee.SelectList = selectListItem;
            selectListItem = new List<SelectListItem>();
            PackingSlipVM.SalesOrder = new SalesOrderViewModel();
            PackingSlipVM.SalesOrder.SelectList = new List<SelectListItem>();
            List< SalesOrderViewModel> salesOrderList= Mapper.Map<List<SalesOrder>, List<SalesOrderViewModel>>(_salesOrderBusiness.GetAllSalesOrderForSelectList());
            if (salesOrderList != null)
            {
                foreach (SalesOrderViewModel Emp in salesOrderList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = Emp.OrderNo,
                        Value = Emp.ID.ToString(),
                        Selected = false,
                    });

                }
            }
            PackingSlipVM.SalesOrder.SelectList = selectListItem;
            return View(PackingSlipVM);
        }
        #endregion AddPackingSlip

        #region ListPackingSlips
        public ActionResult ListPackingSlips(string code)
        {
            ViewBag.SysModuleCode = code;
            PackingSlipAdvanceSearchViewModel PackingSlipAdvanceSearchVM = new PackingSlipAdvanceSearchViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            PackingSlipAdvanceSearchVM.Employee = new EmployeeViewModel();
            PackingSlipAdvanceSearchVM.Employee.SelectList = new List<SelectListItem>();
            List<EmployeeViewModel> employeeList = Mapper.Map<List<Employee>, List<EmployeeViewModel>>(_employeeBusiness.GetEmployeeForSelectList());
            if (employeeList != null)
            {
                foreach (EmployeeViewModel Emp in employeeList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = Emp.Name,
                        Value = Emp.ID.ToString(),
                        Selected = false,
                    });

                }
            }
            PackingSlipAdvanceSearchVM.Employee.SelectList = selectListItem;
            return View(PackingSlipAdvanceSearchVM);
        }
        #endregion ListPackingSlips

        #region GetAllPaySlip
        [HttpPost]
        
        public JsonResult GetAllPackingSlip(DataTableAjaxPostModel model, PackingSlipAdvanceSearchViewModel packingSlipAdvanceSearchVM)
        {
            packingSlipAdvanceSearchVM.DataTablePaging.Start = model.start;
            packingSlipAdvanceSearchVM.DataTablePaging.Length = (packingSlipAdvanceSearchVM.DataTablePaging.Length == 0 ? model.length : packingSlipAdvanceSearchVM.DataTablePaging.Length);
            List<PackingSlipViewModel> packingSlipList = Mapper.Map<List<PackingSlip>, List<PackingSlipViewModel>>(_packingSlipBusiness.GetAllPackingSlip(Mapper.Map<PackingSlipAdvanceSearchViewModel, PackingSlipAdvanceSearch>(packingSlipAdvanceSearchVM)));
            if (packingSlipAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = packingSlipList.Count != 0 ? packingSlipList[0].TotalCount : 0;
                int filteredResult = packingSlipList.Count != 0 ? packingSlipList[0].FilteredCount : 0;
                packingSlipList = packingSlipList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }

            return Json(new
            {
                draw = model.draw,
                recordsTotal = packingSlipList.Count != 0 ? packingSlipList[0].TotalCount : 0,
                recordsFiltered = packingSlipList.Count != 0 ? packingSlipList[0].FilteredCount : 0,
                data = packingSlipList
            });
        }
        #endregion GetAllPaySlip

        #region ButtonStyling
        [HttpGet]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("AddPackingSlip", "PackingSlip", new { code = "SALE" });
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetPackingSlipList();";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "ImportPackingSlipData();";
                    //---------------------------------------

                    break;
                case "Edit":

                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "New";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("AddPackingSlip", "PackingSlip", new { code = "SALE" });

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

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Event = "";
                    toolboxVM.ListBtn.Href = Url.Action("ListPackingSlips", "PackingSlip", new { Code = "SALE" });


                    break;
                case "Add":

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "Save();";

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Event = "";
                    toolboxVM.ListBtn.Href = Url.Action("ListPackingSlips", "PackingSlip", new { Code = "SALE" });


                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion

    }
}