using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.RepositoryServices.Contracts
{
    public interface ISalesOrderRepository
    {
        List<SalesOrder> GetAllSalesOrder(SalesOrderAdvanceSearch salesOrderAdvanceSearch);
        List<SalesOrder> GetAllSalesOrderDetail(SalesOrderAdvanceSearch salesOrderAdvanceSearch);
        object InsertUpdateSalesOrder(SalesOrder salesOrder);
        List<SalesOrder> GetAllSalesOrderForSelectList();
        List<SalesOrderDetail> GetSalesOrderProductList(Guid salesOrderId,Guid packingSlipID);
        SalesOrder GetSalesOrder(Guid id);
        List<SalesOrderDetail> GetSalesOrderDetail(Guid salesOrderId);
        object DeleteSalesOrderDetail(Guid id);
        object DeleteSalesOrder(Guid id);
        SalesOrder GetCustomerOfSalesOrderForPackingSlip(Guid salesOrderId);
        List<SalesOrder> GetRecentSalesOrder();
    }
}
