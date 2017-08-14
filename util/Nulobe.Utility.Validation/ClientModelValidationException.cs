using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Utility.Validation
{
    public class ClientModelValidationException : ClientValidationException
    {
        public ModelErrorDictionary ModelErrors { get; private set; }

        public ClientModelValidationException(ModelErrorDictionary modelErrors)
        {
            ModelErrors = modelErrors;
        }
    }
}
