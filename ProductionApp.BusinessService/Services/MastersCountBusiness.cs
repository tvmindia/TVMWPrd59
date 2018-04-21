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
   public class MastersCountBusiness:IMastersCountBusiness
    {
        private IMastersCountRepository _mastersCountRepository;
        
        public MastersCountBusiness(IMastersCountRepository mastersCountRepository)
        {
            _mastersCountRepository = mastersCountRepository;
            
        }
        public List<MastersCount> GetRecentMastersCount()
        {
            return _mastersCountRepository.GetRecentMastersCount();
        }
    }
}
