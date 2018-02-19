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
    public class RawMaterialController : Controller
    {
        AppConst _appConst = new AppConst();
        private Common _common = new Common();
        private IRawMaterialBusiness _rawMaterialBusiness;

        #region Constructor Injection
        public RawMaterialController(IRawMaterialBusiness rawMaterialBusiness)
        {
            _rawMaterialBusiness = rawMaterialBusiness;
        }
        #endregion Constructor Injection

        #region Index
        // GET: RawMaterial
        [AuthSecurityFilter(ProjectObject = "RawMaterial", Mode = "R")]
        public ActionResult Index(string code)
        {
            ViewBag.SysModuleCode = code;
            RawMaterialAdvanceSearchViewModel rawMaterialAdvanceSearchVM = new RawMaterialAdvanceSearchViewModel();
            return View(rawMaterialAdvanceSearchVM);
        }
        #endregion Index

        #region RawMaterialDropdown
        public ActionResult RawMaterialDropdown()
        {
            RawMaterialViewModel rawMaterialVM = new RawMaterialViewModel();
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            rawMaterialVM.SelectList = new List<SelectListItem>();
            List<RawMaterialViewModel> rawMaterialList = Mapper.Map<List<RawMaterial>, List<RawMaterialViewModel>>(_rawMaterialBusiness.GetRawMaterialForSelectList());
            if (rawMaterialList != null)
                foreach (RawMaterialViewModel rawMaterial in rawMaterialList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = rawMaterial.MaterialCode + '-'+ rawMaterial.Description,
                        Value = rawMaterial.ID.ToString(),
                        Selected = false
                    });
                }
            rawMaterialVM.SelectList = selectListItem;
            return PartialView("_RawMaterialDropdown", rawMaterialVM);
        }
        #endregion RawMaterialDropdown

        #region CheckMaterialCodeExist
        [AcceptVerbs("Get", "Post")]
        public ActionResult CheckMaterialCodeExist(RawMaterialViewModel rawMaterialVM)
        {
            try
            {
                bool exists = rawMaterialVM.IsUpdate ? false : _rawMaterialBusiness.CheckMaterialCodeExist(rawMaterialVM.MaterialCode);
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

        #region GetAllRawMaterial
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "RawMaterial", Mode = "R")]
        public JsonResult GetAllRawMaterial(DataTableAjaxPostModel model,RawMaterialAdvanceSearchViewModel rawMaterialAdvanceSearchVM)
        {
            try
            {
                //setting options to our model
                rawMaterialAdvanceSearchVM.DataTablePaging.Start = model.start;
                rawMaterialAdvanceSearchVM.DataTablePaging.Length = (rawMaterialAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : rawMaterialAdvanceSearchVM.DataTablePaging.Length;

                //bankAdvanceSearchVM.OrderColumn = model.order[0].column;
                //bankAdvanceSearchVM.OrderDir = model.order[0].dir;

                // action inside a standard controller
                List<RawMaterialViewModel> rawMaterialVMList = Mapper.Map<List<RawMaterial>, List<RawMaterialViewModel>>(_rawMaterialBusiness.GetAllRawMaterial(Mapper.Map<RawMaterialAdvanceSearchViewModel, RawMaterialAdvanceSearch>(rawMaterialAdvanceSearchVM)));
                if (rawMaterialAdvanceSearchVM.DataTablePaging.Length == -1)
                {
                    int totalResult = rawMaterialVMList.Count != 0 ? rawMaterialVMList[0].TotalCount : 0;
                    int filteredResult = rawMaterialVMList.Count != 0 ? rawMaterialVMList[0].FilteredCount : 0;
                    rawMaterialVMList = rawMaterialVMList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
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
                    recordsTotal = rawMaterialVMList.Count != 0 ? rawMaterialVMList[0].TotalCount : 0,
                    recordsFiltered = rawMaterialVMList.Count != 0 ? rawMaterialVMList[0].FilteredCount : 0,
                    data = rawMaterialVMList
                });
            }
            catch(Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return Json(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion GetAllRawMaterial

        #region InsertUpdateRawMaterial
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "RawMaterial", Mode = "R")]
        public string InsertUpdateRawMaterial(RawMaterialViewModel rawMaterialVM)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                rawMaterialVM.Common = new CommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _common.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _common.GetCurrentDateTime(),
                };
                var result = _rawMaterialBusiness.InsertUpdateRawMaterial(Mapper.Map<RawMaterialViewModel, RawMaterial>(rawMaterialVM));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
            }
            catch(Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message=cm.Message });
            }
        }
        #endregion InsertUpdateRawMaterial

        #region MasterPartial
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "RawMaterial", Mode = "R")]
        public ActionResult MasterPartial(string masterCode)
        {
            RawMaterialViewModel rawMaterialVM = string.IsNullOrEmpty(masterCode) ? new RawMaterialViewModel() : Mapper.Map<RawMaterial, RawMaterialViewModel>(_rawMaterialBusiness.GetRawMaterial(Guid.Parse(masterCode)));
            rawMaterialVM.IsUpdate = string.IsNullOrEmpty(masterCode) ? false : true;
            return PartialView("_AddRawMaterialPartial", rawMaterialVM);
        }
        #endregion MasterPartial

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "RawMaterial", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddRawMaterialMaster()";
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetRawMaterialList();";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "ImportRawMaterialData();";
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