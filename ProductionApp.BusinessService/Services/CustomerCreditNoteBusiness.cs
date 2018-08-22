using ProductionApp.BusinessService.Contracts;
using ProductionApp.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Services
{
    public class CustomerCreditNoteBusiness: ICustomerCreditNoteBusiness
    {
        private ICustomerCreditNoteRepository _customerCreditNoteRepository;
        public CustomerCreditNoteBusiness(ICustomerCreditNoteRepository customerCreditNoteRepository)
        {
            _customerCreditNoteRepository = customerCreditNoteRepository;
        }

    }
}
