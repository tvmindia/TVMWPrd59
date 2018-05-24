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
    public class ProductionTrackingBusiness: IProductionTrackingBusiness
    {
        #region ConstructorInjection
        private IProductionTrackingRepository _productionTrackingRepository;
        ICommonBusiness _commonBusiness;
        public ProductionTrackingBusiness(IProductionTrackingRepository productionTrackingRepository, ICommonBusiness commonBusiness)
        {
            _productionTrackingRepository = productionTrackingRepository;
            _commonBusiness = commonBusiness;
        }
        #endregion ConstructorInjection

        public List<ProductionTracking> GetAllProductionTracking(ProductionTrackingAdvanceSearch productionTrackingAdvanceSearch)
        {
            return _productionTrackingRepository.GetAllProductionTracking(productionTrackingAdvanceSearch);
        }

        public List<ProductionTracking> GetProductionTrackingSearchList(string searchTerm)
        {
            return _productionTrackingRepository.GetProductionTrackingSearchList(searchTerm);
        }
        
        public object InsertUpdateProductionTracking(ProductionTracking productionTracking)
        {
            return _productionTrackingRepository.InsertUpdateProductionTracking(productionTracking);
        }

        public object DeleteProductionTracking(ProductionTracking productionTracking)
        {
            return _productionTrackingRepository.DeleteProductionTracking(productionTracking);
        }

        public ProductionTracking GetProductionTracking(Guid id)
        {
            return _productionTrackingRepository.GetProductionTracking(id);
        }

        public List<ProductionTracking> GetRecentProductionTracking(string BaseURL)
        {
            return _productionTrackingRepository.GetRecentProductionTracking();
        }

        public List<ProductionTracking> GetPendingProductionTracking(string postDate)
        {
            return _productionTrackingRepository.GetPendingProductionTracking(postDate);
        }

        public List<ProductionTracking> GetPendingProductionTrackingDetail(ProductionTrackingAdvanceSearch productionTrackingAdvanceSearch)
        {
            return _productionTrackingRepository.GetPendingProductionTrackingDetail(productionTrackingAdvanceSearch);
        }

        public List<string> GetAllAvailableProductionTrackingEntryDate()
        {
            return _productionTrackingRepository.GetAllAvailableProductionTrackingEntryDate();
        }

        public string DetailsXMl(List<ProductionTracking> productionTrackingList)
        {
            string result = "<Details>";
            int totalRows = 0;
            foreach (object some_object in productionTrackingList)
            {
                _commonBusiness.XML(some_object, ref result, ref totalRows);
            }
            result = result + "</Details>";

            return result;
        }

        public object UpdateProductionTrackingByXML(Common common,List<ProductionTracking> productionTrackingList)
        {
            string productionTrackingXML = DetailsXMl(productionTrackingList);
            return _productionTrackingRepository.UpdateProductionTrackingByXML(common, productionTrackingXML);
        }

        public object ProductionTrackingPosting(Common common, string postDate)
        {
            return _productionTrackingRepository.ProductionTrackingPosting(common, postDate);
        }
    }
}
