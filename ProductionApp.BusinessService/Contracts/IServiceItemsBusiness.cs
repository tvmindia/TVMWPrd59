﻿using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Contracts
{
    public interface IServiceItemsBusiness
    {
        List<ServiceItems> GetServiceItemsForSelectList();
        ServiceItems GetServiceItem(Guid id);

    }
}
