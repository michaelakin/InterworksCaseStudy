using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterworksCaseStudy.Converters
{
    class TimeConverter : ConverterBase
    {
        public override object StringToField(string from)
        {
            var tempFrom = from;
            if (tempFrom == "2400")
                tempFrom = "2359";

            if (tempFrom.Trim() == string.Empty)
                return new DateTime();

            string newFrom = tempFrom.PadLeft(4, '0');
            int hours = Convert.ToInt32(newFrom.Substring(0, 2));
            int min = Convert.ToInt32(newFrom.Substring(2, 2));
            DateTime datetime = new DateTime(1, 1, 1, hours, min, 0);
            return datetime;
        }
    }
}
