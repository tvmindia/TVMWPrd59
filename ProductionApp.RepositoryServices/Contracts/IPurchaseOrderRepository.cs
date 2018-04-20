using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.RepositoryServices.Contracts
{
    public interface IPurchaseOrderRepository
    {
        List<PurchaseOrder> GetAllPurchaseOrder(PurchaseOrderAdvanceSearch purchaseOrderAdvanceSearch);
        List<PurchaseOrder> PurchaseOrderDropdownList(Guid supplierID);
        object InsertPurchaseOrder(PurchaseOrder purchaseOrder);
        object UpdatePurchaseOrder(PurchaseOrder purchaseOrder);
        PurchaseOrder GetPurchaseOrder(Guid ID);
        List<PurchaseOrderDetail> GetPurchaseOrderDetails(Guid ID);
        List<PurchaseOrderDetail> GetPurchaseOrderDetailByPODetailID(Guid ID);
        object DeletePurchaseOrder(Guid ID);
        object DeletePurchaseOrderDetail(Guid ID);
        object UpdatePurchaseOrderDetailLink(PurchaseOrder purchaseOrder);
        object UpdatePOMailDetails(PurchaseOrder purchaseOrder);
        object UpdatePurchaseOrderMailStatus(PurchaseOrder purchaseOrder);
        List<PurchaseOrder> RecentPurchaseOrder();
    }
}
