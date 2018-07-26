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
    public class OtherIncomeBusiness: IOtherIncomeBusiness
    {
        #region Constructor Injection
        private IOtherIncomeRepository _otherIncomeRepository;
        public OtherIncomeBusiness(IOtherIncomeRepository otherIncomeRepository)
        {
            _otherIncomeRepository = otherIncomeRepository;
        }
        #endregion Constructor Injection

        public List<OtherIncome> GetAllOtherIncome(OtherIncomeAdvanceSearch otherIncomeAdvanceSearch)
        {
            return _otherIncomeRepository.GetAllOtherIncome(otherIncomeAdvanceSearch);
        }

        public object InsertUpdateOtherIncome(OtherIncome otherIncome)
        {
            return _otherIncomeRepository.InsertUpdateOtherIncome(otherIncome);
        }

        public OtherIncome GetOtherIncome(Guid id)
        {
            return _otherIncomeRepository.GetOtherIncome(id);
        }
        
        public List<SelectListItem> GetAllAccountSubHeadForSelectList()
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            try
            {
                List<string> subHeadList = _otherIncomeRepository.GetAllAccountSubHeadForSelectList();
                if (subHeadList != null)
                {
                    selectList = new List<SelectListItem>();
                    foreach (string subHead in subHeadList)
                    {
                        selectList.Add(new SelectListItem()
                        {
                            Text = subHead,
                            Value = subHead,
                            Selected = false
                        });
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return selectList;
        }

        public object DeleteOtherIncome(Guid id,string UserName)
        {
            return _otherIncomeRepository.DeleteOtherIncome(id, UserName);
        }
    }
}
