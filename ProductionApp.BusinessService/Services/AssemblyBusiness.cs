using ProductionApp.BusinessService.Contracts;
using ProductionApp.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Services
{
    public class AssemblyBusiness : IAssemblyBusiness
    {
        private ICommonBusiness _commonBusiness;
        private IAssemblyRepository _assemblyRepository;
        public AssemblyBusiness(ICommonBusiness commonBusiness , IAssemblyRepository assemblyRepository)
        {
            _commonBusiness = commonBusiness;
            _assemblyRepository = assemblyRepository;
        }
    }
}
