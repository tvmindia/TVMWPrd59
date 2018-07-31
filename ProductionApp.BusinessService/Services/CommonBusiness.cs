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
                result = result + " " + name + @"=""" + (value != null ? value.ToString().Replace("\"", "&quot;").Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;") : value) + @""" ";
            }
            result = result + "></item>";
            totalRows = totalRows + 1;
        }
        
        private static PropertyInfo[] GetProperties(object obj)
        {
            return obj.GetType().GetProperties();
        }

        public string NumberToWords(double number)
        {

            try
            {

                string[] numbersArr = new string[] { "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                string[] tensArr = new string[] { "Twenty", "Thirty", "Fourty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninty" };
                string[] suffixesArr = new string[] { "Thousand", "Million", "Billion", "Trillion", "Quadrillion", "Quintillion", "Sextillion", "Septillion", "Octillion", "Nonillion", "Decillion", "Undecillion", "Duodecillion", "Tredecillion", "Quattuordecillion", "Quindecillion", "Sexdecillion", "Septdecillion", "Octodecillion", "Novemdecillion", "Vigintillion" };
                string words = "";

                bool tens = false;

                if (number < 0)
                {
                    words += "negative ";
                    number *= -1;
                }

                int power = (suffixesArr.Length + 1) * 3;

                while (power > 3)
                {
                    double pow = Math.Pow(10, power);
                    if (number >= pow)
                    {
                        if (number % pow > 0)
                        {
                            words += NumberToWords(Math.Floor(number / pow)) + " " + suffixesArr[(power / 3) - 1] + ", ";
                        }
                        else if (number % pow == 0)
                        {
                            words += NumberToWords(Math.Floor(number / pow)) + " " + suffixesArr[(power / 3) - 1];
                        }
                        number %= pow;
                    }
                    power -= 3;
                }
                if (number >= 1000)
                {
                    if (number % 1000 > 0) words += NumberToWords(Math.Floor(number / 1000)) + " Thousand, ";
                    else words += NumberToWords(Math.Floor(number / 1000)) + " Thousand";
                    number %= 1000;
                }
                if (0 <= number && number <= 999)
                {
                    if ((int)number / 100 > 0)
                    {
                        words += NumberToWords(Math.Floor(number / 100)) + " Hundred";
                        number %= 100;
                    }
                    if ((int)number / 10 > 1)
                    {
                        if (words != "")
                            words += " and ";
                        words += tensArr[(int)number / 10 - 2];
                        tens = true;
                        number %= 10;
                    }

                    if (number < 20 && number >= 1)
                    {
                        if (words != "" && tens == false)
                            words += " and ";
                        words += (tens ? "-" + numbersArr[(int)number - 1] : numbersArr[(int)number - 1]);
                        number -= Math.Floor(number);
                    }
                }

                return words;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
