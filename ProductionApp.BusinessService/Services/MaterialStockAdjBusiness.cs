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

        public object InsertUpdateStockAdjustment(MaterialStockAdj materialStockAdj)
        {
            DetailsXML(materialStockAdj);
            return _materialStockAdjRepository.InsertUpdateStockAdjustment(materialStockAdj);
        }

        public void DetailsXML(MaterialStockAdj materialStockAdj)
        {
            string result = "<Details>";
            int totalRows = 0;
            foreach (object some_object in materialStockAdj.MaterialStockAdjDetailList)
            {
                _commonBusiness.XML(some_object, ref result, ref totalRows);
            }
            result = result + "</Details>";

            materialStockAdj.DetailXML = result;
        }

        public MaterialStockAdj GetMaterialStockAdjustment(Guid id)
        {
            return _materialStockAdjRepository.GetMaterialStockAdjustment(id);
        }

        public List<MaterialStockAdjDetail> GetMaterialStockAdjustmentDetail(Guid id)
        {
            return _materialStockAdjRepository.GetMaterialStockAdjustmentDetail(id);
        }

        public object DeleteMaterialStockAdjustment(Guid id)
        {
            return _materialStockAdjRepository.DeleteMaterialStockAdjustment(id);
        }

        public object DeleteMaterialStockAdjustmentDetail(Guid id)
        {
            return _materialStockAdjRepository.DeleteMaterialStockAdjustmentDetail(id);
        }
    }
}
