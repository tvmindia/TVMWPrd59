using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Contracts
{
    public interface IRawMaterialBusiness
    {
        List<RawMaterial> GetAllRawMaterial(RawMaterialAdvanceSearch rawMaterialAdvanceSearch);
        bool CheckMaterialCodeExist(string materialCode);
        object InsertUpdateRawMaterial(RawMaterial rawMaterial);
        RawMaterial GetRawMaterial(Guid id);
    }
}
