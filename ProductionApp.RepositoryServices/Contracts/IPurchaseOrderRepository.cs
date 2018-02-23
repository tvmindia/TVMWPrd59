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
        object InsertPurchaseOrder(PurchaseOrder PO);
        PurchaseOrder GetPurchaseOrderByID(Guid ID);
        List<PurchaseOrderDetail> GetPurchaseOrderDetailByID(Guid ID);
    }
}
