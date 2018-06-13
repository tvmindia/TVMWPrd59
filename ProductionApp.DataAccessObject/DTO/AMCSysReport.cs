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

    public class  PurchaseRegisterReport
    {
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public Supplier Supplier { get; set; }
        public Guid SupplierID { get; set; }
        public string Status { get; set; }
        public string InvoiceStatus{ get; set; }
        public string PaymentStatus { get; set; }
        public string DateFilter { get; set; }
        public Guid MaterialID { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public Guid ID { get; set; }
        public string PurchaseOrderNo { get; set; }
        public DateTime PurchaseOrderDate { get; set; }
        public string PurchaseOrderDateFormatted { get; set; }
        public decimal GSTPerc { get; set; }
        public decimal GSTAmt { get; set; }
        public decimal Discount { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal InvoicedAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal TaxableAmount { get; set; }
        public List<PurchaseRegisterReport> PurchaseRegisterReportList { get; set; }
    }

    public class InventoryReorderStatusReport
    {
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }        
        public int ItemStatus { get; set; }       
        public int TotalCount { get; set; }
        public Guid MaterialID { get; set; }
        public Material Material { get; set; }

        public string Code { get; set; }       
        public MaterialType MaterialType { get; set; }
        public int FilteredCount { get; set; }
        public Guid ID { get; set; }
        public string Description { get; set; }
        public decimal ReorderQty { get; set; }
        public decimal CurrentStock { get; set; }
        public decimal PODueQty { get; set; }
        public decimal NetAvailableQty { get; set; }
        public decimal ShortFall { get; set; }
        public List<InventoryReorderStatusReport> InventoryReorderStatusList { get; set; }
    }


    public class StockRegisterReport
    {
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public Guid MaterialID { get; set; }
        public Material Material { get; set; }
        public string MaterialTypeCode { get; set; }
        public MaterialType MaterialType { get; set; }        
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public Guid ID { get; set; }
        public string Description { get; set; }
        public string TypeDescription { get; set; }
        public decimal CostPrice { get; set; }
        public decimal CurrentStock { get; set; }
        public string UnitCode { get; set; }
        public decimal StockValue { get; set; }
        public List<StockRegisterReport> StockRegisterReportList { get; set; }
        public string Location { get; set; }
    }

    public class  StockLedgerReport
    {
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public Guid MaterialID { get; set; }
        public Material Material { get; set; }
        public string MaterialTypeCode { get; set; }
        public MaterialType MaterialType { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string DateFilter { get; set; }
        public string TransactionType { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public string Description { get; set; }
        public string UnitCode { get; set; }
        public decimal OpeningStock { get; set; }
        public decimal ClosingStock { get; set; }       
        public DateTime TransactionDate { get; set; }
        public string TransactionDateFormatted { get; set; }
        public string DocumentNo { get; set; }
        public decimal StockIn { get; set; }
        public decimal StockOut { get; set; }
        public List<StockLedgerReport> StockLedgerReportList { get; set; }
    }

    public class InventoryReOrderStatusFGReport
    {
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public int ItemStatus { get; set; }
        public int TotalCount { get; set; }
        public Guid ProductID { get; set; }
        public Product Product { get; set; }
        public string ProductType { get; set; }
        public int FilteredCount { get; set; }
        public Guid ID { get; set; }
        public string Description { get; set; }
        public decimal ReorderQty { get; set; }
        public decimal CurrentStock { get; set; }
        public decimal SalesOrderDueQty { get; set; }
        public decimal NetAvailableQty { get; set; }
        public decimal ShortFall { get; set; }
        public List<InventoryReOrderStatusFGReport> InventoryReorderStatusFGList { get; set; }
    }

    public class StockRegisterFGReport
    {
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public Guid ProductID { get; set; }
        public Product Product { get; set; }
        public string ProductType { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public Guid ID { get; set; }
        public string Description { get; set; }
        public string TypeDescription { get; set; }
        public decimal CostPrice { get; set; }
        public decimal CurrentStock { get; set; }
        public string UnitCode { get; set; }
        public decimal CostAmount { get; set; }
        public decimal SellingRate { get; set; }
        public decimal SellingAmount { get; set; }
        public string StockCostAmount { get; set; }
        public string StockSellingAmount { get; set; }        
        public List<StockRegisterFGReport> StockRegisterFGReportList { get; set; }
    }

    public class StockLedgerFGReport
    {
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public Guid ProductID { get; set; }
        public Product Product { get; set; }
        public string ProductType{ get; set; }      
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string DateFilter { get; set; }       
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public string TransactionType { get; set; }
        public string Description { get; set; }
        public string UnitCode { get; set; }
        public decimal OpeningStock { get; set; }
        public decimal ClosingStock { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionDateFormatted { get; set; }
        public string DocumentNo { get; set; }
        public decimal StockIn { get; set; }
        public decimal StockOut { get; set; }      
        public List<StockLedgerFGReport> StockLedgerFGReportList { get; set; }
    }

    public class ProductStageWiseStockReport
    {
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public Guid ProductID { get; set; }
        public Product Product { get; set; }
        public string Description { get;set;}
        public string Stage { get; set; }
        public decimal CurrentStock { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public List<ProductStageWiseStockReport> ProductStagewiseReportList { get; set; }

    }
}
