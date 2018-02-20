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
        public bool IsUpdate { get; set; }
        public List<MaterialIssueDetail> MaterialIssueDetailList { get; set; }
        public List<MaterialIssue> MaterialIssueList { get; set; }

        public string IssueToEmployeeName { get; set; }
        public string IssuedByEmployeeName { get; set; }
        public Employee Employee { get; set; }
    }

    public class MaterialIssueDetail
    {
        public Guid ID { get; set; }
        public Guid HeaderID { get; set; }
        public Guid MaterialID { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialDec { get; set; }
        public string UnitCode { get; set; }
        public decimal Qty { get; set; }
        public string OtherUnit { get; set; }
        public decimal OtherQty { get; set; }
        public Common Common { get; set; }
    }

    public class MaterialIssueAdvanceSearch
    {
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
    }
}

