using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.RepositoryServices.Contracts
{
    public interface IRawMaterialRepository
    {
        List<RawMaterial> GetAllRawMaterial(RawMaterialAdvanceSearch rawMaterialAdvanceSearch);
        bool CheckMaterialCodeExist(string materialCode);
        object InsertUpdateRawMaterial(RawMaterial rawMaterial);
    }
}
