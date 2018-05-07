using AutoMapper;
using Newtonsoft.Json;
using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.UserInterface.Models;
using ProductionApp.UserInterface.SecurityFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [AuthSecurityFilter(ProjectObject = "DocumentApproval", Mode = "R")]
        public ActionResult ViewApprovalHistory(string code)
        {
            ViewBag.SysModuleCode = code;
            return View();
        }

        [AuthSecurityFilter(ProjectObject = "DocumentApproval", Mode = "R")]
        public ActionResult AboutApprovalHistory(string id, string docType)
        {
            ViewBag.DocumentID = id;
            ViewBag.DocumentType = docType;
            DocumentApprovalViewModel documentApprovalVM = new DocumentApprovalViewModel();
            documentApprovalVM.ApprovalHistoryList = Mapper.Map<List<ApprovalHistory>, List<ApprovalHistoryViewModel>>(_documentApprovalBusiness.GetApprovalHistory(Guid.Parse(id), docType));
            return PartialView("_AboutApprovalHistory", documentApprovalVM);
        }

        #region Approvals
        public ActionResult GetApprovers(DocumentApprovalViewModel documentApprovalVM)
        {
            DocumentApproverViewModel SendForApprovalVM = new DocumentApproverViewModel();
            SendForApprovalVM.SendForApprovalList = Mapper.Map<List<DocumentApprover>, List<DocumentApproverViewModel>>(_documentApprovalBusiness.GetApproversByDocType(documentApprovalVM.DocumentType));
            SendForApprovalVM.ApproversCount = SendForApprovalVM.SendForApprovalList.Select(m => m.ApproverLevel).Distinct().Count();
            return PartialView("_SendForApproval", SendForApprovalVM);

        }
        #endregion Approvals

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
        public async Task<string> ApproveDocumentInsert(string ApprovalLogID, string DocumentID, string DocumentTypeCode, string Remarks)
        {
            try
            {
                DateTime approvalDate = _common.GetCurrentDateTime();
                var result = _documentApprovalBusiness.ApproveDocument(Guid.Parse(ApprovalLogID),Guid.Parse(DocumentID),DocumentTypeCode, Remarks, approvalDate);
                bool mailresult = await _documentApprovalBusiness.SendApprolMails(Guid.Parse(DocumentID), DocumentTypeCode);

                return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion ApproveDocumentInsert 

        #region RejectDocument
        [AuthSecurityFilter(ProjectObject = "DocumentApproval", Mode = "R")]
        public async Task<string> RejectDocument(string ApprovalLogID, string DocumentID, string DocumentTypeCode,string Remarks)
        {
            try
            {
                DateTime rejectionDate = _common.GetCurrentDateTime();
                var result = _documentApprovalBusiness.RejectDocument(Guid.Parse(ApprovalLogID), Guid.Parse(DocumentID), DocumentTypeCode,Remarks, rejectionDate);
                bool mailresult = await _documentApprovalBusiness.SendApprolMails(Guid.Parse(DocumentID), DocumentTypeCode);
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }
        #endregion RejectDocument

        #region ValidateDocumentsApprovalPermission
        [AuthSecurityFilter(ProjectObject = "DocumentApproval", Mode = "R")]
        public string ValidateDocumentsApprovalPermission( string DocumentID, string DocumentTypeCode)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                string LoginName = appUA.UserName;
                var result = _documentApprovalBusiness.ValidateDocumentsApprovalPermission(LoginName, Guid.Parse(DocumentID), DocumentTypeCode);
                return JsonConvert.SerializeObject(new { Result = "OK", Records = result });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }

        #endregion ValidateDocumentsApprovalPermission

        #region SendDocForApproval
        [AuthSecurityFilter(ProjectObject = "DocumentApproval", Mode = "R")]
        public async Task <string> SendDocForApproval(string documentID, string documentTypeCode,string approvers)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                string createdBy = appUA.UserName;
                DateTime createdDate = _common.GetCurrentDateTime();

                var result = _documentApprovalBusiness.SendDocForApproval(Guid.Parse(documentID), documentTypeCode, approvers, createdBy, createdDate);

                bool mailresult = await _documentApprovalBusiness.SendApprolMails(Guid.Parse(documentID), documentTypeCode);

                return JsonConvert.SerializeObject(new { Result = "OK", Message = result });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }

        #endregion SendDocForApproval

        #region ReSendDocForApproval
        [AuthSecurityFilter(ProjectObject = "DocumentApproval", Mode = "R")]
        public async Task<string> ReSendDocForApproval(string documentID, string documentTypeCode, string latestApprovalID)
        {
            try
            {
                AppUA appUA = Session["AppUA"] as AppUA;
                string createdBy = appUA.UserName;
                DateTime createdDate = _common.GetCurrentDateTime();

                var result = _documentApprovalBusiness.ReSendDocForApproval(Guid.Parse(documentID), documentTypeCode,Guid.Parse(latestApprovalID), createdBy, createdDate);
                bool mailresult = await _documentApprovalBusiness.SendApprolMails(Guid.Parse(documentID), documentTypeCode);
                return JsonConvert.SerializeObject(new { Result = "OK", Message = result });
            }
            catch (Exception ex)
            {
                AppConstMessage cm = _appConst.GetMessage(ex.Message);
                return JsonConvert.SerializeObject(new { Result = "ERROR", Message = cm.Message });
            }
        }

        #endregion ReSendDocForApproval

        #region GetAllApprovalHistory
        [HttpPost]
        [AuthSecurityFilter(ProjectObject = "DocumentApproval", Mode = "R")]
        public JsonResult GetAllApprovalHistory(DataTableAjaxPostModel model, DocumentApprovalAdvanceSearchViewModel documentApprovalAdvanceSearchVM)
        {
            AppUA appUA = Session["AppUA"] as AppUA;
            documentApprovalAdvanceSearchVM.LoginName = appUA.UserName;
            documentApprovalAdvanceSearchVM.DataTablePaging.Start = model.start;
            documentApprovalAdvanceSearchVM.DataTablePaging.Length = (documentApprovalAdvanceSearchVM.DataTablePaging.Length == 0 ? model.length : documentApprovalAdvanceSearchVM.DataTablePaging.Length);
            List<DocumentApprovalViewModel> documentApprovalList = Mapper.Map<List<DocumentApproval>, List<DocumentApprovalViewModel>>(_documentApprovalBusiness.GetAllApprovalHistory(Mapper.Map<DocumentApprovalAdvanceSearchViewModel, DocumentApprovalAdvanceSearch>(documentApprovalAdvanceSearchVM)));
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
        #endregion GetAllApprovalHistory

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
                case "Back":
                    toolboxVM.backbtn.Visible = true;
                    toolboxVM.backbtn.Text = "Back";
                    toolboxVM.backbtn.Title = "Back";
                    toolboxVM.ListBtn.Event = "";
                    toolboxVM.backbtn.Href = Url.Action("ViewPendingDocuments", "DocumentApproval", new { Code = "APR"});

                    break;
              
              

                     
                default:
                    return Content("Nochange");
            }
            return PartialView("ToolboxView", toolboxVM);
        }

        #endregion
    }
}