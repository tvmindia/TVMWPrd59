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
    public class DocumentApprovalController : Controller
    {
        private IDocumentApprovalBusiness _documentApprovalBusiness;
        Common _common = new Common();
        AppConst _appConst = new AppConst();

        public DocumentApprovalController(IDocumentApprovalBusiness documentApprovalBusiness)
        {
            _documentApprovalBusiness = documentApprovalBusiness;
        }

        // GET: DocumentApproval
        [AuthSecurityFilter(ProjectObject = "DocumentApproval", Mode = "R")]
        public ActionResult ViewPendingDocuments(string code)
        {
            ViewBag.SysModuleCode = code;
            return View();
        }
        [AuthSecurityFilter(ProjectObject = "DocumentApproval", Mode = "R")]
        public ActionResult ApproveDocument(string code,string ID,string DocType,string DocID)
        {
            ViewBag.SysModuleCode = code;
            ViewBag.DocumentID = DocID;
            ViewBag.ApprovalLogID = ID;
            ViewBag.DocumentType = DocType;
            return View();
        }

        [AuthSecurityFilter(ProjectObject = "DocumentApproval", Mode = "R")]
        public ActionResult ApprovalHistory()
        {
            ApprovalHistoryViewModel approvalHistoryVM = new ApprovalHistoryViewModel();
            return PartialView("_ApprovalHistory", approvalHistoryVM);
        }

        [AuthSecurityFilter(ProjectObject = "DocumentApproval", Mode = "R")]
        public ActionResult DocumentSummary(DocumentSummaryViewModel documentSummaryVM)
        {
            documentSummaryVM.DataTable = _documentApprovalBusiness.GetDocumentSummary(documentSummaryVM.DocumentID, documentSummaryVM.DocumentTypeCode);
            return PartialView("_DocumentSummary", documentSummaryVM);
        }


        #region GetAllDocumentApproval
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "DocumentApproval", Mode = "R")]
        public JsonResult GetAllDocumentApproval(DataTableAjaxPostModel model, DocumentApprovalAdvanceSearchViewModel documentApprovalAdvanceSearchVM)
        {
            AppUA appUA = Session["AppUA"] as AppUA;
            documentApprovalAdvanceSearchVM.LoginName = appUA.UserName;
            documentApprovalAdvanceSearchVM.DataTablePaging.Start = model.start;
            documentApprovalAdvanceSearchVM.DataTablePaging.Length = (documentApprovalAdvanceSearchVM.DataTablePaging.Length == 0 ? model.length : documentApprovalAdvanceSearchVM.DataTablePaging.Length);
            List<DocumentApprovalViewModel> documentApprovalList = Mapper.Map<List<DocumentApproval>, List<DocumentApprovalViewModel>>(_documentApprovalBusiness.GetAllDocumentsPendingForApprovals(Mapper.Map<DocumentApprovalAdvanceSearchViewModel, DocumentApprovalAdvanceSearch>(documentApprovalAdvanceSearchVM)));
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.None
            };
            return Json(new
            {
                draw = model.draw,
                recordsTotal = documentApprovalList.Count != 0 ? documentApprovalList[0].TotalCount : 0,
                recordsFiltered = documentApprovalList.Count != 0 ? documentApprovalList[0].FilteredCount : 0,
                data = documentApprovalList

            });
        }
        #endregion GetAllDocumentApproval

        #region GetApprovalHistory
        [AuthSecurityFilter(ProjectObject = "DocumentApproval", Mode = "R")]
        public string GetApprovalHistory(string DocumentID, string DocumentTypeCode)
            {
            try
            {
                List<ApprovalHistoryViewModel> approvalHistoryVMList = new List<ApprovalHistoryViewModel>();
                approvalHistoryVMList = Mapper.Map<List<ApprovalHistory>,List< ApprovalHistoryViewModel>>(_documentApprovalBusiness.GetApprovalHistory(Guid.Parse(DocumentID), DocumentTypeCode));
                return JsonConvert.SerializeObject(new { Result = "OK", Records = approvalHistoryVMList });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = ex });
            }
        }
        #endregion GetApprovalHistory

        #region ApproveDocumentInsert
        [AuthSecurityFilter(ProjectObject = "DocumentApproval", Mode = "R")]
        public string ApproveDocumentInsert(string ApprovalLogID, string DocumentID, string DocumentTypeCode)
        {
            try
            {
                var result = _documentApprovalBusiness.ApproveDocumentInsert(Guid.Parse(ApprovalLogID),Guid.Parse(DocumentID),DocumentTypeCode);
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion ApproveDocumentInsert 

        #region RejectDocumentInsert

        #endregion RejectDocumentInsert


        #region ButtonStyling
        [HttpGet]
        [AuthSecurityFilter(ProjectObject = "DocumentApproval", Mode = "R")]
        public ActionResult ChangeButtonStyle(string actionType)
        {
            ToolboxViewModel toolboxVM = new ToolboxViewModel();
            switch (actionType)
            {
                case "List":
                   
                    //----added for reset button---------------
                    toolboxVM.resetbtn.Visible = true;
                    toolboxVM.resetbtn.Text = "Reset";
                    toolboxVM.resetbtn.Title = "Reset All";
                    toolboxVM.resetbtn.Event = "ResetPendingDocList();";
                    //----added for export button--------------
                    toolboxVM.PrintBtn.Visible = true;
                    toolboxVM.PrintBtn.Text = "Export";
                    toolboxVM.PrintBtn.Title = "Export";
                    toolboxVM.PrintBtn.Event = "ExportPendingDocs();";
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