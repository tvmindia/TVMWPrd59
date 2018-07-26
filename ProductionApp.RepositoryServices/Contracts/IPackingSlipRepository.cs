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
        PackingSlip GetPackingSlip(Guid id);
        List<PackingSlipDetail> GetPackingSlipDetail(Guid id);
        List<PackingSlip> PackingSlipDetailByPackingSlipDetailID(Guid PkgSlipDetailID);
        object DeletePackingSlip(Guid id);
        object DeletePackingSlipDetail(Guid id, string isGroupItem);
        List<PackingSlip> GetPackingSlipForSelectList();
        List<PackingSlip> GetRecentPackingSlip();
        List<SalesOrderDetail> GetPackingSlipDetailGroupEdit(Guid groupID, Guid packingSlipID,Guid saleOrderID);
    }
}
