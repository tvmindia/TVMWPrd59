using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductionApp.DataAccessObject.DTO;

namespace ProductionApp.RepositoryServices.Contracts
{
    public interface IIssueToProductionRepository
    {
        List<MaterialIssue> GetAllIssueToProduction(MaterialIssueAdvanceSearch materialIssueAdvanceSearch);
        object InsertUpdateIssueToProduction(MaterialIssue materialIssue);
        MaterialIssue GetIssueToProduction(Guid id);
        List<MaterialIssueDetail> GetIssueToProductionDetail(Guid id);
        object DeleteIssueToProductionDetail(Guid id);
        object DeleteIssueToProduction(Guid id);
    }
}
