using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.RepositoryServices.Contracts;
using ProductionApp.RepositoryServices.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProductionApp.BusinessService.Services
{
    public class UnitBusiness:IUnitBusiness
    {
        private IUnitRepository _unitRepository;
        public UnitBusiness(IUnitRepository unitRepository)
        {
            _unitRepository = unitRepository;
        }

        public List<SelectListItem> GetUnitForSelectList()
        {
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            List<Unit> unitList = _unitRepository.GetUnitForSelectList();
            if (unitList != null)
                foreach (Unit unit in unitList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = unit.Description,
                        Value = unit.Code,
                        Selected = false
                    });
                }
            return selectListItem;
        }

    }
}
