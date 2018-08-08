using ProductionApp.BusinessService.Contracts;
using ProductionApp.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductionApp.DataAccessObject.DTO;

namespace ProductionApp.BusinessService.Services
{
    public class ServiceItemsBusiness : IServiceItemsBusiness
    {

        private IServiceItemsRepository _serviceItemsRepository;
        private ICommonBusiness _commonBusiness;
        public ServiceItemsBusiness(IServiceItemsRepository serviceItemsRepository, ICommonBusiness commonBusiness)
        {
            _serviceItemsRepository = serviceItemsRepository;
            _commonBusiness = commonBusiness;

        }

        public List<ServiceItems> GetServiceItemsForSelectList()
        {
            return _serviceItemsRepository.GetServiceItemsForSelectList();
        }

        public ServiceItems GetServiceItem(Guid id)
        {
            return _serviceItemsRepository.GetServiceItem(id);
        }
    }
}
