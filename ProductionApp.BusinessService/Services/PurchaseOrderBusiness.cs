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
        public void DetailsXMl(PurchaseOrder PO)
        {
            string result = "<Details>";
            int totalRows = 0;
            foreach (object some_object in PO.PODDetail)
            {
                _commonBusiness.XML(some_object, ref result, ref totalRows);
            }
            result = result + "</Details>";

            PO.PODDetailXML = result;

            //reqDetailLink
            result = "<Details>";
            totalRows = 0;
            foreach (object some_object in PO.PODDetailLink)
            {
                _commonBusiness.XML(some_object, ref result, ref totalRows);
            }
            result = result + "</Details>";

            PO.PODDetailLinkXML = result;

        }
        public PurchaseOrder GetPurchaseOrderByID(Guid ID)
        {
            return _purchaseOrderRepository.GetPurchaseOrderByID(ID);
        }
        public List<PurchaseOrderDetail> GetPurchaseOrderDetailByID(Guid ID)
        {
            return _purchaseOrderRepository.GetPurchaseOrderDetailByID(ID);
        }
    }
}
