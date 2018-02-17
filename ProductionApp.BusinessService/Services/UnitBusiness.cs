using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.RepositoryServices.Contracts;
using ProductionApp.RepositoryServices.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Services
{
    public class UnitBusiness:IUnitBusiness
    {
        private IUnitRepository _unitRepository;
        public UnitBusiness(IUnitRepository unitRepository)
        {
            _unitRepository = unitRepository;
        }

        public List<Unit> GetUnitForSelectList()
        {
            return _unitRepository.GetUnitForSelectList();
        }
    }
}
