using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Services
{
    public class ReportBusiness : IReportBusiness
    {
        private IReportRepository _reportRepository;
        public ReportBusiness(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
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
    }
}
