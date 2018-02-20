﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProductionApp.UserInterface.Models;
using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.UserInterface.SecurityFilter;
using Newtonsoft.Json;
using AutoMapper;


namespace ProductionApp.UserInterface.Controllers
{
    public class IssueToProductionController : Controller
    {
        // GET: IssueToProduction
        private IRawMaterialBusiness _rawMaterialBusiness;
        private IIssueToProductionBusiness _issueToProductionBusiness;
        private IEmployeeBusiness _employeeBusiness;
        Common _common = new Common();
        AppConst _appConst = new AppConst();
        //private IIssueToProductionBusiness _issueToProductionBusiness;

        public IssueToProductionController(IIssueToProductionBusiness issueToProductionBusiness,IRawMaterialBusiness rawMaterialBusiness, IEmployeeBusiness employeeBusiness)
        {
            _issueToProductionBusiness = issueToProductionBusiness;
            _rawMaterialBusiness = rawMaterialBusiness;
            _employeeBusiness = employeeBusiness;
        }

        public ActionResult Index(string code)
        {
            ViewBag.SysModuleCode = code;          
            return View();
        }
        #region AddIssueToProduction
        public ActionResult AddIssueToProduction(string code)
        {
            ViewBag.SysModuleCode = code;
            MaterialIssueViewModel materialIssueVM = new MaterialIssueViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            materialIssueVM.Employee = new EmployeeViewModel();
            materialIssueVM.Employee.SelectList = new List<SelectListItem>();
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
            materialIssueVM.Employee.SelectList = selectListItem;

            return View(materialIssueVM);
        }
        #endregion

        #region ListIssueToProduction
        public ActionResult ListIssueToProduction()
        {
            return View();
        }
        #endregion

        #region InsertUpdateIssueToProduction
        [HttpPost]
        public string InsertUpdateIssueToProduction(MaterialIssueViewModel materialIssueVM)
        {

            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                materialIssueVM.Common = new CommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _common.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _common.GetCurrentDateTime(),
                };               
                //Deserialize items
                object ResultFromJS = JsonConvert.DeserializeObject(materialIssueVM.DetailJSON);
                string ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                materialIssueVM.MaterialIssueDetailList = JsonConvert.DeserializeObject<List<MaterialIssueDetailViewModel>>(ReadableFormat);
                var result = _issueToProductionBusiness.InsertUpdateIssueToProduction(Mapper.Map<MaterialIssueViewModel, MaterialIssue>(materialIssueVM));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result });


                //selectListItem = new List<SelectListItem>();
               
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
           
        }
        #endregion

        #region GetRawMaterial
        public string GetRawMaterial(string ID)
        {
            try
            {
                RawMaterialViewModel rawMaterialVM = new RawMaterialViewModel();
                rawMaterialVM = Mapper.Map<RawMaterial, RawMaterialViewModel>(_rawMaterialBusiness.GetRawMaterial(Guid.Parse(ID)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = rawMaterialVM });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex });
            }
        }
        #endregion

        #region ButtonStyling
        [HttpGet]
        //[AuthSecurityFilter(ProjectObject = "Bank", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("AddIssueToProduction", "IssueToProduction", new { code = "STR" });
                    toolboxVM.addbtn.Event = "";
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetIssueToProductionList();";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "ImportIssueToProductionData();";
                    //---------------------------------------

                    break;
                case "Edit":

                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "New";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "openNav();";

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save Bank";
                    toolboxVM.savebtn.Event = "Save();";

                    toolboxVM.deletebtn.Visible = true;
                    toolboxVM.deletebtn.Text = "Delete";
                    toolboxVM.deletebtn.Title = "Delete Bank";
                    toolboxVM.deletebtn.Event = "Delete()";

                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset";
                    toolboxVM.resetbtn.Event = "Reset();";

                    toolboxVM.CloseBtn.Visible = true;
                    toolboxVM.CloseBtn.Text = "Close";
                    toolboxVM.CloseBtn.Title = "Close";
                    toolboxVM.CloseBtn.Event = "closeNav();";

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
                    toolboxVM.ListBtn.Href = Url.Action("ViewPurchaseOrder", "PurchaseOrder", new { Code = "PURCH" });


                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion
    }
}