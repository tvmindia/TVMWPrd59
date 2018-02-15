using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    public class Requisition
    {
        public Guid ID { get; set; }
        public string ReqNo { get; set; }
        public string Title { get; set; }
        public DateTime ReqDate { get; set; }
        public string ReqDateFormatted { get; set; }
        public string ReqStatus { get; set; }
        public string RequisitionBy { get; set; }
        public Guid LatestApprovalID { get; set; }
        public int LatestApprovalStatus { get; set; }
        public bool IsFinalApproved { get; set; }
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public Common Common { get; set; }
        public RequisitionDetail RequisitionDetail { get; set; }
    }


    public class RequisitionDetail
    {
        public Guid ID { get; set; }
        public Guid ReqID { get; set; }
        public Guid MaterialID { get; set; }
        public string Description { get; set; }
        public string RequestedQty { get; set; }
        public string CurrentStock { get; set; }
        public string ApproximateRate { get; set; }
        public Common Common { get; set; }
    }

    public class RequisitionAdvanceSearch
    {

        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }

    }
}
