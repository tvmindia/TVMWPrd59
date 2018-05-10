using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Contracts
{
  public  interface IReportBusiness
    {
        List<AMCSysReport> GetAllReport(string searchTerm);
        List<Requisition> GetRequisitionSummaryReport(RequisitionSummaryReport requisitionSummaryReport);
        List<RequisitionDetailReport> GetRequisitionDetailReport(RequisitionDetailReport requisitionDetailReport);
        List<PurchaseOrder> GetPurchaseSummaryReport(PurchaseSummaryReport purchaseSummaryReport);
        List<PurchaseDetailReport> GetPurchaseDetailReport(PurchaseDetailReport purchaseDetailReport);
    }
}
