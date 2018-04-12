using ProductionApp.BusinessService.Contracts;
using ProductionApp.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Services
{
    public class EmployeeCategoryBusiness : IEmployeeCategoryBusiness
    {
        private ICommonBusiness _commonBusiness;
        private IEmployeeCategoryRepository _employeeCategoryRepository;
        public EmployeeCategoryBusiness(ICommonBusiness commonBusiness, IEmployeeCategoryRepository employeeCategoryRepository)
        {
            _commonBusiness = commonBusiness;
            _employeeCategoryRepository = employeeCategoryRepository;
        }
    }
}
