using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Contracts
{
    public interface IMaterialReturnFromProductionBusiness
    {
        List<MaterialReturnFromProduction> GetAllReturnFromProduction(MaterialReturnFromProductionAdvanceSearch materialReturnAdvanceSearch);
        object InsertUpdateReturnFromProduction(MaterialReturnFromProduction materialReturnFromProduction);
    }
}
