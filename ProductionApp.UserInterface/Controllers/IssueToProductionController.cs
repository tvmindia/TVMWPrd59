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
        private IMaterialBusiness _rawMaterialBusiness;
        private IIssueToProductionBusiness _issueToProductionBusiness;
        private IEmployeeBusiness _employeeBusiness;
        Common _common = new Common();
        AppConst _appConst = new AppConst();
        //private IIssueToProductionBusiness _issueToProductionBusiness;

        public IssueToProductionController(IIssueToProductionBusiness issueToProductionBusiness,IMaterialBusiness rawMaterialBusiness, IEmployeeBusiness employeeBusiness)
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
        public ActionResult AddIssueToProduction(string code,Guid? id)
        {
            ViewBag.SysModuleCode = code;
            MaterialIssueViewModel materialIssueVM = new MaterialIssueViewModel();
            materialIssueVM.ID = id == null ? Guid.Empty : (Guid)id;
            materialIssueVM.IsUpdate = id == null ? false : true;          
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
            materialIssueVM.MaterialIssueDetail = new MaterialIssueDetailViewModel();
            materialIssueVM.MaterialIssueDetail.MaterialIssue = new MaterialIssueViewModel();
            return View(materialIssueVM);
        }
        #endregion

        #region ListIssueToProduction
        public ActionResult ListIssueToProduction(string code)
        {
            ViewBag.SysModuleCode = code;
            MaterialIssueAdvanceSearchViewModel materialIssueAdvanceSearchVM = new MaterialIssueAdvanceSearchViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            materialIssueAdvanceSearchVM.Employee = new EmployeeViewModel();
            materialIssueAdvanceSearchVM.Employee.SelectList = new List<SelectListItem>();
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
            materialIssueAdvanceSearchVM.Employee.SelectList = selectListItem;      
            return View(materialIssueAdvanceSearchVM);
        }
        #endregion

        #region GetAllIssueToProduction
        public JsonResult GetAllIssueToProduction(DataTableAjaxPostModel model, MaterialIssueAdvanceSearchViewModel materialIssueAdvanceSearchVM)
        {
            materialIssueAdvanceSearchVM.DataTablePaging.Start = model.start;
            materialIssueAdvanceSearchVM.DataTablePaging.Length = (materialIssueAdvanceSearchVM.DataTablePaging.Length == 0 ? model.length : materialIssueAdvanceSearchVM.DataTablePaging.Length);
            List<MaterialIssueViewModel> materialIssueOrderList = Mapper.Map<List<MaterialIssue>, List<MaterialIssueViewModel>>(_issueToProductionBusiness.GetAllIssueToProduction(Mapper.Map<MaterialIssueAdvanceSearchViewModel, MaterialIssueAdvanceSearch>(materialIssueAdvanceSearchVM)));


            return Json(new
            {
                draw = model.draw,
                recordsTotal = materialIssueOrderList.Count != 0 ? materialIssueOrderList[0].TotalCount : 0,
                recordsFiltered = materialIssueOrderList.Count != 0 ? materialIssueOrderList[0].FilteredCount : 0,
                data = materialIssueOrderList
            });
        }

        #endregion

        #region InsertUpdateIssueToProduction
        [HttpPost]
        [AuthSecurityFilter(ProjectObject ="IssueToProduction",Mode ="R")]
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

        #region GetMaterial
        public string GetMaterial(string ID)
        {
            try
            {
                MaterialViewModel rawMaterialVM = new MaterialViewModel();
                rawMaterialVM = Mapper.Map<Material, MaterialViewModel>(_rawMaterialBusiness.GetMaterial(Guid.Parse(ID)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = rawMaterialVM });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex });
            }
        }
        #endregion

        #region GetIssueToProduction
        public string GetIssueToProduction(string ID)
        {
            try
            {
                MaterialIssueViewModel materialIssueVM = new MaterialIssueViewModel();
                materialIssueVM = Mapper.Map<MaterialIssue, MaterialIssueViewModel>(_issueToProductionBusiness.GetIssueToProduction(Guid.Parse(ID)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = materialIssueVM });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex });
            }
        }
        #endregion

        #region GetIssueToProductionDetail
        public string GetIssueToProductionDetail(string ID)
        {
            try
            {
                List<MaterialIssueDetailViewModel> materialIssueDetailVM = new List<MaterialIssueDetailViewModel>();
                materialIssueDetailVM = Mapper.Map<List<MaterialIssueDetail>, List<MaterialIssueDetailViewModel>>(_issueToProductionBusiness.GetIssueToProductionDetail(Guid.Parse(ID)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = materialIssueDetailVM });
            }
            catch(Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex });
            }
        }
        #endregion

        #region DeleteIssueToProductionDetail
        public string DeleteIssueToProductionDetail(string ID)
        {
            object result = null;
            try
            {
                if(string.IsNullOrEmpty(ID))
                {
                    throw new Exception("ID Missing");
                }
                result = _issueToProductionBusiness.DeleteIssueToProductionDetail(Guid.Parse(ID));
                return JsonConvert.SerializeObject(new { Result = "OK", Record = result, Message = _appConst.DeleteSuccess });
            }
            catch(Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion

        #region DeleteIssueToProduction
        public string DeleteIssueToProduction(string ID)
        {
            object result = null;
            try
            {
                if (string.IsNullOrEmpty(ID))
                {
                    throw new Exception("ID Missing");
                }
                result = _issueToProductionBusiness.DeleteIssueToProduction(Guid.Parse(ID));
                return JsonConvert.SerializeObject(new { Result = "OK", Record = result, Message = _appConst.DeleteSuccess });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "IssueToProduction", Mode = "")]
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
                    toolboxVM.addbtn.Href =Url.Action("AddIssueToProduction","IssueToProduction",new { code="STR"});

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

                    //toolboxVM.CloseBtn.Visible = true;
                    //toolboxVM.CloseBtn.Text = "Close";
                    //toolboxVM.CloseBtn.Title = "Close";
                    //toolboxVM.CloseBtn.Event = "closeNav();";

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
                    toolboxVM.ListBtn.Href = Url.Action("ListIssueToProduction", "IssueToProduction", new { Code = "STR" });


                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion
    }
}