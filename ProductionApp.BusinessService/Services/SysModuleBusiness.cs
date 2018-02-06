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
    public class SysModuleBusiness: ISysModuleBusiness
    {
        ISysModuleRepository _sysModuleRepository;
        public SysModuleBusiness(ISysModuleRepository sysModuleRepository)
        {
            _sysModuleRepository = sysModuleRepository;
        }
        public List<SysModule> GetAllModule()
        {
            return _sysModuleRepository.GetAllModule();
        }
    }
}
