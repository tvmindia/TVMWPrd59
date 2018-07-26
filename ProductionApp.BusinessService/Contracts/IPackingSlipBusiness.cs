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
        PackingSlip GetPackingSlip(Guid id);
        List<PackingSlipDetail> GetPackingSlipDetail(Guid id);
        List<PackingSlip> PackingSlipDetailByPackingSlipDetailID(Guid PkgSlipDetailID);
        object DeletePackingSlipDetail(Guid id, string isGroupItem);
        object DeletePackingSlip(Guid id);
        List<PackingSlip> GetPackingSlipForSelectList();
        List<PackingSlip> GetRecentPackingSlip(string BaseURL);
        List<SalesOrderDetail> GetPackingSlipDetailGroupEdit(Guid groupID, Guid packingSlipID,Guid saleOrderID);
    }
}
