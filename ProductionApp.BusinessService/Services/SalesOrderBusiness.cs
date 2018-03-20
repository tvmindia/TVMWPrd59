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
        private ICommonBusiness _commonBusiness;

        public SalesOrderBusiness(ISalesOrderRepository salesOrderRepository, ICommonBusiness commonBusiness)
        {
            _salesOrderRepository = salesOrderRepository;
            _commonBusiness = commonBusiness;

        }

        public List<SalesOrder> GetAllSalesOrder(SalesOrderAdvanceSearch salesOrderAdvanceSearch)
        {
            return _salesOrderRepository.GetAllSalesOrder(salesOrderAdvanceSearch);
        }

        public List<SalesOrder> GetAllSalesOrderDetail(SalesOrderAdvanceSearch salesOrderAdvanceSearch)
        {
            return _salesOrderRepository.GetAllSalesOrderDetail(salesOrderAdvanceSearch);
        }

        public object InsertUpdateSalesOrder(SalesOrder salesOrder)
        {
            DetailsXMl(salesOrder);
            return _salesOrderRepository.InsertUpdateSalesOrder(salesOrder);
        }

        public void DetailsXMl(SalesOrder salesOrder)
        {
            string result = "<Details>";
            int totalRows = 0;
            foreach (object some_object in salesOrder.SalesOrderDetailList)
            {
                _commonBusiness.XML(some_object, ref result, ref totalRows);
            }
            result = result + "</Details>";

            salesOrder.DetailXML = result;
        }

        public List<SalesOrder> GetAllSalesOrderForSelectList()
        {
            return _salesOrderRepository.GetAllSalesOrderForSelectList();
        }
        public List<SalesOrderDetail> GetSalesOrderProductList(Guid salesOrderId,Guid packingSlipID)
        {
            return _salesOrderRepository.GetSalesOrderProductList(salesOrderId, packingSlipID);
        }

        public SalesOrder GetSalesOrder(Guid id)
        {
            return _salesOrderRepository.GetSalesOrder(id);
        }

        public List<SalesOrderDetail> GetSalesOrderDetail(Guid salesOrderId)
        {
            return _salesOrderRepository.GetSalesOrderDetail(salesOrderId);
        }

        public object DeleteSalesOrderDetail(Guid id)
        {
            return _salesOrderRepository.DeleteSalesOrderDetail(id);
        }

        public object DeleteSalesOrder(Guid id)
        {
            return _salesOrderRepository.DeleteSalesOrder(id);
        }

        public SalesOrder GetCustomerOfSalesOrderForPackingSlip(Guid salesOrderId)
        {
            return _salesOrderRepository.GetCustomerOfSalesOrderForPackingSlip(salesOrderId);
        }
    }
}
