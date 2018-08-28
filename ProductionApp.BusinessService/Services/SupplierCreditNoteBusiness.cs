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
    public class SupplierCreditNoteBusiness: ISupplierCreditNoteBusiness
    {
        private ISupplierCreditNoteRepository _supplierCreditNoteRepository;
        public SupplierCreditNoteBusiness(ISupplierCreditNoteRepository supplierCreditNoteRepository)
        {
            _supplierCreditNoteRepository = supplierCreditNoteRepository;
        }


        public List<SupplierCreditNote> GetAllSupplierCreditNote(SupplierCreditNoteAdvanceSearch supplierCreditNoteAdvanceSearch)
        {
            return _supplierCreditNoteRepository.GetAllSupplierCreditNote(supplierCreditNoteAdvanceSearch);
        }
        public SupplierCreditNote GetSupplierCreditNote(Guid ID)
        {
            SupplierCreditNote SupplierCreditNote = new SupplierCreditNote();
            SupplierCreditNote = _supplierCreditNoteRepository.GetSupplierCreditNote(ID);
            //if (SupplierCreditNote != null)
            //{
            //    SupplierCreditNote.creditAmountFormatted = _commonBusiness.ConvertCurrency(SupplierCreditNote.CreditAmount, 2);
            //    SupplierCreditNote.adjustedAmountFormatted = _commonBusiness.ConvertCurrency(SupplierCreditNote.adjustedAmount, 2);
            //}
            return SupplierCreditNote;
        }
        public object InsertUpdateSupplierCreditNote(SupplierCreditNote SupplierCreditNote)
        {
            object result = null;
            try
            {
                result = _supplierCreditNoteRepository.InsertUpdateSupplierCreditNote(SupplierCreditNote);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public object DeleteSupplierCreditNote(Guid ID, string userName)
        {
            object result = null;
            try
            {
                result = _supplierCreditNoteRepository.DeleteSupplierCreditNote(ID, userName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}
