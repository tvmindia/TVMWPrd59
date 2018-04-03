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
        public List<Supplier> GetSupplierForSelectList()
        {
            return _supplierRepository.GetSupplierForSelectList();
        }
        public List<Supplier> GetAllSupplier(SupplierAdvanceSearch supplierAdvanceSearch)
        {
            return _supplierRepository.GetAllSupplier(supplierAdvanceSearch);
        }
        public object InsertUpdateSupplier(Supplier supplier)
        {
            return _supplierRepository.InsertUpdateSupplier(supplier);
        }
        public Supplier GetSupplier(Guid id)
        {
            return _supplierRepository.GetSupplier(id);
        }
        public object DeleteSupplier(Guid id)
        {
            return _supplierRepository.DeleteSupplier(id);
        }

    }
}
