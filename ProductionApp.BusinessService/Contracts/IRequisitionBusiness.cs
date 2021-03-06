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
        List<RequisitionDetail> GetRequisitionDetail(Guid ID);
        object InsertUpdateRequisition(Requisition requisition);
        List<Requisition> GetAllRequisitionForPurchaseOrder();
        List<RequisitionDetail> GetRequisitionDetailsByIDs(string IDs, Guid POID);
        object DeleteRequisitionDetail(Guid ID);
        object DeleteRequisition(Guid ID);
        List<Requisition> GetRecentRequisition(string BaseURL);       
    }
}
