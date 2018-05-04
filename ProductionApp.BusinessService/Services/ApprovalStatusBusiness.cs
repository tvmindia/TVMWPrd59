using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProductionApp.BusinessService.Services
{
    public class ApprovalStatusBusiness : IApprovalStatusBusiness
    {
        private IApprovalStatusRepository _approvalStatusRepository;
        public ApprovalStatusBusiness(IApprovalStatusRepository approvalStatusRepository)
        {
            _approvalStatusRepository = approvalStatusRepository;
        }
        public List<SelectListItem> GetApprovalStatusForSelectList()
        {
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            List<ApprovalStatus> statusList = _approvalStatusRepository.GetApprovalStatusForSelectList();
            if (statusList != null)
                foreach (ApprovalStatus unit in statusList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = unit.Description,
                        Value = unit.ID.ToString(),
                        Selected = false
                    });
                }
            return selectListItem;
        }
    }
}
