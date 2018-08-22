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
    public class ServiceItemBusiness : IServiceItemBusiness
    {

        private IServiceItemRepository _serviceItemRepository;
        private ICommonBusiness _commonBusiness;
        public ServiceItemBusiness(IServiceItemRepository serviceItemsRepository, ICommonBusiness commonBusiness)
        {
            _serviceItemRepository = serviceItemsRepository;
            _commonBusiness = commonBusiness;

        }

        public List<ServiceItem> GetServiceItemsForSelectList()
        {
            return _serviceItemRepository.GetServiceItemsForSelectList();
        }

        public ServiceItem GetServiceItem(Guid id)
        {
            return _serviceItemRepository.GetServiceItem(id);
        }

        public List<ServiceItem> GetAllServiceItem(ServiceItemAdvanceSearch serviceItemAdvanceSearch)
        {
            return _serviceItemRepository.GetAllServiceItem(serviceItemAdvanceSearch);
        }

        public object InsertUpdateServiceItem(ServiceItem serviceItem)
        {
            return _serviceItemRepository.InsertUpdateServiceItem(serviceItem);
        }

        public object DeleteServiceItem(Guid id, string deletedBy)
        {
            return _serviceItemRepository.DeleteServiceItem(id, deletedBy);
        }
    }
}
