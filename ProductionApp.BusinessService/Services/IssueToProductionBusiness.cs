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
        private ICommonBusiness _commonBusiness;
        public IssueToProductionBusiness(IIssueToProductionRepository issueToProductionRepository,ICommonBusiness commonBusiness)
        {
            _issueToProductionRepository = issueToProductionRepository;
            _commonBusiness = commonBusiness;
        }

        //    //public List<MaterialIssue> GetAllIssueToProduction(MaterialIssueAdvanceSearch materialAdvanceSearch)
        //    //{
        //    //    return _issueToProductionRepository.GetAllIssueToProduction(materialAdvanceSearch);
        //    //}

        public object InsertUpdateIssueToProduction(MaterialIssue materialIssue)
        {
            DetailsXMl(materialIssue);
            return _issueToProductionRepository.InsertUpdateIssueToProduction(materialIssue);
        }

        public void DetailsXMl(MaterialIssue materialIssue)
        {
            string result = "<Details>";
            int totalRows = 0;
            foreach (object some_object in materialIssue.MaterialIssueDetailList)
            {
                _commonBusiness.XML(some_object, ref result, ref totalRows);
            }
            result = result + "</Details>";

            materialIssue.DetailXML = result;
        }

        public MaterialIssue GetIssueToProduction(Guid ID)
        {
            return _issueToProductionRepository.GetIssueToProduction(ID);
        }

        public List<MaterialIssueDetail> GetIssueToProductionDetail(Guid ID)
        {
            return _issueToProductionRepository.GetIssueToProductionDetail(ID);
        }

        public object DeleteIssueToProductionDetail(Guid ID)
        {
            return _issueToProductionRepository.DeleteIssueToProductionDetail(ID);
        }

        public object DeleteIssueToProduction(Guid ID)
        {
            return _issueToProductionRepository.DeleteIssueToProduction(ID);
        }
    }
}
