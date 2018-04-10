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
        public CustomerInvoiceBusiness(ICustomerInvoiceRepository customerInvoiceRepository, ICommonBusiness commonBusiness)
        {
            _customerInvoiceRepository = customerInvoiceRepository;
            _commonBusiness = commonBusiness;
        }

        public List<CustomerInvoiceDetail> GetPackingSlipListDetail(string packingSlipIDs, string id)
        {
            return _customerInvoiceRepository.GetPackingSlipListDetail(packingSlipIDs,id);
        }

        public object InsertUpdateCustomerInvoice(CustomerInvoice customerInvoice)
        {
            DetailsXMl(customerInvoice);
            return _customerInvoiceRepository.InsertUpdateCustomerInvoice(customerInvoice);
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
            return _customerInvoiceRepository.GetPackingSlipList(customerID);
        }

        public CustomerInvoice GetCustomerInvoice(Guid id)
        {
            return _customerInvoiceRepository.GetCustomerInvoice(id);
        }

        public List<CustomerInvoiceDetail> GetCustomerInvoiceDetail(Guid id)
        {
            return _customerInvoiceRepository.GetCustomerInvoiceDetail(id);
        }

        public List<CustomerInvoice> GetAllCustomerInvoice(CustomerInvoiceAdvanceSearch customerInvoiceAdvanceSearch)
        {
            return _customerInvoiceRepository.GetAllCustomerInvoice(customerInvoiceAdvanceSearch);
        }

        public List<CustomerInvoiceDetail> GetCustomerInvoiceDetailLinkForEdit(string id)
        {
            return _customerInvoiceRepository.GetCustomerInvoiceDetailLinkForEdit(id);
        }

        public object UpdateCustomerInvoiceDetail(CustomerInvoice customerInvoice)
        {
            DetailsXMl(customerInvoice);
            return _customerInvoiceRepository.UpdateCustomerInvoiceDetail(customerInvoice);
        }

        public object DeleteCustomerInvoice(Guid id)
        {
            return _customerInvoiceRepository.DeleteCustomerInvoice(id);

        }

        public object DeleteCustomerInvoiceDetail(Guid id)
        {
            return _customerInvoiceRepository.DeleteCustomerInvoiceDetail(id);

        }
    }
}
