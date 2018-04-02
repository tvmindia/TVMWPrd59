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
    public class SubComponentBusiness: ISubComponentBusiness
    {
        private ISubComponentRepository _subComponentRepository;
        private ICommonBusiness _commonBusiness;

        public SubComponentBusiness(ISubComponentRepository subComponentRepository, ICommonBusiness commonBusiness)
        {
            _subComponentRepository = subComponentRepository;
            _commonBusiness = commonBusiness;
        }
        public List<SubComponent> GetAllSubComponent(SubComponentAdvanceSearch subComponentAdvanceSearch)
        {
            return _subComponentRepository.GetAllSubComponent(subComponentAdvanceSearch);
        }
        //public bool CheckSubComponentCodeExist(string subComponentCode)
        //{
        //    return _subComponentRepository.CheckSubComponentCodeExist(subComponentCode);
        //}
        public object InsertUpdateSubComponent(SubComponent subComponent)
        {
            return _subComponentRepository.InsertUpdateSubComponent(subComponent);
        }
        public SubComponent GetSubComponent(Guid id)
        {
            return _subComponentRepository.GetSubComponent(id);
        }
        public object DeleteSubComponent(Guid id)
        {
            return _subComponentRepository.DeleteSubComponent(id);
        }
        public List<SubComponent> GetSubComponentForSelectList()
        {
            return _subComponentRepository.GetSubComponentForSelectList();
        }
    }
}
