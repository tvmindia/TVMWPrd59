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

        List<CustomerCreditNote> GetAllCustomerCreditNote();
        //List<CustomerCreditNotes> GetCreditNoteByCustomer(Guid ID);
        //List<CustomerCreditNotes> GetCreditNoteByPaymentID(Guid ID, Guid PaymentID);
        //CustomerCreditNotes GetCreditNoteAmount(Guid CreditID, Guid CustomerID);
        CustomerCreditNote GetCustomerCreditNote(Guid ID);
        object InsertUpdateCustomerCreditNote(CustomerCreditNote customerCreditNote);
        object DeleteCustomerCreditNote(Guid ID, string userName);
    }
}
