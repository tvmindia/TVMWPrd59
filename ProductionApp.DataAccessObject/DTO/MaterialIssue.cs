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
        public string IssueDate { get; set; }
        public string GeneralNotes { get; set; }
        public Common common { get; set; }
    }
}
