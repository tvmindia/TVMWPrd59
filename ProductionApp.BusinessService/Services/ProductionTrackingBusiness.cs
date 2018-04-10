﻿using ProductionApp.BusinessService.Contracts;
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

        public List<ProductionTracking> GetProductionTrackingSearchList(string searchTerm)
        {
            return _ProductionTrackingRepository.GetProductionTrackingSearchList(searchTerm);
        }
        
        public object InsertUpdateProductionTracking(ProductionTracking productionTracking)
        {
            return _ProductionTrackingRepository.InsertUpdateProductionTracking(productionTracking);
        }
    }
}