﻿using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProductionApp.RepositoryServices.Contracts
{
    public interface IOtherExpenseRepository
    {
        List<OtherExpense> GetAllOtherExpense(OtherExpenseAdvanceSearch otherExpenseAdvanceSearch);
        object InsertUpdateOtherExpense(OtherExpense otherExpense);
        OtherExpense GetOtherExpense(Guid id);
        List<SelectListItem> GetAccountSubHeadForSelectList();
        List<OtherExpense> GetReversalReference(string accountCode);
        object DeleteOtherExpense(Guid id, string userName);
    }
}
