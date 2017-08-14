using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Utility.Validation
{
    public class ClientValidationException : ClientException
    {
        public ClientValidationException()
        {
        }

        public ClientValidationException(string message) : base(message)
        {
        }

        public ClientValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ClientValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
