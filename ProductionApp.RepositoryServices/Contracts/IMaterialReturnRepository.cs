using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.RepositoryServices.Contracts
{
    public interface IMaterialReturnRepository
    {
        List<MaterialReturn> GetAllReturnToSupplier(MaterialReturnAdvanceSearch materialReturnAdvanceSearch);
        object InsertUpdateMaterialReturn(MaterialReturn materialReturn);
        List<MaterialReturnDetail> GetMaterialReturnDetail(Guid id);
        MaterialReturn GetMaterialReturn(Guid id);
        object DeleteMaterialReturn(Guid id);
        object DeleteMaterialReturnDetail(Guid id);
    }
}
