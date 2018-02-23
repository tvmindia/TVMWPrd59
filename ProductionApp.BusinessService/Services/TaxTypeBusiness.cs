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
    public class TaxTypeBusiness: ITaxTypeBusiness
    {
        private ITaxTypeRepository _taxTypeRepository;
        public TaxTypeBusiness(ITaxTypeRepository taxTypeRepository)
        {
            _taxTypeRepository = taxTypeRepository;
        }
        public TaxType GetTaxTypeDetailsByCode(string Code)
        {
            return _taxTypeRepository.GetTaxTypeDetailsByCode(Code);
        }
    }
}