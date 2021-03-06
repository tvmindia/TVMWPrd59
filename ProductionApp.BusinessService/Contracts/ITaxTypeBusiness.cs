﻿using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Contracts
{
    public interface ITaxTypeBusiness
    {
        TaxType GetTaxTypeDetailsByCode(string Code);
        List<TaxType> GetTaxTypeForSelectList();
    }
}
