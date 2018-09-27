using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProductionApp.UserInterface.Models;
using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using Newtonsoft.Json;
using AutoMapper;

namespace ProductionApp.UserInterface.Controllers
{
    public class FinishedGoodStockAdjController : Controller
    {
        // GET: FinishedGoodStockAdj
        private IProductBusiness _productBusiness;
        private IFinishedGoodStockAdjBusiness _finishedGoodStockAdjBusiness;
        private IDocumentApprovalBusiness _documentApprovalBusiness;
        private IEmployeeBusiness _employeeBusiness;
        Common _common = new Common();
        AppConst _appConst = new AppConst();

        public FinishedGoodStockAdjController(IProductBusiness productBusiness,IFinishedGoodStockAdjBusiness finishedGoodStockAdjBusiness, IDocumentApprovalBusiness documentApprovalBusiness, IEmployeeBusiness employeeBusiness)
        {
            _productBusiness = productBusiness;
            _finishedGoodStockAdjBusiness = finishedGoodStockAdjBusiness;
            _documentApprovalBusiness = documentApprovalBusiness;
            _employeeBusiness = employeeBusiness;
        }

        public ActionResult Index()
        {
            return View();
        }

        #region ViewFinishedGoodStockAdjustment
        public ActionResult ViewFinishedGoodStockAdj(string code)
        {
            ViewBag.SysModuleCode = code;
            FinishedGoodStockAdjAdvanceSearchViewModel finishGoodAdjAdvanceSearchVM = new FinishedGoodStockAdjAdvanceSearchViewModel();
            finishGoodAdjAdvanceSearchVM.Employee = new EmployeeViewModel();
            finishGoodAdjAdvanceSearchVM.Employee.SelectList = _employeeBusiness.GetEmployeeSelectList();
            return View(finishGoodAdjAdvanceSearchVM);
        }
        #endregion

        #region NewFinishedGoodStockAdjustment
        public ActionResult NewFinishedGoodStockAdj(string code,Guid?id)
        {
            ViewBag.SysModuleCode = code;
            FinishedGoodStockAdjViewModel finishedGoodStockAdjVM = new FinishedGoodStockAdjViewModel
            {
                ID = id == null ? Guid.Empty : (Guid)id,
                IsUpdate = id == null ? false : true,
                FinishedGoodStockAdjDetail = new FinishedGoodStockAdjDetailViewModel
                {
                    Product = new ProductViewModel()
                }

            };
            return View(finishedGoodStockAdjVM);
        }
        #endregion

        #region GetAllFinishedGoodStockAdj
        public JsonResult GetAllFinishedGoodStockAdj(DataTableAjaxPostModel model,FinishedGoodStockAdjAdvanceSearchViewModel finishedGoodStockAdjAdvanceSearchVM)
        {
            finishedGoodStockAdjAdvanceSearchVM.DataTablePaging.Start = model.start;
            finishedGoodStockAdjAdvanceSearchVM.DataTablePaging.Length = (finishedGoodStockAdjAdvanceSearchVM.DataTablePaging.Length == 0 ? model.length : finishedGoodStockAdjAdvanceSearchVM.DataTablePaging.Length);
            List<FinishedGoodStockAdjViewModel> finishedGoodStockAdjList = Mapper.Map<List<FinishedGoodStockAdj>, List<FinishedGoodStockAdjViewModel>>(_finishedGoodStockAdjBusiness.GetAllFinishedGoodStockAdj(Mapper.Map<FinishedGoodStockAdjAdvanceSearchViewModel,FinishedGoodStockAdjAdvanceSearch>(finishedGoodStockAdjAdvanceSearchVM)));
            if (finishedGoodStockAdjAdvanceSearchVM.DataTablePaging.Length == -1)
            {
                int totalResult = finishedGoodStockAdjList.Count != 0 ? finishedGoodStockAdjList[0].TotalCount : 0;
                int filteredResult = finishedGoodStockAdjList.Count != 0 ? finishedGoodStockAdjList[0].FilteredCount : 0;
                finishedGoodStockAdjList = finishedGoodStockAdjList.Skip(0).Take(filteredResult > 10000 ? 10000 : filteredResult).ToList();
            }

            return Json(new
            {
                draw = model.draw,
                recordsTotal = finishedGoodStockAdjList.Count != 0 ? finishedGoodStockAdjList[0].TotalCount : 0,
                recordsFiltered = finishedGoodStockAdjList.Count != 0 ? finishedGoodStockAdjList[0].FilteredCount : 0,
                data = finishedGoodStockAdjList
            });
        }
        #endregion

