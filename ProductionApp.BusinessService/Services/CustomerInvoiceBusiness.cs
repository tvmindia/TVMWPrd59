using ProductionApp.BusinessService.Contracts;
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
        public CustomerInvoiceBusiness(ICustomerInvoiceRepository customerInvoiceRepository)
        {
            _customerInvoiceRepository = customerInvoiceRepository;
        }
    }
}
