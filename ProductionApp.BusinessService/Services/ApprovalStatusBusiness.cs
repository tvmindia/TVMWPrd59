using ProductionApp.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Services
{
    public class ApprovalStatusBusiness : IApprovalStatusBusiness
    {
        private IApprovalStatusRepository _approvalStatusRepository;
        public ApprovalStatusBusiness(IApprovalStatusRepository approvalStatusRepository)
        {
            _approvalStatusRepository = approvalStatusRepository;
        }
    }
}
