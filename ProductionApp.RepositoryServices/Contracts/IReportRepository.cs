using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.RepositoryServices.Contracts
{
   public interface IReportRepository
    {
        List<AMCSysReport> GetAllReport(string searchTerm);
        List<Requisition> GetRequisitionSummaryReport(RequisitionSummaryReport requisitionSummaryReport);
        List<RequisitionDetailReport>GetRequisitionDetailReport(RequisitionDetailReport requisitionDetailReport);
        List<PurchaseOrder> GetPurchaseSummaryReport(PurchaseSummaryReport purchaseSummaryReport);
        List<PurchaseDetailReport> GetPurchaseDetailReport(PurchaseDetailReport purchaseDetailReport);
        List<PurchaseRegisterReport> GetPurchaseRegisterReport(PurchaseRegisterReport purchaseRegisterReport);
        List<InventoryReorderStatusReport> GetInventoryReorderStatusReport(InventoryReorderStatusReport inventoryReOrderStatusReport);
        List<StockRegisterReport> GetStockRegisterReport(StockRegisterReport stockRegisterReport);
        List<StockLedgerReport> GetStockLedgerReport(StockLedgerReport stockLedgerReport);
        List<InventoryReOrderStatusFGReport> GetInventoryReOrderStatusFGReport(InventoryReOrderStatusFGReport inventoryReOrderStatusFGReport);
        List<StockRegisterFGReport> GetStockRegisterFGReport(StockRegisterFGReport stockRegisterFGReport);
        List<StockLedgerFGReport> GetStockLedgerFGReport(StockLedgerFGReport stockLedgerFGReport);
    }
}
