using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.RepositoryServices.Contracts
{
    public interface IMaterialRepository
    {
        List<Material> GetMaterialForSelectList();
        List<Material> GetAllMaterial(MaterialAdvanceSearch materialAdvanceSearch);
        bool CheckMaterialCodeExist(string materialCode);
        object InsertUpdateMaterial(Material material);
        Material GetMaterial(Guid id);
    }
}
