using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Utility.Validation
{
    public class ClientArgumentException : ClientValidationException
    {
        public string ParamName { get; private set; }

        public ClientArgumentException(string paramName)
        {
            ParamName = paramName;
        }
    }
}
