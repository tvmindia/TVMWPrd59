using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.RepositoryServices.Contracts
{
    public interface IMaterialTypeRepository
    {
        List<MaterialType> GetMaterialTypeForSelectList();
        MaterialType GetMaterialType(string code);
        object InsertUpdateMaterialType(MaterialType materialType);
    }
}
