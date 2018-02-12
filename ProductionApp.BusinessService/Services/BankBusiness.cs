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
    public class BankBusiness:IBankBusiness
    {
        private IBankRepository _bankRepository;
        public BankBusiness(IBankRepository bankRepository)
        {
            _bankRepository = bankRepository;
        }
        public List<Bank> GetAllBank(BankAdvanceSearch bankAdvanceSearch)
        {
            return _bankRepository.GetAllBank(bankAdvanceSearch);
        }
    }
}
