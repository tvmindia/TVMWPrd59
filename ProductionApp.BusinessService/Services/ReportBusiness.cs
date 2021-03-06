﻿using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ProductionApp.BusinessService.Services
{
    public class ReportBusiness : IReportBusiness
    {
        private IReportRepository _reportRepository;
        private ICommonBusiness _commonBusiness;
        public ReportBusiness(IReportRepository reportRepository,ICommonBusiness commonBusiness)
        {
            _reportRepository = reportRepository;
            _commonBusiness = commonBusiness;
        }

        #region GetAllReports
        public List<AMCSysReport> GetAllReport(string searchTerm)
        {
            return _reportRepository.GetAllReport(searchTerm);
        }
        #endregion GetAllReports

        #region GetRequisitionSummaryReport
        public List<Requisition> GetRequisitionSummaryReport(RequisitionSummaryReport requisitionSummaryReport)
        {
            return _reportRepository.GetRequisitionSummaryReport(requisitionSummaryReport);
        }
        #endregion GetRequisitionSummaryReport


        #region GetRequisitionDetailReport
        public List<RequisitionDetailReport> GetRequisitionDetailReport(RequisitionDetailReport requisitionDetailReport)
        {
            return _reportRepository.GetRequisitionDetailReport(requisitionDetailReport);
        }
        #endregion GetRequisitionDetailReport


        #region GetPurchaseSummaryReport
        public List<PurchaseOrder>GetPurchaseSummaryReport(PurchaseSummaryReport purchaseSummaryReport)
        {
            return _reportRepository.GetPurchaseSummaryReport(purchaseSummaryReport);
        }
        #endregion GetPurchaseSummaryReport


        #region GetPurchaseDetailReport
        public List<PurchaseDetailReport> GetPurchaseDetailReport(PurchaseDetailReport purchaseDetailReport)
        {
            return _reportRepository.GetPurchaseDetailReport(purchaseDetailReport);
        }
        #endregion GetPurchaseDetailReport

        #region GetPurchaseRegisterReport
        public List<PurchaseRegisterReport>GetPurchaseRegisterReport(PurchaseRegisterReport purchaseRegisterReport)
        {
            return _reportRepository.GetPurchaseRegisterReport(purchaseRegisterReport);
        }
        #endregion GetPurchaseRegisterReport


        #region GetInventoryReorderStatusReport
        public List<InventoryReorderStatusReport> GetInventoryReorderStatusReport(InventoryReorderStatusReport inventoryReOrderStatusReport)
        {
            return _reportRepository.GetInventoryReorderStatusReport(inventoryReOrderStatusReport);
        }
        #endregion GetInventoryReorderStatusReport


        #region GetStockRegisterReport
        public List<StockRegisterReport> GetStockRegisterReport(StockRegisterReport stockRegisterReport)
        {
            return _reportRepository.GetStockRegisterReport(stockRegisterReport);
        }
        #endregion GetStockRegisterReport

        #region GetStockLedgerReport
        public List<StockLedgerReport> GetStockLedgerReport(StockLedgerReport stockLedgerReport)
        {
            return _reportRepository.GetStockLedgerReport(stockLedgerReport);
        }
        #endregion GetStockLedgerReport

        #region GetInventoryReOrderStatusFGReport
        public List<InventoryReOrderStatusFGReport> GetInventoryReOrderStatusFGReport(InventoryReOrderStatusFGReport inventoryReOrderStatusFGReport)
        {
            return _reportRepository.GetInventoryReOrderStatusFGReport(inventoryReOrderStatusFGReport);
        }
        #endregion GetInventoryReOrderStatusFGReport

     
        #region GetStockRegisterFGReport      
        public List<StockRegisterFGReport> GetStockRegisterFGReport(StockRegisterFGReport stockRegisterFGReport)
        {            
            return _reportRepository.GetStockRegisterFGReport(stockRegisterFGReport);
        }
        #endregion GetStockRegisterFGReport


        #region GetStockLedgerFGReport
        public List<StockLedgerFGReport> GetStockLedgerFGReport(StockLedgerFGReport stockLedgerFGReport)
        {
            return _reportRepository.GetStockLedgerFGReport(stockLedgerFGReport);
        }
        #endregion GetStockLedgerFGReport

        #region GetProductStageWiseStockReport
        public List<ProductStageWiseStockReport> GetProductStageWiseStockReport(ProductStageWiseStockReport productStagewiseReport)
        {
            return _reportRepository.GetProductStageWiseStockReport(productStagewiseReport);
        } 
        #endregion GetProductStageWiseStockReport

        public List<DayBook> GetDayBook(string date, string searchTerm)
        {
            return _reportRepository.GetDayBook(date,searchTerm);
        }

        public DataSet GetDayBookDetailByCode(string code, string date)
        {
            return _reportRepository.GetDayBookDetailByCode(code,date);
        }

        public DataSet GetSalesAnalysisReport(string isInvoicedOnly, string fromDate, string toDate)
        {
            return _reportRepository.GetSalesAnalysisReport(isInvoicedOnly,fromDate,toDate);
        }

        public DataSet GetMovementAnalysisReport(MovementAnalysisReport movementAnalysisReport)
        {
            return _reportRepository.GetMovementAnalysisReport(movementAnalysisReport);

        }

        public List<SalesRegisterReport> GetSalesRegisterReport(SalesRegisterReport salesRegisterReport)
        {
            return _reportRepository.GetSalesRegisterReport(salesRegisterReport);
        }
    }
}