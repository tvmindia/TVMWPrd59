using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Contracts
{
    public interface ICommonBusiness
    {
        void XML(object some_object, ref string result, ref int totalRows);
        string ConvertCurrency(decimal value, int DecimalPoints = 0, bool Symbol = true);
    }
}
