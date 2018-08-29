using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Contracts
{
    public interface ICustomerCreditNoteBusiness
    {

        List<CustomerCreditNote> GetAllCustomerCreditNote(CustomerCreditNoteAdvanceSearch customerCreditNoteAdvanceSearch);
        List<CustomerCreditNote> GetCreditNoteByCustomer(Guid ID);
        List<CustomerCreditNote> GetCreditNoteByPaymentID(Guid ID, Guid PaymentID);
        CustomerCreditNote GetCreditNoteAmount(Guid CreditID, Guid CustomerID);
        CustomerCreditNote GetCustomerCreditNote(Guid ID);
        object InsertUpdateCustomerCreditNote(CustomerCreditNote customerCreditNote);
        object DeleteCustomerCreditNote(Guid ID, string userName);
    }
}
