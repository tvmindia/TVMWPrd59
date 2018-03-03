using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Contracts
{
    public interface IPurchaseOrderBusiness
    {
        List<PurchaseOrder> GetAllPurchaseOrder(PurchaseOrderAdvanceSearch purchaseOrderAdvanceSearch);
        List<PurchaseOrder> GetAllPurchaseOrderForSelectList();
        object InsertPurchaseOrder(PurchaseOrder purchaseOrder);
        object UpdatePurchaseOrder(PurchaseOrder purchaseOrder);
        object UpdatePurchaseOrderDetailLink(PurchaseOrder purchaseOrder);
        PurchaseOrder GetPurchaseOrderByID(Guid ID);
        PurchaseOrder GetMailPreview(Guid ID);
        List<PurchaseOrderDetail> GetPurchaseOrderDetailByID(Guid ID);
        List<PurchaseOrderDetail> GetPurchaseOrderDetailByIDForEdit(Guid ID);
        object DeletePurchaseOrder(Guid ID);
        object DeletePurchaseOrderDetail(Guid ID);
    }
}
