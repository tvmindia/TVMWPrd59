using ProductionApp.BusinessService.Contracts;
using ProductionApp.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Services
{
    public class DepartmentBusiness : IDepartmentBusiness
    {
        private ICommonBusiness _commonBusiness;
        private IDepartmentRepository _departmentRepository;
        public DepartmentBusiness(ICommonBusiness commonBusiness, IDepartmentRepository departmentRepository)
        {
            _commonBusiness = commonBusiness;
            _departmentRepository = departmentRepository;
        }
    }
}
