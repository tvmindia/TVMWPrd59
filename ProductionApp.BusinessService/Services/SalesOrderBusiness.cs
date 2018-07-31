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
            try
            {
                return _salesOrderRepository.GetAllSalesOrder(salesOrderAdvanceSearch);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SalesOrder> GetAllSalesOrderDetail(SalesOrderAdvanceSearch salesOrderAdvanceSearch)
        {
            try
            {
                return _salesOrderRepository.GetAllSalesOrderDetail(salesOrderAdvanceSearch);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object InsertUpdateSalesOrder(SalesOrder salesOrder)
        {
            try
            {
                DetailsXMl(salesOrder);
                return _salesOrderRepository.InsertUpdateSalesOrder(salesOrder);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
            try
            {
                return _salesOrderRepository.GetAllSalesOrderForSelectList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SalesOrderDetail> GetSalesOrderProductList(Guid salesOrderId, Guid packingSlipID)
        {
            try
            {
                return _salesOrderRepository.GetSalesOrderProductList(salesOrderId, packingSlipID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SalesOrder GetSalesOrder(Guid id)
        {
            try
            {
                return _salesOrderRepository.GetSalesOrder(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SalesOrderDetail> GetSalesOrderDetail(Guid salesOrderId)
        {
            try
            {
                return _salesOrderRepository.GetSalesOrderDetail(salesOrderId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object DeleteSalesOrderDetail(Guid id)
        {
            try
            {
                return _salesOrderRepository.DeleteSalesOrderDetail(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object DeleteSalesOrder(Guid id)
        {
            try
            {
                return _salesOrderRepository.DeleteSalesOrder(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SalesOrder GetCustomerOfSalesOrderForPackingSlip(Guid salesOrderId)
        {
            try
            {
                return _salesOrderRepository.GetCustomerOfSalesOrderForPackingSlip(salesOrderId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SalesOrder> GetRecentSalesOrder(string BaseURL)
        {
            try
            {
                return _salesOrderRepository.GetRecentSalesOrder();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SalesOrderDetail> GetSaleOrderDetaiByGroupId(Guid id)
        {
            try
            {
                return _salesOrderRepository.GetSaleOrderDetaiByGroupId(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SalesOrderDetail> GetProductListForPackingSlipByGroupID(Guid salesOrderId, Guid packingSlipId, Guid groupId)
        {
            try
            {
                return _salesOrderRepository.GetProductListForPackingSlipByGroupID(salesOrderId, packingSlipId, groupId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
