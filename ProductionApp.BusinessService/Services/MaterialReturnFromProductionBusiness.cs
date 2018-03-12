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
    public class MaterialReturnFromProductionBusiness : IMaterialReturnFromProductionBusiness
    {
        private IMaterialReturnFromProductionRepository _materialReturnFromProductionRepository;
        private ICommonBusiness _commonBusiness;
        public MaterialReturnFromProductionBusiness(IMaterialReturnFromProductionRepository materialReturnFromProductionRepository, ICommonBusiness commonBusiness)
        {
            _materialReturnFromProductionRepository = materialReturnFromProductionRepository;
            _commonBusiness = commonBusiness;
        }
        public List<MaterialReturnFromProduction> GetAllReturnFromProduction(MaterialReturnFromProductionAdvanceSearch materialReturnAdvanceSearch)
        {
            return _materialReturnFromProductionRepository.GetAllReturnFromProduction(materialReturnAdvanceSearch);
        }
        public object InsertUpdateReturnFromProduction(MaterialReturnFromProduction materialReturnFromProduction)
        {
            DetailsXMl(materialReturnFromProduction);
            return _materialReturnFromProductionRepository.InsertUpdateReturnFromProduction(materialReturnFromProduction);
        }

        public void DetailsXMl(MaterialReturnFromProduction materialReturnFromProduction)
        {
            string result = "<Details>";
            int totalRows = 0;
            foreach (object some_object in materialReturnFromProduction.MaterialReturnFromProductionDetailList)
            {
                _commonBusiness.XML(some_object, ref result, ref totalRows);
            }
            result = result + "</Details>";

            materialReturnFromProduction.DetailXML = result;
        }

        public MaterialReturnFromProduction GetReturnFromProduction(Guid id)
        {
            return _materialReturnFromProductionRepository.GetReturnFromProduction(id);
        }

       public List<MaterialReturnFromProductionDetail> GetReturnFromProductionDetail(Guid id)
        {
            return _materialReturnFromProductionRepository.GetReturnFromProductionDetail(id);
        }

        public object DeleteReturnFromProductionDetail(Guid id)
        {
            return _materialReturnFromProductionRepository.DeleteReturnFromProductionDetail(id);
        }

        public object DeleteReturnFromProduction(Guid id)
        {
            return _materialReturnFromProductionRepository.DeleteReturnFromProduction(id);
        }

    }
}
