using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Contracts
{
    public interface IAssemblyBusiness
    {
        List<Assembly> GetAllAssembly(AssemblyAdvanceSearch assemblyAdvanceSearch);
        List<Assembly> GetProductComponentList(Guid id, decimal qty, Guid assemblyId);
        object InsertUpdateAssembly(Assembly assembly);
        Assembly GetAssembly(Guid id);
        object DeleteAssembly(Guid id, string createdBy);
    }
}
