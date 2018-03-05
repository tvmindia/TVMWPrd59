using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ProductionApp.BusinessService.Contracts;
using System.Globalization;

namespace ProductionApp.BusinessService.Services
{
    public class CommonBusiness: ICommonBusiness
    {
        public string ConvertCurrency(decimal value, int DecimalPoints = 0, bool Symbol = true)
        {
            string result = value.ToString();
            string fare = result;
            decimal parsed = decimal.Parse(fare, CultureInfo.InvariantCulture);
            CultureInfo hindi = new CultureInfo("hi-IN");
            if (Symbol)
                result = string.Format(hindi, "{0:C" + DecimalPoints + "}", parsed);
            else
            {
                if (DecimalPoints == 0)
                { result = string.Format(hindi, "{0:#,#.##}", parsed); }
                else
                { result = string.Format(hindi, "{0:#,#0.00}", parsed); }
            }
            return result;

        }

        public void XML(object some_object, ref string result, ref int totalRows)
        {
            var properties = GetProperties(some_object);
            result = result + "<item ";
            foreach (var p in properties)
            {
                string name = p.Name;
                var value = p.GetValue(some_object, null);
                result = result + " " + name + @"=""" + value + @""" ";
            }
            result = result + "></item>";
            totalRows = totalRows + 1;
        }
        
        private static PropertyInfo[] GetProperties(object obj)
        {
            return obj.GetType().GetProperties();
        }
    }
}
