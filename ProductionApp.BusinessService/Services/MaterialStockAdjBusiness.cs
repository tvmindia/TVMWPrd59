using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.RepositoryServices.Contracts;


namespace ProductionApp.BusinessService.Services
{
    public class MaterialStockAdjBusiness:IMaterialStockAdjBusiness
    {
        private IMaterialStockAdjRepository _materialStockAdjRepository;
        private ICommonBusiness _commonBusiness;
        public MaterialStockAdjBusiness(IMaterialStockAdjRepository materialStockAdjRepoitory,ICommonBusiness commonBusiness)
        {
            _materialStockAdjRepository = materialStockAdjRepoitory;
            _commonBusiness = commonBusiness;
        }

        public List<MaterialStockAdj> GetAllMaterialStockAdjustment(MaterialStockAdjAdvanceSearch materialStockAdjAdvanceSearch)
        {
            return _materialStockAdjRepository.GetAllMaterialStockAdjustment(materialStockAdjAdvanceSearch);
        }
    }
}
