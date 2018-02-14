using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductionApp.BusinessService.Contracts;
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

    }
}
