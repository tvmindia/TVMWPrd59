using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    public class MaterialIssue
    {
        public Guid ID { get; set; }
        public Guid IssueTo { get; set; }
        public Guid IssuedBy { get; set; }
        public string IssueNo { get; set; }
        public DateTime IssueDate { get; set; }
        public string IssueDateFormatted { get; set; }
        public string GeneralNotes { get; set; }
        public Common Common { get; set; }

        public List<MaterialIssue> MaterialIssueList { get; set; }

        public string IssueToEmployeeName { get; set; }
        public string IssuedByEmployeeName { get; set; }
    }
}
