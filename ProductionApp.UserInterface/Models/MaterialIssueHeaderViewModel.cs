using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductionApp.UserInterface.Models
{
    public class MaterialIssueHeaderViewModel
    {
        public Guid ID { get; set; }
        public Guid IssueTo { get; set; }
        public Guid IssuedBy { get; set; }
        public string IssueNo { get; set; }
        public string IssueDate { get; set; }
        public string GeneralNotes { get; set; }
        public CommonViewModel common { get; set; }
    }
}