        #region GetProduct       
        public string GetProduct(string ID)
        {
            try
            {
                ProductViewModel productVM = new ProductViewModel();
                productVM = Mapper.Map<Product, ProductViewModel>(_productBusiness.GetProduct(Guid.Parse(ID)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = productVM });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex });
            }
        }
        #endregion

        #region GetApprovers
        public ActionResult GetApprovers()
        {
            DocumentApproverViewModel SendForApprovalVM = new DocumentApproverViewModel();
            SendForApprovalVM.SendForApprovalList = Mapper.Map<List<DocumentApprover>, List<DocumentApproverViewModel>>(_documentApprovalBusiness.GetApproversByDocType("FGADJ"));
            SendForApprovalVM.ApproversCount = SendForApprovalVM.SendForApprovalList.Select(m => m.ApproverLevel).Distinct().Count();
            return PartialView("_SendForApproval", SendForApprovalVM);

        }
        #endregion

        #region InsertUpdateFinishedGoodStockAdj
        public string InsertUpdateFinishedGoodStockAdj(FinishedGoodStockAdjViewModel finishedGoodStockAdjVM)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                finishedGoodStockAdjVM.Common = new CommonViewModel
                {
                    CreatedBy = appUA.UserName,
                    CreatedDate = _common.GetCurrentDateTime(),
                    UpdatedBy = appUA.UserName,
                    UpdatedDate = _common.GetCurrentDateTime(),
                };
                object ResultFromJS = JsonConvert.DeserializeObject(finishedGoodStockAdjVM.DetailJSON);
                string ReadableFormat = JsonConvert.SerializeObject(ResultFromJS);
                finishedGoodStockAdjVM.FinishedGoodStockAdjDetailList = JsonConvert.DeserializeObject<List<FinishedGoodStockAdjDetailViewModel>>(ReadableFormat);
                var result = _finishedGoodStockAdjBusiness.InsertUpdateFinishedGoodStockAdj(Mapper.Map<FinishedGoodStockAdjViewModel, FinishedGoodStockAdj>(finishedGoodStockAdjVM));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result, Message = "Success" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = ex.Message });
            }
        }
        #endregion

        #region GetFinishedGoodStockAdj
        public string GetFinishedGoodStockAdj(string id)
        {
            try
            {
                FinishedGoodStockAdjViewModel finishedGoodStockAdjVM = new FinishedGoodStockAdjViewModel();
                finishedGoodStockAdjVM= Mapper.Map<FinishedGoodStockAdj, FinishedGoodStockAdjViewModel>(_finishedGoodStockAdjBusiness.GetFinishedGoodStockAdj(Guid.Parse(id)));
                return JsonConvert.SerializeObject(new { Result = "OK", Record = finishedGoodStockAdjVM, Message = "Success" });
            }
            catch(Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Record = "", Message = ex });
            }
        }
        #endregion

        #region GetFinishedGoodStockAdjDetail
        public string GetFinishedGoodStockAdjDetail(string id)
        {
            try
            {
                List<FinishedGoodStockAdjDetailViewModel> finishedGoodStockAdjDetailVM = new List<FinishedGoodStockAdjDetailViewModel>();
                finishedGoodStockAdjDetailVM = Mapper.Map<List<FinishedGoodStockAdjDetail>, List<FinishedGoodStockAdjDetailViewModel>>(_finishedGoodStockAdjBusiness.GetFinishedGoodStockAdjDetail(Guid.Parse(id)));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = finishedGoodStockAdjDetailVM, Message = "Success" });
            }
            catch(Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Records = "", Message = ex });
            }
        }
        #endregion

        #region DeleteFinishedGoodStockAdj
        public string DeleteFinishedGoodStockAdj(string id)
        {
            object result = null;
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    throw new Exception("ID Missing");
                }
                result = _finishedGoodStockAdjBusiness.DeleteFinishedGoodStockAdj(Guid.Parse(id));
                return JsonConvert.SerializeObject(new { Result = "OK", Record = result, Message = _appConst.DeleteSuccess });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion

        #region DeleteFinishedGoodStockAdjDetail
        public string DeleteFinishedGoodStockAdjDetail(string id)
        {
            object result = null;
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    throw new Exception("ID Missing");
                }
                result = _finishedGoodStockAdjBusiness.DeleteFinishedGoodStockAdjDetail(Guid.Parse(id));
                return JsonConvert.SerializeObject(new { Result = "OK", Record = result, Message = _appConst.DeleteSuccess });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Record = "", Message = cm.Message });
            }
        }
        #endregion


        #region CheckUnpostedProductExists
        [HttpGet]
        
        public string CheckUnpostedProductExists(Guid adjustmentID)
        {          
            try
            {
                FinishedGoodStockAdjViewModel finishedGoodStockAdjVM = new FinishedGoodStockAdjViewModel();
                finishedGoodStockAdjVM = Mapper.Map<FinishedGoodStockAdj, FinishedGoodStockAdjViewModel>(_finishedGoodStockAdjBusiness.CheckUnpostedProductExists(adjustmentID));
                return JsonConvert.SerializeObject(new { Result = "OK", Record = finishedGoodStockAdjVM, Message = "Success" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Record = "", Message = ex });
            }
        }


        #endregion CheckUnpostedProductExists

        #region GetAllUnpostedData        
        public string GetAllUnpostedData(Guid adjustmentID)
        {
            try
            {
                List<FinishedGoodStockAdjViewModel> finishedGoodStockAdjVM = new List<FinishedGoodStockAdjViewModel>();
                finishedGoodStockAdjVM = Mapper.Map<List<FinishedGoodStockAdj>, List<FinishedGoodStockAdjViewModel>>(_finishedGoodStockAdjBusiness.GetAllUnpostedData(adjustmentID));
                return JsonConvert.SerializeObject(new { Result = "OK", Record = finishedGoodStockAdjVM, Message = "Success" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Record = "", Message = ex });
            }
        }
        #endregion GetAllUnpostedData

        #region ButtonStyling
        [HttpGet]
        //[AuthSecurityFilter(ProjectObject = "MaterialStockAdjustment", Mode = "")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "Add";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewFinishedGoodStockAdj", "FinishedGoodStockAdj", new { code = "STR" });
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetFGStockAdjustmentList();";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "ImportFGStockAdjustmentData();";
                    //---------------------------------------

                    break;
                case "Edit":

                    toolboxVM.addbtn.Visible = true;
                    toolboxVM.addbtn.Text = "New";
                    toolboxVM.addbtn.Title = "Add New";
                    toolboxVM.addbtn.Href = Url.Action("NewFinishedGoodStockAdj", "FinishedGoodStockAdj", new { code = "STR" });

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
                    toolboxVM.ListBtn.Href = Url.Action("ViewFiniShedGoodStockAdj", "FinishedGoodStockAdj", new { code = "STR" });


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
                    toolboxVM.addbtn.Href = Url.Action("NewFinishedGoodStockAdj", "FinishedGoodStockAdj", new { code = "STR" });

                    toolboxVM.ListBtn.Visible = true;
                    toolboxVM.ListBtn.Text = "List";
                    toolboxVM.ListBtn.Title = "List";
                    toolboxVM.ListBtn.Href = Url.Action("ViewFiniShedGoodStockAdj", "FinishedGoodStockAdj", new { code = "STR" });


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
                    toolboxVM.ListBtn.Event = "";
                    toolboxVM.ListBtn.Href = Url.Action("ViewFiniShedGoodStockAdj", "FinishedGoodStockAdj", new { code = "STR" });


                    break;
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion
    }
}