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
    public class MaterialTypeBusiness: IMaterialTypeBusiness
    {
        private IMaterialTypeRepository _materialTypeRepository;

        public MaterialTypeBusiness(IMaterialTypeRepository materialRepository)
        {
            _materialTypeRepository = materialRepository;
        }

        public List<MaterialType> GetMaterialTypeForSelectList()
        {
            return _materialTypeRepository.GetMaterialTypeForSelectList();
        }
    }
}
