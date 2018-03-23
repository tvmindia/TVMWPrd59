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
        public object InsertUpdateMaterialReturn(MaterialReturn materialReturn)
        {
            DetailsXMl(materialReturn);
            return _materialReturnRepository.InsertUpdateMaterialReturn(materialReturn);
        }
        public void DetailsXMl(MaterialReturn materialReturn)
        {
            string result = "<Details>";
            int totalRows = 0;
            foreach (object some_object in materialReturn.MaterialReturnDetailList)
            {
                _commonBusiness.XML(some_object, ref result, ref totalRows);
            }
            result = result + "</Details>";

            materialReturn.DetailXML = result;
        }
        public MaterialReturn GetMaterialReturn(Guid id)
        {
            return _materialReturnRepository.GetMaterialReturn(id);
        }
        public List<MaterialReturnDetail> GetMaterialReturnDetail(Guid id)
        {
            return _materialReturnRepository.GetMaterialReturnDetail(id);
        }
        public object DeleteMaterialReturn(Guid id)
        {
            return _materialReturnRepository.DeleteMaterialReturn(id);
        }
        public object DeleteMaterialReturnDetail(Guid id)
        {
            return _materialReturnRepository.DeleteMaterialReturnDetail(id);
        }
    }
}
