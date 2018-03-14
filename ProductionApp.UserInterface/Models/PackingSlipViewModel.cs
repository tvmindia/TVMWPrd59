using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductionApp.UserInterface.Models
{
    public class PackingSlipViewModel
    {
        public Guid ID { get; set; }
        public string SlipNo { get; set; }
        public DateTime Date { get; set; }
        [Display(Name = "Packed By")]
        public Guid PackedBy { get; set; }
        [Display(Name = "Sales Order")]
        public Guid SalesOrderID { get; set; }
        [Display(Name = "Issued To Dispatch Date")]
        public DateTime IssueToDispatchDate { get; set; }
        [Display(Name = "Total Package Weight(KGs")]
        public decimal TotalPackageWeight { get; set; }
        public DateTime DispatchedDate { get; set; }
        [Display(Name = "Dispatched By")]
        public Guid DispatchedBy { get; set; }
        [Display(Name = "Vehicle Plate No.")]
        public string VehiclePlateNo { get; set; }
        [Display(Name = "Driver Name")]
        public string DriverName { get; set; }
        [Display(Name = "Checked Package Weight(KGs")]
        public decimal CheckedPackageWeight { get; set; }
        [Display(Name = "Received By")]
        public string ReceivedBy { get; set; }
        [Display(Name = "Received Date")]
        public DateTime ReceivedDate { get; set; }
        public string PackingRemarks { get; set; }
        [Display(Name = "Remarks By Dispatch Section (If Any)")]
        [DataType(DataType.MultilineText)]
        public string DispatchRemarks { get; set; }
        //Additional properties
        public CommonViewModel Common { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public SalesOrderViewModel SalesOrder { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        [Display(Name = "Date")]
        public string DateFormatted { get; set; }
        [Display(Name = "Dispatched Date")]
        public string DispatchedDateFormatted { get; set; }
        public string IssueToDispatchDateFormatted { get; set; }
        public string ReceivedDateFormatted { get; set; }
        public string PackedByEmployeeName { get; set; }
        public string DispatchedByEmployeeName { get; set; }
        public bool IsUpdate { get; set; }
    }
    public class PackingSlipDetailViewModel
    {
        public Guid ID { get; set; }
        public Guid PackingSlipID { get; set; }
        public Guid ProductID { get; set; }
        public decimal Qty { get; set; }
        public decimal Weight { get; set; }
    }
    public class PackingSlipAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }

        [Display(Name = "Packing Date From")]
        public string PackingFromDate { get; set; }
        [Display(Name = "Packing Date To")]
        public string PackingToDate { get; set; }
        [Display(Name = "Dispatched Date From")]
        public string DispatchedFromDate { get; set; }
        [Display(Name = "Dispatched Date To")]
        public string DispatchedToDate { get; set; }
        [Display(Name = "Packed By")]
        public Guid PackedBy { get; set; }
        [Display(Name = "Dispatched By" )]
        public Guid DispatchedBy { get; set; }
        public string PackedByEmployeeName { get; set; }
        public string DispatchedByEmployeeName { get; set; }
        public EmployeeViewModel Employee { get; set; }
    }
}