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
    public class ServiceItemController : Controller
    {
        Common _common = new Common();
        AppConst _appConst = new AppConst();
        #region Constructor Injection
        private IServiceItemBusiness _serviceItemBusiness;
        public ServiceItemController(IServiceItemBusiness serviceItemBusiness)
        {
            _serviceItemBusiness = serviceItemBusiness;
        }
        #endregion Constructor Injection

        // GET: ServiceItem
        [AuthSecurityFilter(ProjectObject = "ServiceItem", Mode = "R")]
        public ActionResult Index(string code)
        {
            ViewBag.SysModuleCode = code;
            return View();
        }

        #region GetAllServiceItem
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "ServiceItem", Mode = "R")]
        public JsonResult GetAllServiceItem(DataTableAjaxPostModel model, ServiceItemAdvanceSearchViewModel serviceItemAdvanceSearchVM)
        {
            try
            {
                //setting options to our model
                serviceItemAdvanceSearchVM.DataTablePaging.Start = model.start;
                serviceItemAdvanceSearchVM.DataTablePaging.Length = (serviceItemAdvanceSearchVM.DataTablePaging.Length == 0) ? model.length : serviceItemAdvanceSearchVM.DataTablePaging.Length;

                // action inside a standard controller
                List<ServiceItemViewModel> serviceItemVMList = Mapper.Map<List<ServiceItem>, List<ServiceItemViewModel>>(_serviceItemBusiness.GetAllServiceItem(Mapper.Map<ServiceItemAdvanceSearchViewModel, ServiceItemAdvanceSearch>(serviceItemAdvanceSearchVM)));
                if (serviceItemAdvanceSearchVM.DataTablePaging.Length == -1)
                {
                    int totalResult = serviceItemVMList.Count != 0 ? serviceItemVMList[0].TotalCount : 0;
                    int filteredResult = serviceItemVMList.Count != 0 ? serviceItemVMList[0].FilteredCount : 0;
                    serviceItemVMList = serviceItemVMList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
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
                    recordsTotal = serviceItemVMList.Count != 0 ? serviceItemVMList[0].TotalCount : 0,
                    recordsFiltered = serviceItemVMList.Count != 0 ? serviceItemVMList[0].FilteredCount : 0,
                    data = serviceItemVMList
                });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return Json(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion GetAllServiceItem

        #region MasterPartial
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ServiceItem", Mode = "R")]
        public ActionResult MasterPartial(string masterCode)
        {
            ServiceItemViewModel serviceItemVM = string.IsNullOrEmpty(masterCode) ? new ServiceItemViewModel() : Mapper.Map<ServiceItem, ServiceItemViewModel>(_serviceItemBusiness.GetServiceItem(Guid.Parse(masterCode)));
            serviceItemVM.IsUpdate = string.IsNullOrEmpty(masterCode) ? false : true;
            return PartialView("_AddServiceItemPartial", serviceItemVM);
        }
        #endregion MasterPartial

        #region InsertUpdateServiceItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthSecurityFilter(ProjectObject = "ServiceItem", Mode = "W")]
        public string InsertUpdateServiceItem(ServiceItemViewModel serviceItemVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AppUA appUA = Session["AppUA"] as AppUA;
                    serviceItemVM.Common = new CommonViewModel
                    {
                        CreatedBy = appUA.UserName,
                        CreatedDate = _common.GetCurrentDateTime(),
                        UpdatedBy = appUA.UserName,
                        UpdatedDate = _common.GetCurrentDateTime(),
                    };
                    var result = _serviceItemBusiness.InsertUpdateServiceItem(Mapper.Map<ServiceItemViewModel, ServiceItem>(serviceItemVM));
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
        #endregion InsertUpdateServiceItem

        #region DeleteServiceItem
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ServiceItem", Mode = "D")]
        public string DeleteServiceItem(Guid id)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                string deletedBy = appUA.UserName;
                var result = _serviceItemBusiness.DeleteServiceItem(id, deletedBy);
                return JsonConvert.SerializeObject(new { Status = "OK", Record = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Status = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion DeleteServiceItem

        //#region ServiceItemDropdown
        //public ActionResult ServiceItemDropdown(ServiceItemViewModel serviceItemVM)
        //{
        //    try
        //    {
        //        List<SelectListItem> selectListItem = new List<SelectListItem>();
        //        serviceItemVM.SelectList = new List<SelectListItem>();
        //        List<ServiceItemViewModel> serviceItemList = Mapper.Map<List<ServiceItem>, List<ServiceItemViewModel>>(_serviceItemBusiness.GetServiceItemForSelectList());
        //        if (serviceItemList != null)
        //        {
        //            foreach (ServiceItemViewModel serviceItem in serviceItemList)
        //            {
        //                selectListItem.Add(new SelectListItem
        //                {
        //                    Text = serviceItem.Description,
        //                    Value = serviceItem.ID.ToString(),
        //                    Selected = false
        //                });
        //            }
        //        }
        //        serviceItemVM.SelectList = selectListItem;
        //        return PartialView("_ServiceItemDropdown", serviceItemVM);
        //    }
        //    catch (Exception ex)
        //    {
        //        AppConstMessage cm = _appConst.GetMessage(ex.Message);
        //        return Json(new { Result = "ERROR", Message = cm.Message });
        //    }
        //}
        //#endregion ServiceItemDropdown

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "ServiceItem", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Event = "AddServiceItemMaster('MSTR')";
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetServiceItemList();";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "ImportServiceItemData();";
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