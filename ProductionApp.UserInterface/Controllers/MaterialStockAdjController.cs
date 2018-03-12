using ProductionApp.UserInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.UserInterface.SecurityFilter;
using Newtonsoft.Json;
using AutoMapper;


namespace ProductionApp.UserInterface.Controllers
{
    public class MaterialStockAdjController : Controller
    {
        // GET: MaterialStockAdj
        private IMaterialBusiness _materialBusiness;
        private IMaterialStockAdjBusiness _materialStockAdjBusiness;
        private IDocumentApprovalBusiness _documentApprovalBusiness;
        //private IEmployeeBusiness _employeeBusiness;
        Common _common = new Common();
        AppConst _appConst = new AppConst();
        public MaterialStockAdjController(IMaterialStockAdjBusiness materialStockAdjBusiness, IMaterialBusiness materialBusiness, IDocumentApprovalBusiness documentApprovalBusiness)
        {
            _materialStockAdjBusiness = materialStockAdjBusiness;
            _materialBusiness = materialBusiness;
            _documentApprovalBusiness = documentApprovalBusiness;
        }
        public ActionResult Index()
        {
            return View();
        }

        #region ListStockAdjustment
        public ActionResult ListStockAdjustment(string code)
        {
            ViewBag.SysModuleCode = code;            
            return View();

        }
        #endregion 

        #region NewStockAdjustment
        public ActionResult NewStockAdjustment(string code,Guid?id)
        {
            ViewBag.SysModuleCode = code;
            MaterialStockAdjViewModel materialStockAdjVM = new MaterialStockAdjViewModel
            {
                ID=id==null?Guid.Empty:(Guid)id,
                IsUpdate = id == null ? false : true,
                MaterialStockAdjDetail=new MaterialStockAdjDetailViewModel
                {
                    Material = new MaterialViewModel()
                }
            };
            return View(materialStockAdjVM);
        }
        #endregion

        #region GetApprovers
        public ActionResult GetApprovers()
        {
            DocumentApproverViewModel SendForApprovalVM = new DocumentApproverViewModel();
            SendForApprovalVM.SendForApprovalList = Mapper.Map<List<DocumentApprover>, List<DocumentApproverViewModel>>(_documentApprovalBusiness.GetApproversByDocType("SSADJ"));
            SendForApprovalVM.ApproversCount = SendForApprovalVM.SendForApprovalList.Select(m => m.ApproverLevel).Distinct().Count();
            return PartialView("_SendForApproval", SendForApprovalVM);

        }
        #endregion

        #region GetMaterial        
        public string GetMaterial(string id)
        {
            try
            {
                MaterialViewModel materialVM = new MaterialViewModel();
                materialVM = Mapper.Map<Material, MaterialViewModel>(_materialBusiness.GetMaterial(Guid.Parse(id)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = materialVM, Message = "Sucess" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = ex });
            }
        }
        #endregion GetMaterial

        #region GetAllMaterialStockAdjustment
        public JsonResult GetAllMaterialStockAdjustment(DataTableAjaxPostModel model,MaterialStockAdjAdvanceSearchViewModel materialStockAdjAdvanceSearchVM)
        {
            materialStockAdjAdvanceSearchVM.DataTablePaging.Start = model.start;
            materialStockAdjAdvanceSearchVM.DataTablePaging.Length = (materialStockAdjAdvanceSearchVM.DataTablePaging.Length == 0 ? model.length : materialStockAdjAdvanceSearchVM.DataTablePaging.Length);
            List<MaterialStockAdjViewModel> materialStockAdjList = Mapper.Map<List<MaterialStockAdj>, List<MaterialStockAdjViewModel>>(_materialStockAdjBusiness.GetAllMaterialStockAdjustment(Mapper.Map<MaterialStockAdjAdvanceSearchViewModel, MaterialStockAdjAdvanceSearch>(materialStockAdjAdvanceSearchVM)));
            if (materialStockAdjAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = materialStockAdjList.Count != 0 ? materialStockAdjList[0].TotalCount : 0;
                int filteredResult = materialStockAdjList.Count != 0 ? materialStockAdjList[0].FilteredCount : 0;
                materialStockAdjList = materialStockAdjList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }

            return Json(new
            {
                draw = model.draw,
                recordsTotal = materialStockAdjList.Count != 0 ? materialStockAdjList[0].TotalCount : 0,
                recordsFiltered = materialStockAdjList.Count != 0 ? materialStockAdjList[0].FilteredCount : 0,
                data = materialStockAdjList
            });
        }
        #endregion

