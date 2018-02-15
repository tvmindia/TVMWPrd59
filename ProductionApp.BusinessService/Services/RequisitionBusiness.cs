using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.RepositoryServices.Contracts;

namespace ProductionApp.BusinessService.Services
{
    public class RequisitionBusiness: IRequisitionBusiness
    {
        private IRequisitionRepository _requisitionRepository;
        public RequisitionBusiness(IRequisitionRepository requisitionRepository)
        {
            _requisitionRepository = requisitionRepository;
        }

        public List<Requisition> GetAllRequisition(RequisitionAdvanceSearch requisitionAdvanceSearch)
        {
            return _requisitionRepository.GetAllRequisition(requisitionAdvanceSearch);
        }
    }
}
