﻿using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.RepositoryServices.Contracts
{
    public interface IDynamicUIRepository
    {
        List<AMCSysModule> GetAllModule();
        List<AMCSysMenu> GetAllMenu(string code);
        List<IncomeExpenseSummary> GetIncomeExpenseSummary();
        List<ProductionSummary> GetProductionSummary();
        string GetModuleName(string code);
    }
}
