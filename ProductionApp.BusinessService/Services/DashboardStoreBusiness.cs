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
    public class DashboardStoreBusiness: IDashboardStoreBusiness
    {
        private IDashboardStoreRepository _dashboardStoreRepository;
        public DashboardStoreBusiness(IDashboardStoreRepository dashboardStoreRepository)
        {
            _dashboardStoreRepository = dashboardStoreRepository;
        }

        public List<MaterialIssue> GetRecentIssueSummary()
        {
            return _dashboardStoreRepository.GetRecentIssueSummary();
        }

        public List<MaterialReceipt> GetRecentMaterialReceiptSummary()
        {
            return _dashboardStoreRepository.GetRecentMaterialReceiptSummary();
        }
    }
}
