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
    public class RawMaterialBusiness:IRawMaterialBusiness
    {
        private IRawMaterialRepository _rawMaterialRepository;
        public RawMaterialBusiness(IRawMaterialRepository rawMaterialRepository)
        {
            _rawMaterialRepository = rawMaterialRepository;
        }
        public List<RawMaterial> GetAllRawMaterial(RawMaterialAdvanceSearch rawMaterialAdvanceSearch)
        {
            return _rawMaterialRepository.GetAllRawMaterial(rawMaterialAdvanceSearch);
        }
        public bool CheckMaterialCodeExist(string materialCode)
        {
            return _rawMaterialRepository.CheckMaterialCodeExist(materialCode);
        }
        public object InsertUpdateRawMaterial(RawMaterial rawMaterial)
        {
            return _rawMaterialRepository.InsertUpdateRawMaterial(rawMaterial);
        }
    }
}
