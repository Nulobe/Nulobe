using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Utility.Validation
{
    public class ClientArgumentNullException : ClientValidationException
    {
        public string ParamName { get; private set; }

        public ClientArgumentNullException(string paramName)
        {
            ParamName = paramName;
        }
    }
}
