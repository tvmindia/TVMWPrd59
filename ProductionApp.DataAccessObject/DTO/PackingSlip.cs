using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    public class PackingSlip
    {
        public Guid ID { get; set; }
        public string SlipNo { get; set; }
        public DateTime Date { get; set; }
        public Guid? PackedBy { get; set; }
        public Guid SalesOrderID { get; set; }
        public DateTime? IssueToDispatchDate { get; set; }
        public decimal? TotalPackageWeight { get; set; }
        public DateTime? DispatchedDate { get; set; }
        public Guid DispatchedBy { get; set; }
        public string VehiclePlateNo { get; set; }
        public string DriverName { get; set; }
        public decimal? CheckedPackageWeight { get; set; }
        public string ReceivedBy { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string PackingRemarks { get; set; }
        public string DispatchRemarks { get; set; }
        //Additional properties
        public Common Common { get; set; }
        public Employee Employee { get; set; }
        public SalesOrder SalesOrder { get; set; }
        public PackingSlipDetail PackingSlipDetail { get; set; }
        public List<PackingSlipDetail> PackingSlipDetailList { get; set; }
        public string DetailXML { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public string DateFormatted { get; set; }
        public string DispatchedDateFormatted { get; set; }
        public string IssueToDispatchDateFormatted { get; set; }
        public string ReceivedDateFormatted { get; set; }
        public string PackedByEmployeeName { get; set; }
        public string DispatchedByEmployeeName { get; set; }
        public bool IsUpdate { get; set; }
        public string DetailJSON { get; set; }
        public List<PackingSlip> PackingSlipList { get; set; }
        public string BaseURL { get; set; }        
    }

    public class PackingSlipDetail
    {
        public Guid ID { get; set; }
        public Guid PackingSlipID { get; set; }
        public Guid ProductID { get; set; }
        public decimal Qty { get; set; }
        public decimal Weight { get; set; }
        //Additional properties
        public string Name { get; set; }
    }
    public class PackingSlipAdvanceSearch
    {
        
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }

        
        public string PackingFromDate { get; set; }
        public string PackingToDate { get; set; }
        public string DispatchedFromDate { get; set; }
        public string DispatchedToDate { get; set; }
        public Guid PackedBy { get; set; }
        public Guid DispatchedBy { get; set; }
        public string PackedByEmployeeName { get; set; }
        public string DispatchedByEmployeeName { get; set; }
        public Employee Employee { get; set; }
    }
}
