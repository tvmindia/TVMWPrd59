using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.RepositoryServices.Contracts;
using System.Reflection;

namespace ProductionApp.BusinessService.Services
{
    public class IssueToProductionBusiness : IIssueToProductionBusiness
    {
        private IIssueToProductionRepository _issueToProductionRepository;
        public IssueToProductionBusiness(IIssueToProductionRepository issueToProductionRepository)
        {
            _issueToProductionRepository = issueToProductionRepository;
        }

        //    //public List<MaterialIssue> GetAllIssueToProduction(MaterialIssueAdvanceSearch materialAdvanceSearch)
        //    //{
        //    //    return _issueToProductionRepository.GetAllIssueToProduction(materialAdvanceSearch);
        //    //}

        public object InsertUpdateIssueToProduction(MaterialIssue materialIssue)
        {
            // DetailXML(materialIssue);
            return _issueToProductionRepository.InsertUpdateIssueToProduction(materialIssue);
        }
    }
}
