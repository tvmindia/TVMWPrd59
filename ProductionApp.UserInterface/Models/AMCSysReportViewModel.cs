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
        [Display(Name = "From Date")]
        public string FromDate { get; set; }
        [Display(Name = "To Date")]
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

    public class RequisitionDetailReportViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "From Date")]
        public string FromDate { get; set; }
        [Display(Name = "To Date")]
        public string ToDate { get; set; }
        [Display(Name = "Requisition Status")]
        public string ReqStatus { get; set; }
        [Display(Name = "Requisition By")]
        public string RequisitionBy { get; set; }
        public Guid EmployeeID { get; set; }
        public EmployeeViewModel Employee { get; set; }
        [Display(Name = "Quick Filter")]
        public string DateFilter { get; set; }
        [Display(Name = "Material")]
        public Guid MaterialID { get; set; }
        public MaterialViewModel Material { get; set; }
        [Display(Name = "Delivery Status")]
        public string DeliveryStatus { get; set; }

        public Guid ID { get; set; }
        public Guid ReqID { get; set; }
        public string Description { get; set; }
        public string RequestedQty { get; set; }
        public string ApproximateRate { get; set; }
        public Decimal Discount { get; set; }
        public string POQty { get; set; }
        public string OrderedQty { get; set; }
        public string ReceivedQty { get; set; }

        public string ReqNo { get; set; }
        public CommonViewModel Common { get; set; }
        public string Title { get; set; }
        public DateTime ReqDate { get; set; }
        public string ReqDateFormatted { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public List<RequisitionDetailReportViewModel> RequisitionDetailReportList { get; set; }
    }

    public class PurchaseSummaryReportViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "From Date")]
        public string FromDate { get; set; }
        [Display(Name = "To Date")]
        public string ToDate { get; set; }
        public SupplierViewModel Supplier { get; set; }
        [Display(Name = "Supplier")]
        public Guid SupplierID { get; set; }
        [Display(Name = "PO Status")]
        public string Status { get; set; }
        [Display(Name = "Quick Filter")]
        public string DateFilter { get; set; }
        [Display(Name = "Email Sent(Y/N)")]
        public string EmailedYN { get; set; }
        public List<PurchaseOrderViewModel> PurchaseOrderList { get; set; }
    }

    public class PurchaseDetailReportViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "From Date")]
        public string FromDate { get; set; }
        [Display(Name = "To Date")]
        public string ToDate { get; set; }
        public SupplierViewModel Supplier { get; set; }
        [Display(Name = "Supplier")]
        public Guid SupplierID { get; set; }
        [Display(Name = "PO Status")]
        public string Status { get; set; }
        [Display(Name = "Quick Filter")]
        public string DateFilter { get; set; }
        [Display(Name = "Material")]       
        public Guid MaterialID { get; set; }
        public MaterialViewModel Material { get; set; }
        [Display(Name = "Approval Status")]
        public string ApprovalStatus { get; set; }
        public ApprovalStatusViewModel Approval { get; set; }
        [Display(Name = "Delivery Status")]
        public string DeliveryStatus { get; set; }
        public Guid ID { get; set; }
        public string PurchaseOrderNo { get; set; }
        public DateTime PurchaseOrderDate { get; set; }
        public string PurchaseOrderDateFormatted { get; set; }
        public string PurchaseOrderStatus { get; set; }      
        public string MaterialDesc { get; set; }
        public string UnitCode { get; set; }
        public decimal Qty { get; set; }
        public decimal PrevRcvQty { get; set; }       
        public decimal POQty { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
    }

    public class PurchaseRegisterReportViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "From Date")]
        public string FromDate { get; set; }
        [Display(Name = "To Date")]
        public string ToDate { get; set; }
        public SupplierViewModel Supplier { get; set; }
        [Display(Name = "Supplier")]
        public Guid SupplierID { get; set; }
        [Display(Name = "Invoice Status")]
        public string InvoiceStatus { get; set; }
        [Display(Name = "Payment Status")]
        public string PaymentStatus { get; set; }
        [Display(Name = "PO Status")]
        public string Status { get; set; }
        [Display(Name = "Quick Filter")]
        public string DateFilter { get; set; }
        public MaterialViewModel Material { get; set; }
        [Display(Name = "Material")]
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
        public List<PurchaseRegisterReportViewModel> PurchaseRegisterReportList { get; set; }

    }


    public class InventoryReorderStatusReportViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }       
        [Display(Name = "Item Status")]
        public int ItemStatus { get; set; }
        [Display(Name = "Material")]
        public Guid MaterialID { get; set; }
        public MaterialViewModel Material { get; set; }
        [Display(Name = "Material Type")]
        public string Code { get; set; }
        public MaterialTypeViewModel MaterialType { get; set; }

        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public Guid ID { get; set; }
        public string Description { get; set; }
        public decimal ReorderQty { get; set; }
        public decimal CurrentStock { get; set; }
        public decimal PODueQty { get; set; }
        public decimal NetAvailableQty { get; set; }
        public decimal ShortFall { get; set; }
        public List<InventoryReorderStatusReportViewModel> InventoryReorderStatusList { get; set; }
    }

    public class StockRegisterReportViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "Material")]
        public Guid MaterialID { get; set; }
        public MaterialViewModel Material { get; set; }
        [Display(Name = "Material Type")]
        public string MaterialTypeCode { get; set; }
        public MaterialTypeViewModel MaterialType { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public Guid ID { get; set; }
        public string Description { get; set; }
        public string TypeDescription { get; set; }
        public decimal CostPrice { get; set; }
        public decimal CurrentStock { get; set; }
        public string UnitCode { get; set; }
        public decimal StockValue { get; set; }
        public List<StockRegisterReportViewModel> StockRegisterReportList { get; set; }
        [Display(Name = "Location")]
        public string Location { get; set; }
    }

    public class StockLedgerReportViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "Material")]
        public Guid MaterialID { get; set; }
        public MaterialViewModel Material { get; set; }
        [Display(Name = "Material Type")]
        public string MaterialTypeCode { get; set; }
        public MaterialTypeViewModel MaterialType { get; set; }
        [Display(Name = "Transaction Type")]
        public string TransactionType { get; set; }
        [Display(Name = "From Date")]
        public string FromDate { get; set; }
        [Display(Name = "To Date")]
        public string ToDate { get; set; }
        [Display(Name = "Quick Filter")]
        public string DateFilter { get; set; }
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
        public List<StockLedgerReportViewModel> StockLedgerReportList { get; set; }
    }

    public class InventoryReOrderStatusFGReportViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "Item Status")]
        public int ItemStatus { get; set; }
        [Display(Name = "Product")]
        public Guid ProductID { get; set; }
        public ProductViewModel Product { get; set; }
        [Display(Name = "Type")]
        public string ProductType { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public Guid ID { get; set; }
        public string Description { get; set; }
        public decimal ReorderQty { get; set; }
        public decimal CurrentStock { get; set; }
        public decimal SalesOrderDueQty { get; set; }
        public decimal NetAvailableQty { get; set; }
        public decimal ShortFall { get; set; }
        public List<InventoryReOrderStatusFGReportViewModel> InventoryReorderStatusFGList { get; set; }
    }

    public class StockRegisterFGReportViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        public Guid ProductID { get; set; }
        public ProductViewModel Product { get; set; }
        [Display(Name = "Type")]
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
        public List<StockRegisterFGReportViewModel> StockRegisterFGReportList { get; set; }
    }

    public class StockLedgerFGReportViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        public Guid ProductID { get; set; }
        public ProductViewModel Product { get; set; }
        [Display(Name = "Type")]
        public string ProductType { get; set; }
        [Display(Name = "From Date")]
        public string FromDate { get; set; }
        [Display(Name = "To Date")]
        public string ToDate { get; set; }
        [Display(Name = "Quick Filter")]
        public string DateFilter { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        [Display(Name = "Transaction Type")]
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
        public List<StockLedgerFGReportViewModel> StockLedgerFGReportList { get; set; }
    }

    public class ProductStageWiseStockReportViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "Product")]
        public Guid ProductID { get; set; }
        public ProductViewModel Product { get; set; }
        public string Description { get; set; }
        public string Stage { get; set; }
        public decimal CurrentStock { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public List<ProductStageWiseStockReportViewModel> ProductStagewiseReportList { get; set; }

    }

    public class SalesAnalysisReportViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "From Date")]
        public string FromDate { get; set; }
        [Display(Name = "To Date")]
        public string ToDate { get; set; }
        [Display(Name = "Product")]
        public Guid ProductID { get; set; }
        [Display(Name = "Quick Filter")]
        public string DateFilter { get; set; }
        public bool IsInvoicedOnly { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public Guid ID { get; set; }
      
        public List<SalesAnalysisReportViewModel> SalesAnalysisReportList { get; set; }

    }
    //MovementAnalysisReportViewModel
    public class MovementAnalysisReportViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "From Date")]
        public string FromDate { get; set; }
        [Display(Name = "To Date")]
        public string ToDate { get; set; }

        [Display(Name = "Month Range")]
        public string MonthFilter { get; set; }
        [Display(Name = "Sales Person")]
        public Guid EmployeeID { get; set; }
        [Display(Name = "Product")]
        public Guid ProductID { get; set; } 
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public Guid ID { get; set; }

        public List<SelectListItem> EmployeeSelectList { get; set; }
        public List<SelectListItem> ProductSelectList { get; set; }
        public List<MovementAnalysisReportViewModel> MovementAnalysisReportList { get; set; }

    }


}
