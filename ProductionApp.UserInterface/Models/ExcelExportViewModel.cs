using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductionApp.UserInterface.Models
{
    public class ExcelExportViewModel
    {
        public string AdvanceSearch { get; set; }
        public string DocumentType { get; set; }
        public string[] TableHeaderColumns { get; set; }
        public string[] TableHeaderColumnsWidth { get; set; }
    }
}