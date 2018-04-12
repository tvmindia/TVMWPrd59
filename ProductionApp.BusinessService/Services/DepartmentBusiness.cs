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
    public class DepartmentBusiness : IDepartmentBusiness
    {
        private ICommonBusiness _commonBusiness;
        private IDepartmentRepository _departmentRepository;
        public DepartmentBusiness(ICommonBusiness commonBusiness, IDepartmentRepository departmentRepository)
        {
            _commonBusiness = commonBusiness;
            _departmentRepository = departmentRepository;
        }
        public List<SelectListItem> GetDepartmentForSelectList()
        {
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            List<Department> departmentList = _departmentRepository.GetDepartmentForSelectList();
            if (departmentList != null)
                foreach (Department department in departmentList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = department.Name,
                        Value = department.Code,
                        Selected = false
                    });
                }
            return selectListItem;
        }
    }
}
