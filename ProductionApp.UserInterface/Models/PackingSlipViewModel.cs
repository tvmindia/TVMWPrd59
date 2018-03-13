using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductionApp.UserInterface.Models
{
    public class PackingSlipViewModel
    {
        public Guid ID { get; set; }
        public string SlipNo { get; set; }
        public DateTime Date { get; set; }
        public Guid PackedBy { get; set; }
        public Guid SalesOrderID { get; set; }
        public DateTime IssueToDispatchDate { get; set; }
        public decimal TotalPackageWeight { get; set; }
        public DateTime DispatchedDate { get; set; }
        public Guid DispatchedBy { get; set; }
        public string VehiclePlateNo { get; set; }
        public string DriverName { get; set; }
        public decimal CheckedPackageWeight { get; set; }
        public string ReceivedBy { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string PackingRemarks { get; set; }
        public string DispatchRemarks { get; set; }
        //Additional properties
        public CommonViewModel Common { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
    }
    public class PackingSlipDetailViewModel
    {
        public Guid ID { get; set; }
        public Guid PackingSlipID { get; set; }
        public Guid ProductID { get; set; }
        public decimal Qty { get; set; }
        public decimal Weight { get; set; }
    }
}