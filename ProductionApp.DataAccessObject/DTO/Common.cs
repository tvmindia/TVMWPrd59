using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace ProductionApp.DataAccessObject.DTO
{
    public class Common
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateString { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedDateString { get; set; }        
        public DateTime GetCurrentDateTime()
        {
            string tz = System.Web.Configuration.WebConfigurationManager.AppSettings["TimeZone"];
            DateTime DateNow = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);
            return (TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateNow, tz));
        }
    }

    public class AppUA
    {
        public string UserName { get; set; }
        public DateTime LoginDateTime { get; set; }
        public Guid AppID { get; set; }
        public string RolesCSV { get; set; }
    }

    public class Settings
    {

        public string DateFormat = "dd-MMM-yyyy";
    }


    public class AppConst
    {
        #region Messages

        private List<AppConstMessage> constMessage = new List<AppConstMessage>();

        public AppConst()
        {
            constMessage.Add(new AppConstMessage("Test message", "DF8D1", "ERROR"));
            constMessage.Add(new AppConstMessage(FKviolation, "FK_Exec", "ERROR"));
            //
        }

        public string LoginAndEmailExist
        {
            get { return "Login or Email exist! "; }
        }

        public string ApprovalSuccess
        {
            get { return "Document approved! "; }
        }
        public string ApprovalFailure
        {
            get { return "Approval failed! "; }
        }
        public string SendForApproval
        {
            get { return "Document sent for approval! "; }
        }
        public string SendForApprovalFailure
        {
            get { return "Sending for approval failed! "; }
        }

        public string RejectSuccess
        {
            get { return "Document rejected! "; }
        }
        public string RejectFailure
        {
            get { return "Rejection failed! "; }
        }
        public string InsertFailure
        {
            get { return "Insertion not successfull! "; }
        }

        public string InsertSuccess
        {
            get { return "Values saved successfully ! "; }
        }

        public string UpdateFailure
        {
            get { return "Updation not successfull! "; }
        }

        public string UpdateSuccess
        {
            get { return "Updation successfull! "; }
        }

        public string NotificationSuccess
        {
            get { return "Notification send successfully ! "; }
        }


        public string DeleteFailure
        {
            get { return "Deletion not successfull! "; }
        }
        public string DeleteSuccess
        {
            get { return "Deletion successfull! "; }
        }
        public string FKviolation
        {
            get { return "Deletion not successfull!-already in use"; }
        }
        public string Duplicate
        {
            get { return "Already exist.."; }
        }
        
        public string NoItems
        {
            get { return "No items"; }
        }
        
        public string PasswordError
        {
            get { return "Password is wrong"; }
        }
        public string MailFailure
        {
            get { return "Mail sending failed! "; }
        }

        public string MailSuccess
        {
            get { return "Mail send successfully ! "; }
        }

        public AppConstMessage GetMessage(string messageCode)
        {
            AppConstMessage result = new AppConstMessage(messageCode, messageCode, "ERROR");

            try
            {
                foreach (AppConstMessage c in constMessage)
                {
                    if (c.Code == messageCode)
                    {
                        result = c;
                        break;
                    }

                }

            }
            catch (Exception)
            {


            }
            return result;



        }


        #endregion Messages

        #region Strings
        public string AppUser
        {
            get { return "App User"; }
        }
        #endregion
        
    }

    public class AppConstMessage
    {
        public string Message;
        public string Code;
        public string Type;
        public AppConstMessage(string message, string code, string type)
        {
            Message = (code == "" ? "" :  message);
           // Message = (code == "" ? "" : code + "-") + message;
            Code = code;
            Type = type;

        }
    }
    public class FileUpload
    {
        public Guid ID { get; set; }
        public Guid ParentID { get; set; }
        public string ParentType { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileSize { get; set; }
        public string AttachmentURL { get; set; }
        public Common CommonObj { get; set; }
    }



    //--need to move this to dynamic UI ----
    public class IncomeExpenseSummary
    {
        public string Month { get; set; }
        public int MonthCode { get; set; }
        public int Year { get; set; }
        public decimal InAmount { get; set; }
        public decimal ExAmount { get; set; }
    }


    public class ProductionSummary
    {
        public string Month { get; set; }
        public int MonthCode { get; set; }
        public int Year { get; set; }
        public decimal Material { get; set; }
        public decimal Product { get; set; }
        public decimal InProduction { get; set; }
        public decimal Damage { get; set; }
    }

    public class DayBook
    {
        public string TransactionName { get; set; }
        public int Count { get; set; }
        public List<DayBook> DayBookList { get; set; }
        public string dayBookDate { get; set; }
        public string TransactionCode { get; set; }
        public string SearchTerm { get; set; }

    }

}