using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterworksCaseStudy.Converters
{

    public class BoolConverter : ConverterBase
    {
        public override object StringToField(string from)
        {
            var check = from.ToLower();
            if (check.StartsWith("f") || check.StartsWith("0") || check.Length == 0)
                return false;
            else if (check.StartsWith("t") || check.StartsWith("1") )
            {
                return true;
            }
            else
            {   
                return false;
            }
        }

        public override string FieldToString(object fieldValue)
        {
            return ((bool)fieldValue).ToString(); ;
        }

    }
}
