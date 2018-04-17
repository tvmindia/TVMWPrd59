using ProductionApp.BusinessService.Contracts;
using ProductionApp.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Services
{
    public class SupplierPaymentBusiness: ISupplierPaymentBusiness
    {
        private ISupplierPaymentRepository _supplierPaymentRepository;
        private ICommonBusiness _commonBusiness;
        public SupplierPaymentBusiness(ISupplierPaymentRepository supplierPaymentRepository, ICommonBusiness commonBusiness)
        {
            _supplierPaymentRepository = supplierPaymentRepository;
            _commonBusiness = commonBusiness;
        }



    }
}
