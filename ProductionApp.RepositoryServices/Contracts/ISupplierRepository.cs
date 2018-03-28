using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.RepositoryServices.Contracts
{
    public interface ISupplierRepository
    {
        List<Supplier> GetSupplierForSelectList();
        List<Supplier> GetAllSupplier(SupplierAdvanceSearch supplierAdvanceSearch);
        object InsertUpdateSupplier(Supplier supplier);
        Supplier GetSupplier(Guid id);
        object DeleteSupplier(Guid id);

    }
}
