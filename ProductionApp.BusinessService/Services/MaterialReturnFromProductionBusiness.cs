using ProductionApp.BusinessService.Contracts;
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
    }
}
