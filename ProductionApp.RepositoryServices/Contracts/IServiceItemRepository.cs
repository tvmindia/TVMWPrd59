using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.RepositoryServices.Contracts
{
    public interface IServiceItemRepository
    { 
        List<ServiceItem> GetServiceItemsForSelectList();
        ServiceItem GetServiceItem(Guid id);
        List<ServiceItem> GetAllServiceItem(ServiceItemAdvanceSearch serviceItemAdvanceSearch);
        object InsertUpdateServiceItem(ServiceItem serviceItem);
        object DeleteServiceItem(Guid id, string deletedBy);
    }
}
