using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserInterface.Models;

namespace ProductionApp.UserInterface.Models
{
    public class AMCSysReportViewModel
    {      
        public Guid ID { get; set; }
        public string ReportName { get; set; }
        public string ReportDescription { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string ReportGroup { get; set; }
        public int GroupOrder { get; set; }
        public string SPName { get; set; }
        public string SQL { get; set; }
        public int ReportOrder { get; set; }
        public List<AMCSysReportViewModel> AMCSysReportList { get; set; }
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
    }
    
    public class RequisitionSummaryReportViewModel
    {
        public List<RequisitionViewModel> RequisitionList { get; set; }
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "FromDate")]
        public string FromDate { get; set; }
        [Display(Name = "ToDate")]
        public string ToDate { get; set; }
        [Display(Name = "Requisition Status")]
        public string ReqStatus { get; set; }
        [Display(Name = "Requisition By")]
        public string RequisitionBy { get; set; }
        public Guid EmployeeID { get; set; }
        public EmployeeViewModel Employee { get; set; }
        [Display(Name = "Quick Filter")]
        public string DateFilter { get; set; }      
    }
  
}