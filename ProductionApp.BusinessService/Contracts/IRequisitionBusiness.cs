using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductionApp.DataAccessObject.DTO;

namespace ProductionApp.BusinessService.Contracts
{
    public interface IRequisitionBusiness
    {
        List<Requisition> GetAllRequisition(RequisitionAdvanceSearch requisitionAdvanceSearch);
        object InsertUpdateRequisition(Requisition requisition);
    }
}
