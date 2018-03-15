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
    public class PackingSlipBusiness : IPackingSlipBusiness
    {
        private ICommonBusiness _commonBusiness;
        private IPackingSlipRepository _packingSlipRepository;
        public PackingSlipBusiness(ICommonBusiness commonBusiness, IPackingSlipRepository packingSlipRepository)
        {
            _commonBusiness = commonBusiness;
            _packingSlipRepository = packingSlipRepository;
        }
        public List<PackingSlip> GetAllPackingSlip(PackingSlipAdvanceSearch paySlipAdvanceSearch)
        {
            return _packingSlipRepository.GetAllPackingSlip(paySlipAdvanceSearch);
        }
    }
}
