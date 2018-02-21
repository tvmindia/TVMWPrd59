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
    public class MaterialBusiness:IMaterialBusiness
    {
        private IMaterialRepository _materialRepository;

        public MaterialBusiness(IMaterialRepository materialRepository)
        {
            _materialRepository = materialRepository;
        }
        public List<Material> GetMaterialForSelectList()
        {
            return _materialRepository.GetMaterialForSelectList();
        }
        public List<Material> GetAllMaterial(MaterialAdvanceSearch materialAdvanceSearch)
        {
            return _materialRepository.GetAllMaterial(materialAdvanceSearch);
        }
        public bool CheckMaterialCodeExist(string materialCode)
        {
            return _materialRepository.CheckMaterialCodeExist(materialCode);
        }
        public object InsertUpdateMaterial(Material rawMaterial)
        {
            return _materialRepository.InsertUpdateMaterial(rawMaterial);
        }
        public Material GetMaterial(Guid id)
        {
            return _materialRepository.GetMaterial(id);
        }

    }
}
