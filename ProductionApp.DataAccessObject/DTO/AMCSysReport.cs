using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    public class AMCSysReport
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
        public List<AMCSysReport> AMCSysReportList { get; set; }
        public string SearchTerm { get; set; }

    }

    public class RequisitionSummaryReport
    {       
        public List<Requisition> RequisitionList { get; set; }       
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }       
        public string FromDate { get; set; }       
        public string ToDate { get; set; }        
        public string ReqStatus { get; set; }        
        public string RequisitionBy { get; set; }
        public Guid EmployeeID { get; set; }
        public Employee Employee { get; set; }       
        public string DateFilter { get; set; }
        
    }
}
