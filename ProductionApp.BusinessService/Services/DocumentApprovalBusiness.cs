using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductionApp.DataAccessObject.DTO;
using System.Data;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.IO;

namespace ProductionApp.BusinessService.Services
{
    public class DocumentApprovalBusiness: IDocumentApprovalBusiness
    {
        private IDocumentApprovalRepository _documentApprovalRepository;
        private IMailBusiness _mailBusiness;
        private ICommonBusiness _commonBusiness;
        Common _common = new Common();

        public DocumentApprovalBusiness(IDocumentApprovalRepository documentApprovalRepository, IMailBusiness mailBusiness, ICommonBusiness commonBusiness)
        {
            _documentApprovalRepository = documentApprovalRepository;
            _mailBusiness = mailBusiness;
            _commonBusiness = commonBusiness;
        }

        public List<ApprovalHistory> GetApprovalHistory(Guid DocumentID, string DocumentTypeCode)
        {
          return  _documentApprovalRepository.GetApprovalHistory(DocumentID, DocumentTypeCode);
        }

        public List<DocumentApproval> GetAllDocumentsPendingForApprovals(DocumentApprovalAdvanceSearch documentApprovalAdvanceSearch)
        {
            return _documentApprovalRepository.GetAllDocumentsPendingForApprovals(documentApprovalAdvanceSearch);
        }

        public DataTable GetDocumentSummary(Guid DocumentID, string DocumentTypeCode)
        {
            return _documentApprovalRepository.GetDocumentSummary(DocumentID, DocumentTypeCode);
        }

        public object ApproveDocument(Guid ApprovalLogID, Guid DocumentID, string DocumentTypeCode)
        {
            return _documentApprovalRepository.ApproveDocument(ApprovalLogID, DocumentID, DocumentTypeCode);
        }

        public object RejectDocument(Guid ApprovalLogID, Guid DocumentID, string DocumentTypeCode, string Remarks)
        {
            return _documentApprovalRepository.RejectDocument(ApprovalLogID, DocumentID, DocumentTypeCode, Remarks);
        }

        public object ValidateDocumentsApprovalPermission(string LoginName, Guid DocumentID, string DocumentTypeCode)
        {
            return _documentApprovalRepository.ValidateDocumentsApprovalPermission(LoginName, DocumentID, DocumentTypeCode);

        }

        public List<DocumentApprover> GetApproversByDocType(string docType)
        {
            return _documentApprovalRepository.GetApproversByDocType(docType);
        }

        public object SendDocForApproval(Guid DocumentID, string DocumentTypeCode, string approvers, string CreatedBy, DateTime CreatedDate)
        {
            object result= _documentApprovalRepository.SendDocForApproval(DocumentID, DocumentTypeCode, approvers, CreatedBy, CreatedDate);
          
            return result;
        }

        public object ReSendDocForApproval(Guid documentID, string documentTypeCode, Guid latestApprovalID, string createdBy, DateTime createdDate)
        {
            return _documentApprovalRepository.ReSendDocForApproval(documentID, documentTypeCode, latestApprovalID, createdBy, createdDate);
        }



        public async Task<bool> SendApprolMails(Guid documentID, string documentType) {
            try
            {
                DocumentApprovalMailDetail docAprovalMailDetail = new DocumentApprovalMailDetail();
                bool sendsuccess;
                string link;
                string loginLink= WebConfigurationManager.AppSettings["AppURL"];
                Settings settings = new Settings();
                docAprovalMailDetail = _documentApprovalRepository.GetApprovalMailDetails(documentID, documentType);
                if (docAprovalMailDetail != null) {
                    link= WebConfigurationManager.AppSettings["AppURL"]  + "/DocumentApproval/ApproveDocument?code=APR&ID="+ docAprovalMailDetail.ApprovalID + "&DocType="+ documentType +"&DocID="+ documentID ;
                    if (docAprovalMailDetail.Status == "PENDING") {

                        string mailBody= File.ReadAllText(HttpContext.Current.Server.MapPath("~/Content/MailTemplate/SendForApproval.html"));


                        Mail mail = new Mail();
                        mail.Body = mailBody.Replace("$DocumentType$", docAprovalMailDetail.DocumentType).Replace("$DocumentNo$", docAprovalMailDetail.DocumentNo).Replace("$DocumentOwner$", docAprovalMailDetail.DocumentOwner).Replace("$Name$", docAprovalMailDetail.NextApprover).Replace("$ApproveLink$", link).Replace("$Date$", _common.GetCurrentDateTime().ToString(settings.DateFormat)).Replace("$LoginLink$",loginLink);
                        mail.Subject = "Document Pending For Approval (" + docAprovalMailDetail.DocumentNo + ")";
                        mail.To = docAprovalMailDetail.NextApproverEmail;
                        sendsuccess = await _mailBusiness.MailSendAsync(mail);
                    }

                    if (docAprovalMailDetail.Status == "REJECTED")
                    {
                        string mailBody = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Content/MailTemplate/DocumentRejected.html"));
                        Mail mail = new Mail();
                        mail.Body = mailBody.Replace("$DocumentType$", docAprovalMailDetail.DocumentType).Replace("$DocumentNo$", docAprovalMailDetail.DocumentNo).Replace("$Name$", docAprovalMailDetail.DocumentOwner).Replace("$Date$", _common.GetCurrentDateTime().ToString(settings.DateFormat)).Replace("$LoginLink$", loginLink).Replace("$Approver$", docAprovalMailDetail.NextApprover).Replace("$Remarks$", docAprovalMailDetail.Remarks);
                        mail.Subject = "Document Rejected for ammedment (" + docAprovalMailDetail.DocumentNo + ")";
                        mail.To = docAprovalMailDetail.DocumnetOwnerMail;
                        sendsuccess = await _mailBusiness.MailSendAsync(mail);
                    }

                    if (docAprovalMailDetail.Status == "APPROVED")
                    {
                        string mailBody = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Content/MailTemplate/DocumentApproved.html"));
                        Mail mail = new Mail();
                        mail.Body =  mailBody.Replace("$DocumentType$", docAprovalMailDetail.DocumentType).Replace("$DocumentNo$", docAprovalMailDetail.DocumentNo).Replace("$Name$", docAprovalMailDetail.DocumentOwner).Replace("$Date$", _common.GetCurrentDateTime().ToString(settings.DateFormat)).Replace("$LoginLink$", loginLink).Replace("$Remarks$", docAprovalMailDetail.Remarks);
                        mail.Subject = "Document Approved (" + docAprovalMailDetail.DocumentNo + ")";
                        mail.To = docAprovalMailDetail.DocumnetOwnerMail;
                        sendsuccess = await _mailBusiness.MailSendAsync(mail);
                    }

                }


                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }
    }
}
