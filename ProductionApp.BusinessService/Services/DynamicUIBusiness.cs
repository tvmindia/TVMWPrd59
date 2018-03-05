using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductionApp.BusinessService.Contracts;
using ProductionApp.RepositoryServices.Contracts;
using ProductionApp.DataAccessObject.DTO;

namespace ProductionApp.BusinessService.Services
{
    public class DynamicUIBusiness : IDynamicUIBusiness
    {
        private IDynamicUIRepository _dynamicUIRepository;
        public DynamicUIBusiness(IDynamicUIRepository dynamicUIRespository)
        {
            _dynamicUIRepository = dynamicUIRespository;

        }
        public List<AMCSysMenu> GetAllMenu(string code)
        {
            try
            {
                return _dynamicUIRepository.GetAllMenu(code);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        public List<AMCSysModule> GetAllModule()
        {
            return _dynamicUIRepository.GetAllModule();
        }

        public List<IncomeExpenseSummary> GetIncomeExpenseSummary()
        {
            return _dynamicUIRepository.GetIncomeExpenseSummary();
        }
    }
}
