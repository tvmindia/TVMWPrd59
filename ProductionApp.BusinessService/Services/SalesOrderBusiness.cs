using ProductionApp.BusinessService.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.RepositoryServices.Contracts;

namespace ProductionApp.BusinessService.Services
{
    public class SalesOrderBusiness : ISalesOrderBusiness
    {
        ISalesOrderRepository _salesOrderRepository; 

        public SalesOrderBusiness(ISalesOrderRepository salesOrderRepository)
        {
            _salesOrderRepository = salesOrderRepository;
        }

        public List<SalesOrder> GetAllSalesOrder(SalesOrderAdvanceSearch salesOrderAdvanceSearch)
        {
            return _salesOrderRepository.GetAllSalesOrder(salesOrderAdvanceSearch);
        }

        public List<SalesOrder> GetAllSalesOrderDetail(SalesOrderAdvanceSearch salesOrderAdvanceSearch)
        {
            return _salesOrderRepository.GetAllSalesOrderDetail(salesOrderAdvanceSearch);
        }

    }
}
