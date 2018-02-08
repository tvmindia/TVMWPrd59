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
            get { return "Login or Email Exist! "; }
        }
        public string InsertFailure
        {
            get { return "Insertion Not Successfull! "; }
        }

        public string InsertSuccess
        {
            get { return "Values Saved Successfully ! "; }
        }

        public string UpdateFailure
        {
            get { return "Updation Not Successfull! "; }
        }

        public string UpdateSuccess
        {
            get { return "Updation Successfull! "; }
        }

        public string NotificationSuccess
        {
            get { return "Notification Send Successfully ! "; }
        }


        public string DeleteFailure
        {
            get { return "Deletion Not Successfull! "; }
        }
        public string DeleteSuccess
        {
            get { return "Deletion Successfull! "; }
        }
        public string FKviolation
        {
            get { return "Deletion Not Successfull!-Already In Use"; }
        }
        public string Duplicate
        {
            get { return "Already Exist.."; }
        }
        
        public string NoItems
        {
            get { return "No items"; }
        }
        
        public string PasswordError
        {
            get { return "Password is wrong"; }
        }
        public AppConstMessage GetMessage(string messageCode)
        {
            AppConstMessage result = new AppConstMessage(messageCode, "", "ERROR");

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
            Message = (code == "" ? "" : code + "-") + message;
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

}