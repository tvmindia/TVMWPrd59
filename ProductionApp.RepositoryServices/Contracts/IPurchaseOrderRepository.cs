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
        List<PurchaseOrder> GetAllPurchaseOrderForSelectList();
        object InsertPurchaseOrder(PurchaseOrder purchaseOrder);
        object UpdatePurchaseOrder(PurchaseOrder purchaseOrder);
        PurchaseOrder GetPurchaseOrderByID(Guid ID);
        List<PurchaseOrderDetail> GetPurchaseOrderDetailByID(Guid ID);
        PurchaseOrderDetail GetPurchaseOrderDetailByIDForEdit(Guid ID);
        object DeletePurchaseOrder(Guid ID);
        object DeletePurchaseOrderDetail(Guid ID);
        object UpdatePurchaseOrderDetailLink(PurchaseOrder purchaseOrder);
    }
}
