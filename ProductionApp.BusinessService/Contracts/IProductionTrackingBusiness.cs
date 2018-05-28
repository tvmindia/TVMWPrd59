using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Contracts
{
    public interface IProductionTrackingBusiness
    {
        List<ProductionTracking> GetAllProductionTracking(ProductionTrackingAdvanceSearch productionTrackingAdvanceSearch);
        List<ProductionTracking> GetProductionTrackingSearchList(string searchTerm);
        object InsertUpdateProductionTracking(ProductionTracking productionTracking);
        object DeleteProductionTracking(ProductionTracking productionTracking);
        ProductionTracking GetProductionTracking(Guid id);
        List<ProductionTracking> GetRecentProductionTracking(string BaseURL);
        List<ProductionTracking> GetPendingProductionTracking(string postDate);
        List<ProductionTracking> GetPendingProductionTrackingDetail(ProductionTrackingAdvanceSearch productionTrackingAdvanceSearch);
        List<string> GetAllAvailableProductionTrackingEntryDate();
        object UpdateProductionTrackingByXML(Common common, List<ProductionTracking> productionTrackingList);
        object ProductionTrackingPosting(Common common, string postDate);
    }
}
