using AutoMapper;
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
        public ActionResult MaterialTypeDropdown(MaterialTypeViewModel materialTypeVM)
        {
            materialTypeVM.MaterialTypeCode = materialTypeVM.Code;
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            materialTypeVM.SelectList = new List<SelectListItem>();
            List<MaterialTypeViewModel> materialTypeList = Mapper.Map<List<MaterialType>, List<MaterialTypeViewModel>>(_materialTypeBusiness.GetMaterialTypeForSelectList());
            if (materialTypeList != null)
                foreach (MaterialTypeViewModel unit in materialTypeList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = unit.Description,
                        Value = unit.Code.ToString(),
                        Selected = false
                    });
                }
            materialTypeVM.SelectList = selectListItem;
            return PartialView("_MaterialTypeDropdown", materialTypeVM);
        }
        #endregion MaterialTypeDropdown

    }
}