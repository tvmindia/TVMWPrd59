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
    public class MaterialTypeController : Controller
    {
        AppConst _appConst = new AppConst();
        private Common _common = new Common();
        private IMaterialTypeBusiness _materialTypeBusiness;

        #region Constructor Injection
        public MaterialTypeController(IMaterialTypeBusiness materialTypeBusiness)
        {
            _materialTypeBusiness = materialTypeBusiness;
        }
        #endregion Constructor Injection
        // GET: MaterialType
        public ActionResult Index(string code)
        {
            ViewBag.SysModuleCode = code;
            return View();
        }
        #region MasterPartial
        [HttpGet]
        public ActionResult MasterPartial(string masterCode)
        {
            MaterialTypeViewModel materTypeVM = string.IsNullOrEmpty(masterCode) ? new MaterialTypeViewModel() : Mapper.Map<MaterialType, MaterialTypeViewModel>(_materialTypeBusiness.GetMaterialType(masterCode));
            materTypeVM.IsUpdate = string.IsNullOrEmpty(masterCode) ? false : true;
            return PartialView("_AddMaterialTypePartial", materTypeVM);
        }
        #endregion MasterPartial

        #region MaterialTypeDropdown
        public ActionResult MaterialTypeSelectList(string required)
        {
            ViewBag.IsRequired = required;
            MaterialTypeViewModel materialTypeVM = new MaterialTypeViewModel();
            materialTypeVM.MaterialTypeSelectList = _materialTypeBusiness.GetMaterialTypeForSelectList();
            return PartialView("_MaterialTypeDropdown", materialTypeVM);
        }
        #endregion MaterialTypeDropdown

        #region InsertUpdateMaterialType
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "MaterialType", Mode = "R")]
        public string InsertUpdateMaterialType(MaterialTypeViewModel materialTypeVM)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                materialTypeVM.Common = new CommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _common.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _common.GetCurrentDateTime(),
                };
                var result = _materialTypeBusiness.InsertUpdateMaterialType(Mapper.Map<MaterialTypeViewModel, MaterialType>(materialTypeVM));
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion InsertUpdateMaterialType

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "MaterialType", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddMaterialTypeMaster('MSTR')";
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetMaterialTypeList();";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "ImportMaterialTypeData();";
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