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
    public class PurchaseOrderBusiness: IPurchaseOrderBusiness
    {
        private IPurchaseOrderRepository _purchaseOrderRepository;
        public PurchaseOrderBusiness(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository =  purchaseOrderRepository;
        }
        public List<PurchaseOrder> GetAllPurchaseOrder(PurchaseOrderAdvanceSearch purchaseOrderAdvanceSearch)
        {
            return _purchaseOrderRepository.GetAllPurchaseOrder(purchaseOrderAdvanceSearch);
        }
    }
}
