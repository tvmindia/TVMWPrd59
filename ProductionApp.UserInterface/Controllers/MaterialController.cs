using AutoMapper;
using Newtonsoft.Json;
using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.UserInterface.Models;
using ProductionApp.UserInterface.SecurityFilter;
using SAMTool.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Controllers
{
    public class MaterialController : Controller
    {
        AppConst _appConst = new AppConst();
        private DataAccessObject.DTO.Common _common = new DataAccessObject.DTO.Common();
        private IMaterialBusiness _materialBusiness;
        private IMaterialTypeBusiness _materialTypeBusiness;
        private IUnitBusiness _unitBusiness;

        #region Constructor Injection
        public MaterialController(IMaterialBusiness materialBusiness, IMaterialTypeBusiness materialTypeBusiness, IUnitBusiness unitBusiness)
        {
            _materialBusiness = materialBusiness;
            _materialTypeBusiness = materialTypeBusiness;
            _unitBusiness = unitBusiness;
        }
        #endregion Constructor Injection

        #region Index
        // GET: RawMaterial
        [AuthSecurityFilter(ProjectObject = "Material", Mode = "R")]
        public ActionResult Index(string code, string masterCode)
        {
            ViewBag.SysModuleCode = code;
            MaterialAdvanceSearchViewModel materialAdvanceSearchVM = new MaterialAdvanceSearchViewModel();
            if(!string.IsNullOrEmpty(masterCode))
                materialAdvanceSearchVM.ID =masterCode;
            materialAdvanceSearchVM.MaterialType = new MaterialTypeViewModel()
            {
                MaterialTypeSelectList= _materialTypeBusiness.GetMaterialTypeForSelectList()
            };
            materialAdvanceSearchVM.Unit = new UnitViewModel()
            {
                UnitSelectList = _unitBusiness.GetUnitForSelectList()
            };
            return View(materialAdvanceSearchVM);
        }
        #endregion Index

        #region MaterialDropdown
        public ActionResult MaterialDropdown(MaterialViewModel materialVM)
        {
           // RawMaterialViewModel rawMaterialVM = new RawMaterialViewModel();
           materialVM.MaterialID=materialVM.ID;
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            materialVM.SelectList = new List<SelectListItem>();
            List<MaterialViewModel> materialList = Mapper.Map<List<Material>, List<MaterialViewModel>>(_materialBusiness.GetMaterialForSelectList());
            if (materialList != null)
                foreach (MaterialViewModel material in materialList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = material.MaterialCode + '-'+ material.Description,
                        Value = material.ID.ToString(),
                        Selected = false
                    });
                }
            materialVM.SelectList = selectListItem;
            return PartialView("_MaterialDropdown", materialVM);
        }
        #endregion MaterialDropdown

        #region CheckMaterialCodeExist
        [AcceptVerbs("Get", "Post")]
        public ActionResult CheckMaterialCodeExist(MaterialViewModel materialVM)
       {
            try
            {
                bool exists = _materialBusiness.CheckMaterialCodeExist(Mapper.Map<MaterialViewModel, Material>(materialVM));

                if (exists)
                {
                    return Json("<p><span style='vertical-align: 2px'>Material code already in use </span> <i class='fa fa-close' style='font-size:19px; color: red'></i></p>", JsonRequestBehavior.AllowGet);
                }
                //var result = new { success = true, message = "Success" };
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return Json(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion CheckMaterialCodeExist

        #region GetAllMaterial
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "Material", Mode = "R")]
        public JsonResult GetAllMaterial(DataTableAjaxPostModel model,MaterialAdvanceSearchViewModel materialAdvanceSearchVM)
        {
            try
            {
                //setting options to our model
                materialAdvanceSearchVM.DataTablePaging.Start = model.start;
                materialAdvanceSearchVM.DataTablePaging.Length = (materialAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : materialAdvanceSearchVM.DataTablePaging.Length;

                //bankAdvanceSearchVM.OrderColumn = model.order[0].column;
                //bankAdvanceSearchVM.OrderDir = model.order[0].dir;

                // action inside a standard controller
                List<MaterialViewModel> materialVMList = Mapper.Map<List<Material>, List<MaterialViewModel>>(_materialBusiness.GetAllMaterial(Mapper.Map<MaterialAdvanceSearchViewModel, MaterialAdvanceSearch>(materialAdvanceSearchVM)));
                if (materialAdvanceSearchVM.DataTablePaging.Length == -1)
                {
                    int totalResult = materialVMList.Count != 0 ? materialVMList[0].TotalCount : 0;
                    int filteredResult = materialVMList.Count != 0 ? materialVMList[0].FilteredCount : 0;
                    materialVMList = materialVMList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
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
                    recordsTotal = materialVMList.Count != 0 ? materialVMList[0].TotalCount : 0,
                    recordsFiltered = materialVMList.Count != 0 ? materialVMList[0].FilteredCount : 0,
                    data = materialVMList
                });
            }
            catch(Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return Json(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion GetAllMaterial

        #region InsertUpdateMaterial
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "Material", Mode = "W")]
        public string InsertUpdateMaterial(MaterialViewModel materialVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AppUA appUA = Session["AppUA"] as AppUA;
                    materialVM.Common = new CommonViewModel
                    {
                        CreatedBy = appUA.UserName,
                        CreatedDate = _common.GetCurrentDateTime(),
                        UpdatedBy = appUA.UserName,
                        UpdatedDate = _common.GetCurrentDateTime(),
                    };
                    var result = _materialBusiness.InsertUpdateMaterial(Mapper.Map<MaterialViewModel, Material>(materialVM));
                    return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
                }
                catch (Exception ex)
                {
                    AppConstMessage cm = _appConst.GetMessage(ex.Message);
                    return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
                }
            }
            else
            {
                List<string> modelErrors = new List<string>();
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var modelError in modelState.Errors)
                    {
                        modelErrors.Add(modelError.ErrorMessage);
                    }
                }
                return JsonConvert.SerializeObject(new { Result = "VALIDATION", Message = string.Join(",", modelErrors) });
            }
        }
        #endregion InsertUpdateRawMaterial

        #region MasterPartial
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Material", Mode = "R")]
        public ActionResult MasterPartial(string masterCode)
        {
            MaterialViewModel materialVM = string.IsNullOrEmpty(masterCode) ? new MaterialViewModel() : Mapper.Map<Material, MaterialViewModel>(_materialBusiness.GetMaterial(Guid.Parse(masterCode)));

            if (materialVM.MaterialTypeCode != "CRC")
            {
                materialVM.WeightInKG = materialVM.WeightInKG == 0 ? null : materialVM.WeightInKG;
            }
            materialVM.IsUpdate = string.IsNullOrEmpty(masterCode) ? false : true;
            materialVM.MaterialType = new MaterialTypeViewModel();
            materialVM.MaterialType.MaterialTypeSelectList = _materialTypeBusiness.GetMaterialTypeForSelectList();
            materialVM.Unit = new UnitViewModel();
            materialVM.Unit.UnitSelectList = _unitBusiness.GetUnitForSelectList();

            Permission permission = Session["UserRights"] as Permission;
            string p = permission.SubPermissionList.Where(li => li.Name == "OpeningAccess").First().AccessCode;
            @ViewBag.OpeningAccess = false;
            if (p.Contains("R") || p.Contains("W"))
            {
                @ViewBag.OpeningAccess = true;
            }

            return PartialView("_AddMaterialPartial", materialVM);
        }
        #endregion MasterPartial

        #region DeleteMaterial
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Material", Mode = "D")]
        public string DeleteMaterial(Guid id)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                string deletedBy = appUA.UserName;
                var result = _materialBusiness.DeleteMaterial(id, deletedBy);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }

        }
        #endregion DeleteMaterial

        #region CheckMaterialWeight
        [AcceptVerbs("Get", "Post")]
        public ActionResult CheckMaterialWeight(MaterialViewModel materialVM)
       {
            try
            {
                if (materialVM.MaterialTypeCode == "CRC"&& (materialVM.WeightInKG)==0)
                {
                    return Json("<p><span style='vertical-align: 2px'>Weight In KG should not be 0 </span> <i class='fa fa-close' style='font-size:19px; color: red'></i></p>", JsonRequestBehavior.AllowGet);
                }
              
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return Json(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion CheckMaterialWeight

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Material", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddMaterialMaster('MSTR')";
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetMaterialList();";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "ImportMaterialData();";
                    //---------------------------------------
                    break;
          
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion

    }
}