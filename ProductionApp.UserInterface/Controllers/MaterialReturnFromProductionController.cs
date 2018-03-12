using AutoMapper;
using iTextSharp.text.pdf.qrcode;
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
    public class MaterialReturnFromProductionController : Controller
    {
        // GET: MaterialReturnFromProduction
        IMaterialReturnFromProductionBusiness _materialReturnFromProductionBusiness;
        private IMaterialBusiness _rawMaterialBusiness;
        private IEmployeeBusiness _employeeBusiness;
        Common _common = new Common();
        AppConst _appConst = new AppConst();
        public MaterialReturnFromProductionController(IMaterialReturnFromProductionBusiness materialReturnFromProductionBusiness,IEmployeeBusiness employeeBusiness, IMaterialBusiness rawMaterialBusiness)
        {
            _materialReturnFromProductionBusiness = materialReturnFromProductionBusiness;
            _employeeBusiness = employeeBusiness;
            _rawMaterialBusiness = rawMaterialBusiness;
        }

        #region NewReceiveFromProduction
        [AuthSecurityFilter(ProjectObject = "MaterialReturnFromProduction", Mode = "R")]
        public ActionResult NewRecieveFromProduction(string code, Guid? id)
        {
            ViewBag.SysModuleCode = code;
            MaterialReturnFromProductionViewModel materiaReturnVM = new MaterialReturnFromProductionViewModel();
            materiaReturnVM.ID = id == null ? Guid.Empty : (Guid)id;
            materiaReturnVM.IsUpdate = id == null ? false : true;
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            materiaReturnVM.Employee = new EmployeeViewModel();
            materiaReturnVM.Employee.SelectList = new List<SelectListItem>();
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
            materiaReturnVM.Employee.SelectList = selectListItem;
            materiaReturnVM.MaterialReturnFromProductionDetail = new MaterialReturnFromProductionDetailViewModel();
            return View(materiaReturnVM);
        }
        #endregion NewReceiveFromProduction

        #region ViewReceiveFromProduction
        [AuthSecurityFilter(ProjectObject = "MaterialReturnFromProduction", Mode = "R")]
        public ActionResult ViewRecieveFromProduction(string code)
        {
            ViewBag.SysModuleCode = code;
            MaterialReturnFromProductionAdvanceSearchViewModel materialReturnAdvanceSearchVM = new MaterialReturnFromProductionAdvanceSearchViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            materialReturnAdvanceSearchVM.Employee = new EmployeeViewModel();
            materialReturnAdvanceSearchVM.Employee.SelectList = new List<SelectListItem>();
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
            materialReturnAdvanceSearchVM.Employee.SelectList = selectListItem;
            return View(materialReturnAdvanceSearchVM);
        }
        #endregion ViewReceiveFromProduction

        #region GetAllReturnFromProduction
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "MaterialReturnFromProduction", Mode = "R")]
        public JsonResult GetAllReturnFromProduction(DataTableAjaxPostModel model, MaterialReturnFromProductionAdvanceSearchViewModel materialReturnAdvanceSearchVM)
        {
            materialReturnAdvanceSearchVM.DataTablePaging.Start = model.start;
            materialReturnAdvanceSearchVM.DataTablePaging.Length = (materialReturnAdvanceSearchVM.DataTablePaging.Length == 0 ? model.length : materialReturnAdvanceSearchVM.DataTablePaging.Length);
            List<MaterialReturnFromProductionViewModel> materialReturnList = Mapper.Map<List<MaterialReturnFromProduction>, List<MaterialReturnFromProductionViewModel>>(_materialReturnFromProductionBusiness.GetAllReturnFromProduction(Mapper.Map<MaterialReturnFromProductionAdvanceSearchViewModel, MaterialReturnFromProductionAdvanceSearch>(materialReturnAdvanceSearchVM)));
            if (materialReturnAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = materialReturnList.Count != 0 ? materialReturnList[0].TotalCount : 0;
                int filteredResult = materialReturnList.Count != 0 ? materialReturnList[0].FilteredCount : 0;
                materialReturnList = materialReturnList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }

            return Json(new
            {
                draw = model.draw,
                recordsTotal = materialReturnList.Count != 0 ? materialReturnList[0].TotalCount : 0,
                recordsFiltered = materialReturnList.Count != 0 ? materialReturnList[0].FilteredCount : 0,
                data = materialReturnList
            });
        }
        #endregion GetAllReturnFromProduction

        #region InsertUpdateReturnFromProduction
        [AuthSecurityFilter(ProjectObject = "MaterialReturnFromProduction", Mode = "R")]
        public string InsertUpdateReturnFromProduction(MaterialReturnFromProductionViewModel materialReturnVM)
        {

            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                materialReturnVM.Common = new CommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _common.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _common.GetCurrentDateTime(),
                };
                //Deserialize items
                object ResultFromJS = JsonConvert.DeserializeObject(materialReturnVM.DetailJSON);
                string ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                materialReturnVM.MaterialReturnFromProductionDetailList = JsonConvert.DeserializeObject<List<MaterialReturnFromProductionDetailViewModel>>(ReadableFormat);
                var result = _materialReturnFromProductionBusiness.InsertUpdateReturnFromProduction(Mapper.Map<MaterialReturnFromProductionViewModel, MaterialReturnFromProduction>(materialReturnVM));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result, Message = "Success" });


                //selectListItem = new List<SelectListItem>();

            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = cm.Message });
            }

        }
        #endregion InsertUpdateReturnFromProduction

        #region GetMaterial
        public string GetMaterial(string id)
        {
            try
            {
                MaterialViewModel rawMaterialVM = new MaterialViewModel();
                rawMaterialVM = Mapper.Map<Material, MaterialViewModel>(_rawMaterialBusiness.GetMaterial(Guid.Parse(id)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = rawMaterialVM, Message = "Sucess" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = ex });
            }
        }
        #endregion

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "MaterialReturnFromProduction", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewRecieveFromProduction", "MaterialReturnFromProduction", new { code = "STR" });
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetReturnFromProductionList();";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "ImportReturnFromProductionData();";
                    //---------------------------------------

                    break;
                case "Edit":

                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "New";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewRecieveFromProduction", "MaterialReturnFromProduction", new { code = "STR" });

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
                    toolboxVM.ListBtn.Href = Url.Action("ViewRecieveFromProduction", "MaterialReturnFromProduction", new { Code = "STR" });


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
                    toolboxVM.ListBtn.Href = Url.Action("ViewRecieveFromProduction", "MaterialReturnFromProduction", new { Code = "STR" });


                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion

    }
}