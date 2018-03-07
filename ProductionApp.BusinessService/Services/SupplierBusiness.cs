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
    public class SupplierBusiness: ISupplierBusiness
    {
        private ISupplierRepository _supplierRepository;
        public SupplierBusiness(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }
        public List<Supplier> GetAllSupplier()
        {
            return _supplierRepository.GetAllSupplier();
        }
        public Supplier GetSupplier(Guid supplierid)
        {
            return _supplierRepository.GetSupplier(supplierid);
        }
    }
}
