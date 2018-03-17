using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.RepositoryServices.Contracts
{
    public interface IPackingSlipRepository
    {
        List<PackingSlip> GetAllPackingSlip(PackingSlipAdvanceSearch paySlipAdvanceSearch);
        object InsertUpdatePackingSlip(PackingSlip packingSlip);
        PackingSlip GetPkgSlipByID(Guid id);
        List<PackingSlipDetail> GetPkgSlipDetailByID(Guid id);
    }
}
