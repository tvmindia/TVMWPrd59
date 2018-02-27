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
        private ICommonBusiness _commonBusiness;
        public PurchaseOrderBusiness(IPurchaseOrderRepository purchaseOrderRepository, ICommonBusiness commonBusiness)
        {
            _purchaseOrderRepository =  purchaseOrderRepository;
            _commonBusiness = commonBusiness;
        }
        public List<PurchaseOrder> GetAllPurchaseOrder(PurchaseOrderAdvanceSearch purchaseOrderAdvanceSearch)
        {
            return _purchaseOrderRepository.GetAllPurchaseOrder(purchaseOrderAdvanceSearch);
        }
        public List<PurchaseOrder> GetAllPurchaseOrderForSelectList()
        {
            return _purchaseOrderRepository.GetAllPurchaseOrderForSelectList();
        }
        public object InsertPurchaseOrder(PurchaseOrder purchaseOrder)
        {
            DetailsXMl(purchaseOrder);
            return _purchaseOrderRepository.InsertPurchaseOrder(purchaseOrder);

        }
        public void DetailsXMl(PurchaseOrder purchaseOrder)
        {
            string result = "<Details>";
            int totalRows = 0;
            foreach (object some_object in purchaseOrder.PODDetail)
            {
                _commonBusiness.XML(some_object, ref result, ref totalRows);
            }
            result = result + "</Details>";

            purchaseOrder.PODDetailXML = result;

            //reqDetailLink
            result = "<Details>";
            totalRows = 0;
            foreach (object some_object in purchaseOrder.PODDetailLink)
            {
                _commonBusiness.XML(some_object, ref result, ref totalRows);
            }
            result = result + "</Details>";

            purchaseOrder.PODDetailLinkXML = result;

        }
        public object UpdatePurchaseOrder(PurchaseOrder purchaseOrder)
        {
            DetailsXMl(purchaseOrder);
            return _purchaseOrderRepository.UpdatePurchaseOrder(purchaseOrder);
        }
        public object UpdatePurchaseOrderDetailLink(PurchaseOrder purchaseOrder)
        {
            DetailsXMl(purchaseOrder);
            return _purchaseOrderRepository.UpdatePurchaseOrderDetailLink(purchaseOrder);
        }
        public PurchaseOrder GetPurchaseOrderByID(Guid ID)
        {
            return _purchaseOrderRepository.GetPurchaseOrderByID(ID);
        }
        public List<PurchaseOrderDetail> GetPurchaseOrderDetailByID(Guid ID)
        {
            return _purchaseOrderRepository.GetPurchaseOrderDetailByID(ID);
        }
        public PurchaseOrderDetail GetPurchaseOrderDetailByIDForEdit(Guid ID)
        {
            return _purchaseOrderRepository.GetPurchaseOrderDetailByIDForEdit(ID);
        }
        public object DeletePurchaseOrder(Guid ID)
        {
            return _purchaseOrderRepository.DeletePurchaseOrder(ID);
        }
        public object DeletePurchaseOrderDetail(Guid ID)
        {
            return _purchaseOrderRepository.DeletePurchaseOrderDetail(ID);
        }
    }
}
