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
    public class OtherExpenseBusiness : IOtherExpenseBusiness
    {
        private IOtherExpenseRepository _otherExpenseRepository;
        private ICommonBusiness _commonBusiness;
        public OtherExpenseBusiness(ICommonBusiness commonBusiness, IOtherExpenseRepository otherExpenseRepository)
        {
            _commonBusiness = commonBusiness;
            _otherExpenseRepository = otherExpenseRepository;
        }
        public List<OtherExpense> GetAllOtherExpense(OtherExpenseAdvanceSearch otherExpenseAdvanceSearch)
        {
            return _otherExpenseRepository.GetAllOtherExpense(otherExpenseAdvanceSearch);
        }
        public object InsertUpdateOtherExpense(OtherExpense otherExpense)
        {
            return _otherExpenseRepository.InsertUpdateOtherExpense(otherExpense);
        }
        public OtherExpense GetOtherExpense(Guid id)
        {

            OtherExpense otherExpense = null;
            try
            {
                otherExpense = _otherExpenseRepository.GetOtherExpense(id);
                otherExpense.InvoiceAmountWords = _commonBusiness.NumberToWords(double.Parse((otherExpense.Amount).ToString()));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return otherExpense; 
        }
        public List<SelectListItem> GetAccountSubHeadForSelectList()
        {
            return _otherExpenseRepository.GetAccountSubHeadForSelectList();
        }
        public List<OtherExpense> GetReversalReference(string accountCode)
        {
            return _otherExpenseRepository.GetReversalReference(accountCode);
        }
        public decimal GetMaximumReducibleAmount(string refNumber)
        {
            List<OtherExpense> otherExpenseList = null;
            OtherExpenseAdvanceSearch otherExpenseAdvanceSearch = new OtherExpenseAdvanceSearch();
            otherExpenseAdvanceSearch.DataTablePaging = new DataTablePaging();
            otherExpenseAdvanceSearch.DataTablePaging.Length = -1;
            decimal HeadAmount = 0;
            try
            {
                otherExpenseList = GetAllOtherExpense(otherExpenseAdvanceSearch);
                List<decimal> amountList = otherExpenseList != null ?
                    (from otherExpense in otherExpenseList
                     where otherExpense.ReversalRef == refNumber
                     select otherExpense.Amount).ToList()
                    : null;
                if (amountList != null)
                {
                    foreach (int amount in amountList)
                    {
                        HeadAmount -= amount;
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return HeadAmount;
        }
        public object DeleteOtherExpense(Guid id,string userName)
        {
            return _otherExpenseRepository.DeleteOtherExpense(id,userName);
        }
    }
}
