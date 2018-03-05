using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    public class Mail
    {
            /*--------General properies form Email----------*/
            #region General
            public string To { get; set; }
            public string From { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }
            public bool IsBodyHtml { get; set; }
            #endregion General
            /*--------General properies form Email----------*/

            public string CustomerName { get; set; }
            public string CustomerEmail { get; set; }
            public string OrderComment { get; set; }
            public int OrderNo { get; set; }
            public int OrderID { get; set; }
            public string TemplateString { get; set; }
            public string MailSubject { get; set; }
        
    }
}
