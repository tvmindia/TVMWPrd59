using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Contracts
{
    public interface ISupplierCreditNoteBusiness
    {
        List<SupplierCreditNote> GetAllSupplierCreditNote();
        //List<SupplierCreditNotes> GetCreditNoteBySupplier(Guid ID);
        //List<SupplierCreditNotes> GetCreditNoteByPaymentID(Guid ID, Guid PaymentID);
        //SupplierCreditNotes GetCreditNoteAmount(Guid CreditID, Guid SupplierID);
        SupplierCreditNote GetSupplierCreditNote(Guid ID);
        object InsertUpdateSupplierCreditNote(SupplierCreditNote supplierCreditNote);
        object DeleteSupplierCreditNote(Guid ID, string userName);

    }
}
