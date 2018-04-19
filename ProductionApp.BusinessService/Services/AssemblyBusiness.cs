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
    public class AssemblyBusiness : IAssemblyBusiness
    {
        private ICommonBusiness _commonBusiness;
        private IAssemblyRepository _assemblyRepository;
        public AssemblyBusiness(ICommonBusiness commonBusiness , IAssemblyRepository assemblyRepository)
        {
            _commonBusiness = commonBusiness;
            _assemblyRepository = assemblyRepository;
        }
        public List<Assembly> GetAllAssembly(AssemblyAdvanceSearch assemblyAdvanceSearch)
        {
            return _assemblyRepository.GetAllAssembly(assemblyAdvanceSearch);
        }
        public List<Assembly> GetProductComponentList(Guid id, decimal qty, Guid assemblyId)
        {
            return _assemblyRepository.GetProductComponentList(id, qty, assemblyId);
        }
        public object InsertUpdateAssembly(Assembly assembly)
        {
            return _assemblyRepository.InsertUpdateAssembly(assembly);
        }
        public Assembly GetAssembly(Guid id)
        {
            return _assemblyRepository.GetAssembly(id);
        }
        public object DeleteAssembly(Guid id, string createdBy)
        {
            return _assemblyRepository.DeleteAssembly(id, createdBy);
        }
    }
}
