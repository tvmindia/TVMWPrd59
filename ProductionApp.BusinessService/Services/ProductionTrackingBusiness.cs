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
        private IProductionTrackingRepository _ProductionTrackingRepository;
        public ProductionTrackingBusiness(IProductionTrackingRepository ProductionTrackingRepository)
        {
            _ProductionTrackingRepository = ProductionTrackingRepository;
        }
        #endregion ConstructorInjection

        public List<ProductionTrackingSearch> GetProductionTrackingSearchList()
        {
            return _ProductionTrackingRepository.GetProductionTrackingSearchList();
        }

    }
}
