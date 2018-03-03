using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    public class SendForApproval
    {
        public Guid? ApproverID { get; set; }
        public Guid? UserID { get; set; }
        public string UserName { get; set; }
        public string EmailID { get; set; }
        public int ApproverLevel { get; set; }
        public string ApproverCSV { get; set; }
    }
}
