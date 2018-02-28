﻿using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Contracts
{
    public interface IDocumentApprovalBusiness
    {
        List<ApprovalHistory> GetApprovalHistory(Guid DocumentID, string DocumentTypeCode);
        List<DocumentApproval> GetAllDocumentsPendingForApprovals(DocumentApprovalAdvanceSearch documentApprovalAdvanceSearch);
        DataTable GetDocumentSummary(Guid DocumentID, string DocumentTypeCode);
    }
}
