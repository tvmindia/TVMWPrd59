using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductionApp.UserInterface.Models
{
    public class MaterialIssueViewModel
    {
        public Guid ID { get; set; }
        public Guid IssueTo { get; set; }
        public Guid IssuedBy { get; set; }
        public string IssueNo { get; set; }
        public DateTime IssueDate { get; set; }
        public string IssueDateFormatted { get; set; }
        public string GeneralNotes { get; set; }
        public CommonViewModel Common { get; set; }

        public EmployeeViewModel IssueToEmployee { get; set; }
        public EmployeeViewModel IssuedByEmployee { get; set; }
    }
}