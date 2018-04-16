using ProductionApp.BusinessService.Contracts;
using ProductionApp.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Services
{
    public class SupplierInvoiceBusiness: ISupplierInvoiceBusiness
    {
        private ISupplierInvoiceRepository _supplierInvoiceRepository;
        private ICommonBusiness _commonBusiness;
        public SupplierInvoiceBusiness(ISupplierInvoiceRepository supplierInvoiceRepository, ICommonBusiness commonBusiness)
        {
            _supplierInvoiceRepository = supplierInvoiceRepository;
            _commonBusiness = commonBusiness;
        }
    }
}
