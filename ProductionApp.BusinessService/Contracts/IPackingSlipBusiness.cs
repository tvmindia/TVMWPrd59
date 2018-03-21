using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Contracts
{
    public interface IPackingSlipBusiness
    {
        List<PackingSlip> GetAllPackingSlip(PackingSlipAdvanceSearch paySlipAdvanceSearch);
        object InsertUpdatePackingSlip(PackingSlip packingSlip);
        PackingSlip GetPackingSlipByID(Guid id);
        List<PackingSlipDetail> GetPackingSlipDetailByID(Guid id);
        List<PackingSlip> PackingSlipDetailByIDForEdit(Guid PkgSlipDetailID);
        object DeletePackingSlipDetail(Guid id);
        object DeletePackingSlip(Guid id);
        List<PackingSlip> GetPackingSlipForSelectList();


    }
}
