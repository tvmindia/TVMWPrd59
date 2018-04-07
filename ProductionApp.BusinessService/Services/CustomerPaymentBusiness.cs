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
    public class CustomerPaymentBusiness : ICustomerPaymentBusiness
    {
        private ICommonBusiness _commonBusiness;
        private ICustomerPaymentRepository _customerPaymentRepository;
        public CustomerPaymentBusiness(ICommonBusiness commonBusiness, ICustomerPaymentRepository customerPaymentRepository)
        {
            _commonBusiness = commonBusiness;
            _customerPaymentRepository = customerPaymentRepository;
        }
        public List<CustomerPayment> GetAllCustomerPayment(CustomerPaymentAdvanceSearch customerPaymentAdvanceSearch)
        {
            return _customerPaymentRepository.GetAllCustomerPayment(customerPaymentAdvanceSearch);
        }
        public List<CustomerInvoice> GetOutStandingInvoices(Guid PaymentID, Guid CustID)
        {
            return _customerPaymentRepository.GetOutStandingInvoices(PaymentID, CustID);
        }
        public CustomerInvoice GetOutstandingAmount(Guid Id)
        {
            return _customerPaymentRepository.GetOutstandingAmount(Id);
        }
        public object InsertUpdateCustomerPayment(CustomerPayment customerPayment)
        {
            DetailsXMl(customerPayment);
            return _customerPaymentRepository.InsertUpdateCustomerPayment(customerPayment);
        }
        public void DetailsXMl(CustomerPayment customerPayment)
        {
            string result = "<Details>";
            int totalRows = 0;
            foreach (object some_object in customerPayment.CustomerPaymentDetailList)
            {
                _commonBusiness.XML(some_object, ref result, ref totalRows);
            }
            result = result + "</Details>";

            customerPayment.DetailXML = result;
        }

        public CustomerPayment GetCustomerPayment(string Id)
        {
            return _customerPaymentRepository.GetCustomerPayment(Id);
        }

        public object DeleteCustomerPayment(Guid id)
        {
            return _customerPaymentRepository.DeleteCustomerPayment(id);
        }

        public object ValidateCustomerPayment(Guid id, string paymentrefNo)
        {
            return _customerPaymentRepository.ValidateCustomerPayment(id, paymentrefNo);
        }
    }
}
