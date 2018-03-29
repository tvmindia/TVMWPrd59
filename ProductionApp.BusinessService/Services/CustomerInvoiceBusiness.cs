﻿using ProductionApp.BusinessService.Contracts;
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

        public List<CustomerInvoiceDetail> GetPackingSlipListDetail(string packingSlipIDs)
        {
            return _customerInvoiceRepository.GetPackingSlipListDetail(packingSlipIDs);
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
    }
}
