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
    public class EmployeeCategoryBusiness : IEmployeeCategoryBusiness
    {
        private ICommonBusiness _commonBusiness;
        private IEmployeeCategoryRepository _employeeCategoryRepository;
        public EmployeeCategoryBusiness(ICommonBusiness commonBusiness, IEmployeeCategoryRepository employeeCategoryRepository)
        {
            _commonBusiness = commonBusiness;
            _employeeCategoryRepository = employeeCategoryRepository;
        }
        public List<SelectListItem> GetEmployeeCategoryForSelectList()
        {
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            List<EmployeeCategory> employeeCategoryList = _employeeCategoryRepository.GetEmployeeCategoryForSelectList();
            if (employeeCategoryList != null)
                foreach (EmployeeCategory employeeCategory in employeeCategoryList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = employeeCategory.Name,
                        Value = employeeCategory.Code,
                        Selected = false
                    });
                }
            return selectListItem;
        }
    }
}
