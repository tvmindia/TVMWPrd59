using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Newtonsoft.Json;
using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.UserInterface.Models;
using ProductionApp.UserInterface.SecurityFilter;

namespace ProductionApp.UserInterface.Controllers
{
    public class RequisitionController : Controller
    {

        // GET: Requisitions
        private IRequisitionBusiness _requisitionBusiness;
        private IRawMaterialBusiness _rawMaterialBusiness;
        Common _common = new Common();
        AppConst _appConst = new AppConst();

        public RequisitionController(IRequisitionBusiness requisitionBusiness,IRawMaterialBusiness rawMaterialBusiness)
        {
            _requisitionBusiness = requisitionBusiness;
            _rawMaterialBusiness = rawMaterialBusiness;
        }
        public ActionResult ViewRequisition(string code)
        {
            ViewBag.SysModuleCode = code;
         
            return View();
        }

        public ActionResult NewRequisition(string code, Guid? id)
        {
            ViewBag.SysModuleCode = code;
             RequisitionViewModel requisitionVM = new RequisitionViewModel
                {
                    ID = id==null?Guid.Empty:(Guid)id,
                    IsUpdate = id == null ?false: true
                };
                return View(requisitionVM);

            
           
        }

        public ActionResult RequisitionApproval(string code)
        {
            ViewBag.SysModuleCode = code;
            return View();
        }

        #region InsertUpdateRequisition
        [HttpPost]
       // [ValidateAntiForgeryToken]
       // [AuthSecurityFilter(ProjectObject = "Requisition", Mode = "R")]
        public string InsertUpdateRequisition(RequisitionViewModel requisitionVM)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                requisitionVM.Common = new CommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _common.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _common.GetCurrentDateTime(),
                };
                //Deserialize items
                object ResultFromJS = JsonConvert.DeserializeObject(requisitionVM.DetailJSON);
                string ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                requisitionVM.RequisitionDetailList = JsonConvert.DeserializeObject<List<RequisitionDetailViewModel>>(ReadableFormat);
                var result =_requisitionBusiness.InsertUpdateRequisition(Mapper.Map<RequisitionViewModel, Requisition>(requisitionVM));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }

        }
        #endregion InsertUpdateRequisition

        #region GetAllRequisition
        public JsonResult GetAllRequisition(DataTableAjaxPostModel model, RequisitionAdvanceSearchViewModel requisitionAdvanceSearchVM) 
        {
            requisitionAdvanceSearchVM.DataTablePaging.Start = model.start;
            requisitionAdvanceSearchVM.DataTablePaging.Length = (requisitionAdvanceSearchVM.DataTablePaging.Length == 0 ? model.length : requisitionAdvanceSearchVM.DataTablePaging.Length);
            List<RequisitionViewModel> requisitionOrderList = Mapper.Map<List<Requisition>, List<RequisitionViewModel>>(_requisitionBusiness.GetAllRequisition(Mapper.Map<RequisitionAdvanceSearchViewModel, RequisitionAdvanceSearch>(requisitionAdvanceSearchVM)));


            return Json(new
            {
                draw = model.draw,
                recordsTotal = requisitionOrderList.Count != 0 ? requisitionOrderList[0].TotalCount : 0,
                recordsFiltered = requisitionOrderList.Count != 0 ? requisitionOrderList[0].FilteredCount : 0,
                data = requisitionOrderList
            });
        }


        #endregion GetAllRequisition

        #region GetRequisition
        public string GetRequisition(string ID)
        {
            try
            {
                RequisitionViewModel rawMaterialVM = new RequisitionViewModel();
                rawMaterialVM = Mapper.Map<Requisition, RequisitionViewModel>(_requisitionBusiness.GetRequisition(Guid.Parse(ID)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = rawMaterialVM });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex });
            }
        }

        #endregion GetRequisition

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


        #endregion GetRawMaterial

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Requisition", Mode = "")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewRequisition", "Requisition", new { Code = "PURCH" });
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetRequisitionList();";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "ImportRequisitionData();";
                    //---------------------------------------

                    break;
                case "Edit":

                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "New";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewRequisition", "Requisition", new { Code = "PURCH" });

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