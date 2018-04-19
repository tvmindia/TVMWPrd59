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
        private IMailBusiness _mailBusiness;
        public PurchaseOrderBusiness(IPurchaseOrderRepository purchaseOrderRepository, ICommonBusiness commonBusiness, IMailBusiness mailBusiness)
        {
            _purchaseOrderRepository =  purchaseOrderRepository;
            _commonBusiness = commonBusiness;
            _mailBusiness = mailBusiness;
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
        public object UpdatePOMailDetails(PurchaseOrder purchaseOrder)
        {
            return _purchaseOrderRepository.UpdatePOMailDetails(purchaseOrder);
        }
        public PurchaseOrder GetPurchaseOrder(Guid ID)
        {
            return _purchaseOrderRepository.GetPurchaseOrder(ID);
        }
        public List<PurchaseOrderDetail> GetPurchaseOrderDetail(Guid ID)
        {
            return _purchaseOrderRepository.GetPurchaseOrderDetails(ID);
        }
        public List<PurchaseOrderDetail> GetPurchaseOrderDetailByPODetailID(Guid ID)
        {
            return _purchaseOrderRepository.GetPurchaseOrderDetailByPODetailID(ID);
        }
        public object DeletePurchaseOrder(Guid ID)
        {
            return _purchaseOrderRepository.DeletePurchaseOrder(ID);
        }
        public object DeletePurchaseOrderDetail(Guid ID)
        {
            return _purchaseOrderRepository.DeletePurchaseOrderDetail(ID);
        }
        public PurchaseOrder GetMailPreview(Guid ID)
        {
            PurchaseOrder purchaseOrder = null;
            try
            {
                purchaseOrder = GetPurchaseOrder(ID);
                if (purchaseOrder != null)
                {
                    if ((purchaseOrder.ID != Guid.Empty) && (purchaseOrder.ID != null))
                    {
                        purchaseOrder.PODDetail = GetPurchaseOrderDetail(ID);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return purchaseOrder;
        }

        public async Task<bool> QuoteEmailPush(PurchaseOrder purchaseOrder)
        {

            bool sendsuccess = false;
           
            try
            {
                if (!string.IsNullOrEmpty(purchaseOrder.PurchaseOrderMailPreview.SentToEmails))
                {
                    string[] EmailList = purchaseOrder.PurchaseOrderMailPreview.SentToEmails.Split(',');
                    foreach (string email in EmailList)
                    {
                        Mail _mail = new Mail();
                        _mail.Body = purchaseOrder.PurchaseOrderMailPreview.MailBody;
                        _mail.Subject = "Purchase Order";
                        _mail.To = email;
                        sendsuccess = await _mailBusiness.MailSendAsync(_mail);
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sendsuccess;
        }

        public object UpdatePurchaseOrderMailStatus(PurchaseOrder purchaseOrder)
        {
            Object result = null;
            try
            {

                result = _purchaseOrderRepository.UpdatePurchaseOrderMailStatus(purchaseOrder);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public List<PurchaseOrder> RecentPurchaseOrder(string BaseURL)
        {
            return _purchaseOrderRepository.RecentPurchaseOrder();
        }
    }
}
