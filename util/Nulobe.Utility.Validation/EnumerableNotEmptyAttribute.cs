using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Utility.Validation
{
    public class EnumerableNotEmptyAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var enumerable = (IEnumerable)value;
            return enumerable.OfType<dynamic>().Count() > 0;
        }
    }
}
