using ProductionApp.BusinessService.Contracts;
using ProductionApp.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Services
{
    public class OtherExpenseBusiness : IOtherExpenseBusiness
    {
        private IOtherExpenseRepository _otherExpenseRepository;
        private ICommonBusiness _commonBusiness;
        public OtherExpenseBusiness(ICommonBusiness commonBusiness, IOtherExpenseRepository otherExpenseRepository)
        {
            _commonBusiness = commonBusiness;
            _otherExpenseRepository = otherExpenseRepository;
        }
    }
}
