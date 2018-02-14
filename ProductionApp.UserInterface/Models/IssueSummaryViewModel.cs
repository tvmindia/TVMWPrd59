using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProductionApp.UserInterface.Models;

namespace ProductionApp.UserInterface.Models
{
    public class IssueSummaryViewModel
    {
        public string URL { get; set; }
        public string Title { get; set; }
        public string Color { get; set; }
        public List<MaterialIssueViewModel> MaterialIssueHeaderList { get; set; }
    }
}