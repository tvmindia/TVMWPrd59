using ProductionApp.BusinessService.Contracts;
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
    }
}
