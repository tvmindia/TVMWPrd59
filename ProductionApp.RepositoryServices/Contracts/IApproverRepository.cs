﻿using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.RepositoryServices.Contracts
{
    public interface IApproverRepository
    {
        List<Approver> GetAllApprover(ApproverAdvanceSearch approverAdvanceSearch);
        object InsertUpdateApprover(Approver approver);
        Approver GetApprover(Guid id);
        object DeleteApprover(Guid id);
    }
}
