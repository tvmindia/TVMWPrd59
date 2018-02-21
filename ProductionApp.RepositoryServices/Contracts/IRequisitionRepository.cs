using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductionApp.DataAccessObject.DTO;

namespace ProductionApp.RepositoryServices.Contracts
{
    public interface IRequisitionRepository
    {
        List<Requisition> GetAllRequisition(RequisitionAdvanceSearch requisitionAdvanceSearch);
        Requisition GetRequisition(Guid ID);
        List<RequisitionDetail> GetRequisitionDetail(Guid ID);
        object InsertUpdateRequisition(Requisition requisition);
        List<Requisition> GetAllRequisitionForPurchaseOrder(RequisitionAdvanceSearch requisitionAdvanceSearch);
        List<RequisitionDetail> GetRequisitionDetailsByIDs(string IDs, string POID);
        object DeleteRequisitionDetail(Guid ID);
        object DeleteRequisition(Guid ID);

    }
}
