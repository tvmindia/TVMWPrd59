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
    public class CustomerInvoiceBusiness: ICustomerInvoiceBusiness
    {
        private ICustomerInvoiceRepository _customerInvoiceRepository;
        private ICommonBusiness _commonBusiness;
        private IMailBusiness _mailBusiness;
        public CustomerInvoiceBusiness(ICustomerInvoiceRepository customerInvoiceRepository,
            ICommonBusiness commonBusiness, IMailBusiness mailBusiness)
        {
            _customerInvoiceRepository = customerInvoiceRepository;
            _commonBusiness = commonBusiness;
            _mailBusiness = mailBusiness;
        }

        public List<CustomerInvoiceDetail> GetPackingSlipListDetail(string packingSlipIDs, string id)
        {
            try
            {
                return _customerInvoiceRepository.GetPackingSlipListDetail(packingSlipIDs,id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object InsertUpdateCustomerInvoice(CustomerInvoice customerInvoice)
        {
            try
            {
                DetailsXMl(customerInvoice);
                return _customerInvoiceRepository.InsertUpdateCustomerInvoice(customerInvoice);
            }
            catch (Exception ex)
            {
                throw ex;
            } 
        }
        public void DetailsXMl(CustomerInvoice customerInvoice)
        {
            string result = "<Details>";
            int totalRows = 0;
            foreach (object some_object in customerInvoice.CustomerInvoiceDetailList)
            {
                _commonBusiness.XML(some_object, ref result, ref totalRows);
            }
            result = result + "</Details>";

            customerInvoice.DetailXML = result;
        }

        public List<PackingSlip> GetPackingSlipList(Guid customerID)
        {
            try
            {
                return _customerInvoiceRepository.GetPackingSlipList(customerID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CustomerInvoice GetCustomerInvoice(Guid id)
        {
            try
            {
                return _customerInvoiceRepository.GetCustomerInvoice(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CustomerInvoiceDetail> GetCustomerInvoiceDetail(Guid id)
        {
            try
            {
                return _customerInvoiceRepository.GetCustomerInvoiceDetail(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CustomerInvoice> GetAllCustomerInvoice(CustomerInvoiceAdvanceSearch customerInvoiceAdvanceSearch)
        {
            try
            {
                return _customerInvoiceRepository.GetAllCustomerInvoice(customerInvoiceAdvanceSearch);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CustomerInvoiceDetail> GetCustomerInvoiceDetailLinkForEdit(string id)
        {
            try
            {
                return _customerInvoiceRepository.GetCustomerInvoiceDetailLinkForEdit(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CustomerInvoiceDetail> GetCustomerInvoiceDetailLinkForEditGroup(string id, string groupID)
        {
            try
            {
                return _customerInvoiceRepository.GetCustomerInvoiceDetailLinkForEditGroup(id, groupID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object UpdateCustomerInvoiceDetail(CustomerInvoice customerInvoice)
        {
            try
            {
                DetailsXMl(customerInvoice);
            return _customerInvoiceRepository.UpdateCustomerInvoiceDetail(customerInvoice);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public object DeleteCustomerInvoiceDetail(Guid id,string isGroupItem,Guid invoiceID)
        {
            try
            {
                return _customerInvoiceRepository.DeleteCustomerInvoiceDetail(id, isGroupItem, invoiceID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CustomerInvoice> GetRecentCustomerInvoice(string BaseURL)
        {
            try
            {
                return _customerInvoiceRepository.GetRecentCustomerInvoice();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object UpdateCustomerInvoiceMailStatus(CustomerInvoice CustomerInvoice)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> EmailPush(CustomerInvoice customerInvoice)
        {
            bool sendsuccess = false;

            try
            {
                if (!string.IsNullOrEmpty(customerInvoice.CustomerInvoiceMailPreview.SentToEmails))
                {
                    string[] EmailList = customerInvoice.CustomerInvoiceMailPreview.SentToEmails.Split(',');
                    foreach (string email in EmailList)
                    {
                        Mail _mail = new Mail();
                        _mail.Body = customerInvoice.CustomerInvoiceMailPreview.MailBody;
                        _mail.Subject = "Customer Invoice";
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

        public CustomerInvoice GetMailPreview(Guid ID)
        {
            CustomerInvoice customerInvoice = null;
            try
            {
                customerInvoice = GetCustomerInvoice(ID);
                customerInvoice.ID = ID;
                if (customerInvoice != null)
                {
                    if ((customerInvoice.ID != Guid.Empty) && (customerInvoice.ID != null))
                    {
                        customerInvoice.CustomerInvoiceDetailList = GetCustomerInvoiceDetail(ID);
                    }
                    customerInvoice.InvoiceAmountWords = _commonBusiness.NumberToWords(double.Parse((customerInvoice.InvoiceAmount- customerInvoice.Discount).ToString()));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return customerInvoice;
        }

        public List<CustomerInvoiceDetail> GetGroupProductListForCustomerInvoiceDetail(string slipNo, Guid groupID)
        {
            try
            {
                return _customerInvoiceRepository.GetGroupProductListForCustomerInvoiceDetail(slipNo, groupID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CustomerInvoiceDetail> GetGroupCustomerInvoiceDetailLink(Guid id, Guid groupID)
        {
            try
            {
                return _customerInvoiceRepository.GetGroupCustomerInvoiceDetailLink(id, groupID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public decimal GetOutstandingCustomerInvoice()
        {
            try
            {
                return _customerInvoiceRepository.GetOutstandingCustomerInvoice();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object DeleteCustomerInvoice(Guid id)
        {
            try
            {
                return _customerInvoiceRepository.DeleteCustomerInvoice(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
