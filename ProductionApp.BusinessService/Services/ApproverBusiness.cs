using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Services
{
    public class ApproverBusiness: IApproverBusiness
    {
        private IApproverRepository _approverRepository;

        public ApproverBusiness(IApproverRepository approverRepository)
        {
            _approverRepository = approverRepository;
        }
        public List<Approver> GetAllApprover(ApproverAdvanceSearch approverAdvanceSearch)
        {
            return _approverRepository.GetAllApprover(approverAdvanceSearch);
        }
        public object InsertUpdateApprover(Approver approver)
        {
            return _approverRepository.InsertUpdateApprover(approver);
        }
        public Approver GetApprover(Guid id)
        {
            return _approverRepository.GetApprover(id);
        }
        public object DeleteApprover(Guid id)
        {
            return _approverRepository.DeleteApprover(id);
        }

    }
}
