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

        public ActionResult ApproveDocument(string code)
        {
            ViewBag.SysModuleCode = code;
            return View();
        }

        public ActionResult ApprovalHistory()
        {
            ApprovalHistoryViewModel approvalHistoryVM = new ApprovalHistoryViewModel();
            return PartialView("_ApprovalHistory", approvalHistoryVM);
        }
        public ActionResult DocumentSummary()
        {
            DocumentSummaryViewModel documentSummaryVM = new DocumentSummaryViewModel();
            return PartialView("_DocumentSummary", documentSummaryVM);
        }


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