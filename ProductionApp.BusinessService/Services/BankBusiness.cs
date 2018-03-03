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
        public List<Bank> GetBankForSelectList()
        {
            return _bankRepository.GetBankForSelectList();
        }
        public List<Bank> GetAllBank(BankAdvanceSearch bankAdvanceSearch)
        {
            return _bankRepository.GetAllBank(bankAdvanceSearch);
        }
        public object InsertUpdateBank(Bank bank)
        {
            return _bankRepository.InsertUpdateBank(bank);
        }
        public Bank GetBank(string code)
        {
            return _bankRepository.GetBank(code);
        }
        public bool CheckCodeExist(string code)
        {
            return _bankRepository.CheckCodeExist(code);
        }
        public object DeleteBank(string code)
        {
            return _bankRepository.DeleteBank(code);
        }
    }
}
