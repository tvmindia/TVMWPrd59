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

    public class RequisitionDetailReport
    {
        //public List<Requisition> RequisitionList { get; set; }
        //public List<RequisitionDetail> RequisitionDetailList { get; set; }
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ReqStatus { get; set; }
        public string RequisitionBy { get; set; }
        public Guid EmployeeID { get; set; }
        public Employee Employee { get; set; }
        public string DateFilter { get; set; }
        public Guid MaterialID { get; set; }
        public Material Material { get; set; }
        public string DeliveryStatus { get; set; }

        public Guid ID { get; set; }
        public string ReqNo { get; set; }
        public string Title { get; set; }
        public DateTime ReqDate { get; set; }
        public string ReqDateFormatted { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }

        public Guid ReqID { get; set; }
        public string Description { get; set; }
        public string RequestedQty { get; set; }
        public string ApproximateRate { get; set; }
        public Decimal Discount { get; set; }
        public string POQty { get; set; }
        public string OrderedQty { get; set; }
        public Common Common { get; set; }
        public string ReceivedQty { get; set; }
        public List<RequisitionDetailReport> RequisitionDetailReportList { get; set; }
    }

    public class PurchaseSummaryReport
    {
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public Supplier Supplier { get; set; }
        public Guid SupplierID { get; set; }
        public string Status { get; set; }
        public List<PurchaseOrder> PurchaseOrderList { get; set; }
        public string EmailedYN { get; set; }
        public string DateFilter { get; set; }
    }


    public class PurchaseDetailReport
    {        
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }        
        public string FromDate { get; set; }       
        public string ToDate { get; set; }
        public Supplier Supplier { get; set; }       
        public Guid SupplierID { get; set; }      
        public string Status { get; set; }        
        public string DateFilter { get; set; }
        public Guid MaterialID { get; set; }
        
        public Material Material { get; set; }       
        public string DeliveryStatus { get; set; }
        public Guid ID { get; set; }
        public string PurchaseOrderNo { get; set; }
        public DateTime PurchaseOrderDate { get; set; }
        public string PurchaseOrderDateFormatted { get; set; }
        public string PurchaseOrderStatus { get; set; }
        public string ApprovalStatus { get; set; }
        public ApprovalStatus Approval { get; set; }     
        public string MaterialDesc { get; set; }
        public string UnitCode { get; set; }
        public decimal Qty { get; set; }
        public decimal PrevRcvQty { get; set; }
        public decimal POQty { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
    }




}
