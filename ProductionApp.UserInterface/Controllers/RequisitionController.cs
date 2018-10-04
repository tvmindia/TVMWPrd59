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
        private IMaterialBusiness _materialBusiness;
        private IEmployeeBusiness _employeeBusiness;
        Common _common = new Common();
        AppConst _appConst = new AppConst();

        public RequisitionController(IRequisitionBusiness requisitionBusiness,IMaterialBusiness materialBusiness, IEmployeeBusiness employeeBusiness)
        {
            _requisitionBusiness = requisitionBusiness;
            _materialBusiness = materialBusiness;
            _employeeBusiness = employeeBusiness;

        }
        [AuthSecurityFilter(ProjectObject = "Requisition", Mode = "R")]
        public ActionResult ViewRequisition(string code)
        {
            ViewBag.SysModuleCode = code;
            RequisitionAdvanceSearchViewModel requisitionSearchVM = new RequisitionAdvanceSearchViewModel();
            requisitionSearchVM.Employee = new EmployeeViewModel();
            requisitionSearchVM.Employee.SelectList = _employeeBusiness.GetEmployeeSelectList();
            return View(requisitionSearchVM);
        }

        [AuthSecurityFilter(ProjectObject = "Requisition", Mode = "R")]
        public ActionResult NewRequisition(string code, Guid? id)
        {
            ViewBag.SysModuleCode = code;
            RequisitionViewModel requisitionVM = new RequisitionViewModel
            {
                ID = id == null ? Guid.Empty : (Guid)id,
                IsUpdate = id == null ? false : true,
                RequisitionDetail = new RequisitionDetailViewModel
                {
                    Material = new MaterialViewModel()
                    }
                };
                return View(requisitionVM);

            
           
        }

        [AuthSecurityFilter(ProjectObject = "Requisition", Mode = "R")]
        public ActionResult RequisitionApproval(string code)
        {
            ViewBag.SysModuleCode = code;
            return View();
        }

        #region InsertUpdateRequisition
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "Requisition", Mode = "W")]
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
        [AuthSecurityFilter(ProjectObject = "Requisition", Mode = "R")]
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
        [AuthSecurityFilter(ProjectObject = "Requisition", Mode = "R")]
        public string GetRequisition(string ID)
        {
            try
            {
                RequisitionViewModel requisitionVM = new RequisitionViewModel();
                requisitionVM = Mapper.Map<Requisition, RequisitionViewModel>(_requisitionBusiness.GetRequisition(Guid.Parse(ID)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = requisitionVM });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex });
            }
        }

        #endregion GetRequisition

        #region GetRequisitionDetail
        [AuthSecurityFilter(ProjectObject = "Requisition", Mode = "R")]
        public string GetRequisitionDetail(string ID)
        {
            try
            {
                List<RequisitionDetailViewModel> requisitionDetailVM = new List<RequisitionDetailViewModel>();
                requisitionDetailVM = Mapper.Map<List<RequisitionDetail>,List<RequisitionDetailViewModel>>(_requisitionBusiness.GetRequisitionDetail(Guid.Parse(ID)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = requisitionDetailVM });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex });
            }
        }

        #endregion GetRequisitionDetail

        #region GetMaterial
        [AuthSecurityFilter(ProjectObject = "Requisition", Mode = "R")]
        public string GetMaterial(string ID)
        {
            try
            {
                MaterialViewModel materialVM = new MaterialViewModel();
                materialVM = Mapper.Map<Material, MaterialViewModel>(_materialBusiness.GetMaterial(Guid.Parse(ID)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = materialVM });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex });
            }
        }
        #endregion GetMaterial

        #region DeleteRequisitionDetail
        [AuthSecurityFilter(ProjectObject = "Requisition", Mode = "D")]
        public string DeleteRequisitionDetail(string ID)
        {
            object result = null;
            try
            {
                if (string.IsNullOrEmpty(ID))
                {
                    throw new Exception("ID Missing");
                }
                result = _requisitionBusiness.DeleteRequisitionDetail(Guid.Parse(ID));
                return JsonConvert.SerializeObject(new { Result = "OK", Record = result, Message = _appConst.DeleteSuccess });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }

        }

        #endregion DeleteRequisitionDetail


        #region DeleteRequisition
        [AuthSecurityFilter(ProjectObject = "Requisition", Mode = "D")]
        public string DeleteRequisition(string ID)
        {
            object result = null;
            try
            {
                if (string.IsNullOrEmpty(ID))
                {
                    throw new Exception("ID Missing");
                }
                result = _requisitionBusiness.DeleteRequisition(Guid.Parse(ID));
                return JsonConvert.SerializeObject(new { Result = "OK", Record = result, Message = _appConst.DeleteSuccess });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }

        }

        #endregion DeleteRequisition

        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "Requisition", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewRequisition", "Requisition", new { code = "PURCH" });
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
                    toolboxVM.addbtn.Href = Url.Action("NewRequisition", "Requisition", new { code = "PURCH" });

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

                    toolboxVM.SendForApprovalBtn.Visible = true;
                    toolboxVM.SendForApprovalBtn.Text = "Send";
                    toolboxVM.SendForApprovalBtn.Title = "Send For Approval";
                    toolboxVM.SendForApprovalBtn.Event = "ShowSendForApproval('REQ');";

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Href = Url.Action("ViewRequisition", "Requisition", new { code = "PURCH" });


                    //Always To be placed Last
                    toolboxVM.AboutBtn.Visible = true;
                    toolboxVM.AboutBtn.Text = "History";
                    toolboxVM.AboutBtn.Title = "About Approval History";
                    toolboxVM.AboutBtn.Event = "ShowApprovalHistory()";

                    break;

                case "Disable":

                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "New";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewRequisition", "Requisition", new { code = "PURCH" });

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "Save();";

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Href = Url.Action("ViewRequisition", "Requisition", new { code = "PURCH" });


                    //Always To be placed Last
                    toolboxVM.AboutBtn.Visible = true;
                    toolboxVM.AboutBtn.Text = "History";
                    toolboxVM.AboutBtn.Title = "About Approval History";
                    toolboxVM.AboutBtn.Event = "ShowApprovalHistory()";

                    break;
                case "Add":

                    toolboxVM.savebtn.Visible = true;
                    toolboxVM.savebtn.Text = "Save";
                    toolboxVM.savebtn.Title = "Save";
                    toolboxVM.savebtn.Event = "Save();";

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Href = Url.Action("ViewRequisition", "Requisition", new { code = "PURCH" });
                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion

    }
}