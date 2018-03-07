﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductionApp.DataAccessObject.DTO;

namespace ProductionApp.RepositoryServices.Contracts
{
   public interface IMaterialStockAdjRepository
    {
        List<MaterialStockAdj> GetAllMaterialStockAdjustment(MaterialStockAdjAdvanceSearch materialStockAdjAdvanceSearch);
    }
}
