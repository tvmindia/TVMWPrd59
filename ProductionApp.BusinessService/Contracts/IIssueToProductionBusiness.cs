﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductionApp.DataAccessObject.DTO;

namespace ProductionApp.BusinessService.Contracts
{
    public interface IIssueToProductionBusiness
    {
        //List<MaterialIssue> GetAllIssueToProduction(MaterialIssueAdvanceSearch materialAdvanceSearch);
        object InsertUpdateIssueToProduction(MaterialIssue materialIssue);
    }
}