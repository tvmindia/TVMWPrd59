﻿using System;
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
        Requisition GetRequisition(Guid ID);
        object InsertUpdateRequisition(Requisition requisition);
        List<Requisition> GetAllRequisitionForPurchaseOrder(RequisitionAdvanceSearch requisitionAdvanceSearch);
        List<RequisitionDetail> GetRequisitionDetailsByIDs(string IDs, string POID);
    }
}
