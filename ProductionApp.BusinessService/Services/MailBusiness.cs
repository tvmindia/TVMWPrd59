using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Services
{
    public class MailBusiness : IMailBusiness
    {
        string EmailFromAddress = System.Web.Configuration.WebConfigurationManager.AppSettings["EmailFromAddress"];
        string host = System.Web.Configuration.WebConfigurationManager.AppSettings["SMTP-host"];
        string smtpUserName = System.Web.Configuration.WebConfigurationManager.AppSettings["SMTP-UserName"];
        string smtpPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["SMTP-Password"];
        string AliasName = System.Web.Configuration.WebConfigurationManager.AppSettings["SMTP-AliasName"];
        string port = System.Web.Configuration.WebConfigurationManager.AppSettings["Port"];

        public async Task<bool> MailSendAsync(Mail mailObj)
        {
            try
            {
                using (var mail = new MailMessage(new MailAddress(EmailFromAddress, AliasName), new MailAddress(mailObj.To)))
                {
                    mail.Subject = mailObj.Subject;
                    mail.Body = mailObj.Body;
                    mail.IsBodyHtml = true;
                    using (var client = new SmtpClient())
                    {
                        client.UseDefaultCredentials = false;
                        client.Host = host;
                        client.Port = int.Parse(port);
                        client.EnableSsl = true;
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.Credentials = new NetworkCredential(smtpUserName, smtpPassword);
                        await client.SendMailAsync(mail);
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
