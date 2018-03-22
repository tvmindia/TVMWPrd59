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
     public class MaterialReturnBusiness: IMaterialReturnBusiness
    {
        private ICommonBusiness _commonBusiness;
        private IMaterialReturnRepository _materialReturnRepository;
        public MaterialReturnBusiness(ICommonBusiness commonBusiness, IMaterialReturnRepository materialReturnRepository)
        {
            _commonBusiness = commonBusiness;
            _materialReturnRepository = materialReturnRepository;
        }
        public List<MaterialReturn> GetAllReturnToSupplier(MaterialReturnAdvanceSearch materialReturnAdvanceSearch)
        {
            return _materialReturnRepository.GetAllReturnToSupplier(materialReturnAdvanceSearch);
        }
    }
}
