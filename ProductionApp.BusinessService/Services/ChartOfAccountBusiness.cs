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
    public class ChartOfAccountBusiness : IChartOfAccountBusiness
    {
        private IChartOfAccountRepository _chartOfAccountRepository;
        private ICommonBusiness _commonBusiness;

        public ChartOfAccountBusiness(IChartOfAccountRepository chartOfAccountRepository, ICommonBusiness commonBusiness)
        {
            _chartOfAccountRepository = chartOfAccountRepository;
            _commonBusiness = commonBusiness;
        }
        public List<SelectListItem> GetChartOfAccountForSelectList(string type)
        {
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            List<ChartOfAccount> chartOfAccountList = _chartOfAccountRepository.GetChartOfAccountForSelectList(type);
            if (chartOfAccountList != null)
                foreach (ChartOfAccount chartOfAccount in chartOfAccountList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = chartOfAccount.Code + " - " + chartOfAccount.TypeDesc,
                        Value = chartOfAccount.Code+"|"+chartOfAccount.IsSubHeadApplicable,
                        Selected = false
                    });
                }
            return selectListItem;
        }
        public List<ChartOfAccount> GetAllChartOfAccount(ChartOfAccountAdvanceSearch chartOfAccountAdvanceSearch)
        {
            return _chartOfAccountRepository.GetAllChartOfAccount(chartOfAccountAdvanceSearch);
        }
        public object InsertUpdateChartOfAccount(ChartOfAccount chartOfAccount)
        {
            return _chartOfAccountRepository.InsertUpdateChartOfAccount(chartOfAccount);
        }
        //public bool CheckChartOfAccountTypeExist(ChartOfAccount chartOfAccount)
        //{
        //    return _chartOfAccountRepository.CheckChartOfAccountTypeExist(chartOfAccount);
        //}
        public ChartOfAccount GetChartOfAccount(string code)
        {
            return _chartOfAccountRepository.GetChartOfAccount(code);
        }
        public object DeleteChartOfAccount(string code)
        {
            return _chartOfAccountRepository.DeleteChartOfAccount(code);
        }
    }
}