        #region InsertUpdateStockAdjustment
        public string InsertUpdateStockAdjustment(MaterialStockAdjViewModel materialstockAdjVM)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                materialstockAdjVM.Common = new CommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _common.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _common.GetCurrentDateTime(),
                };

                object ResultFromJS = JsonConvert.DeserializeObject(materialstockAdjVM.DetailJSON);
                string ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                materialstockAdjVM.MaterialStockAdjDetailList = JsonConvert.DeserializeObject<List<MaterialStockAdjDetailViewModel>>(ReadableFormat);
                var result = _materialStockAdjBusiness.InsertUpdateStockAdjustment(Mapper.Map<MaterialStockAdjViewModel, MaterialStockAdj>(materialstockAdjVM));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result, Message = "Success" });
            }

            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = cm.Message });
            }
        }
        #endregion

        #region GetMaterialStockAdjustment
        public string GetMaterialStockAdjustment(string id)
        {
            try
            {
                MaterialStockAdjViewModel materialStockAdjVM = new MaterialStockAdjViewModel();
                materialStockAdjVM = Mapper.Map<MaterialStockAdj, MaterialStockAdjViewModel>(_materialStockAdjBusiness.GetMaterialStockAdjustment(Guid.Parse(id)));
                return JsonConvert.SerializeObject(new { Result = "OK", Record = materialStockAdjVM, Message = "Success" });
            }
            catch(Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Record = "", Message = ex });
            }

        }
        #endregion

        #region GetMaterialStockAdjustmentDetail
        public string GetMaterialStockAdjustmentDetail(string id)
        {
            try
            {
                List<MaterialStockAdjDetailViewModel> materialStockAdjDetailVM = new List<MaterialStockAdjDetailViewModel>();
                materialStockAdjDetailVM = Mapper.Map<List<MaterialStockAdjDetail>, List<MaterialStockAdjDetailViewModel>>(_materialStockAdjBusiness.GetMaterialStockAdjustmentDetail(Guid.Parse(id)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = materialStockAdjDetailVM, Message = "Success" });
            }
            catch(Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = ex });
            }
        }
        #endregion

        #region DeleteMaterialStockAdjustment
        public string DeleteMaterialStockAdjustment(string id)
        {
            object result = null;
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    throw new Exception("ID Missing");
                }
                result = _materialStockAdjBusiness.DeleteMaterialStockAdjustment(Guid.Parse(id));
                return JsonConvert.SerializeObject(new { Result = "OK", Record = result, Message = _appConst.DeleteSuccess });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion

        #region DeleteMaterialStockAdjustmentDetail
        public string DeleteMaterialStockAdjustmentDetail(string id)
        {
            object result = null;
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    throw new Exception("ID Missing");
                }
                result = _materialStockAdjBusiness.DeleteMaterialStockAdjustmentDetail(Guid.Parse(id));
                return JsonConvert.SerializeObject(new { Result = "OK", Record = result, Message = _appConst.DeleteSuccess });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion

        #region ButtonStyling
        [HttpGet]
        //[AuthSecurityFilter(ProjectObject = "IssueToProduction", Mode = "")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewStockAdjustment", "MaterialStockAdj", new { code = "STR" });
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetStockAdjustmentList();";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "ImportStockAdjustmentData();";
                    //---------------------------------------

                    break;
                case "Edit":

                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "New";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewStockAdjustment", "MaterialStockAdj", new { code = "STR" });

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
                    toolboxVM.SendForApprovalBtn.Event = "ShowSendForApproval('SSADJ');";

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Event = "";
                    toolboxVM.ListBtn.Href = Url.Action("ListStockAdjustment", "MaterialStockAdj", new { Code = "STR" });
                    break;

                case "Disable":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "New";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewStockAdjustment", "MaterialStockAdj", new { Code = "STR" });

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Href = Url.Action("ListStockAdjustment", "MaterialStockAdj", new { Code = "STR" });

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
                    toolboxVM.ListBtn.Href = Url.Action("ListStockAdjustment", "MaterialStockAdj", new { Code = "STR" });


                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion
    }
}