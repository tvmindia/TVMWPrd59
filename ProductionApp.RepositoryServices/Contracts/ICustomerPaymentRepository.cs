using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.RepositoryServices.Contracts
{
    public interface ICustomerPaymentRepository
    {
        List<CustomerPayment> GetAllCustomerPayment(CustomerPaymentAdvanceSearch customerPaymentAdvanceSearch);
        List<CustomerInvoice> GetOutStandingInvoices(Guid PaymentID, Guid CustID);
        CustomerInvoice GetOutstandingAmount(Guid Id);
        object InsertUpdateCustomerPayment(CustomerPayment customerPayment);
        CustomerPayment GetCustomerPayment(string Id);
        object DeleteCustomerPayment(Guid id);
        object ValidateCustomerPayment(Guid id, string paymentrefNo);
    }
}
