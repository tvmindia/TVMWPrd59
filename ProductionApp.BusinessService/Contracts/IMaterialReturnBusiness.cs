using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Contracts
{
    public interface IMaterialReturnBusiness
    {
        List<MaterialReturn> GetAllReturnToSupplier(MaterialReturnAdvanceSearch materialReturnAdvanceSearch);
        object InsertUpdateMaterialReturn(MaterialReturn materialReturn);
        MaterialReturn GetMaterialReturn(Guid id);
        List<MaterialReturnDetail> GetMaterialReturnDetail(Guid id);
        object DeleteMaterialReturn(Guid id);
        object DeleteMaterialReturnDetail(Guid id);
    }
}
