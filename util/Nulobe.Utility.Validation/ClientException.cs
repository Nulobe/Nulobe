using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Utility.Validation
{
    public class ClientException : Exception
    {
        public ClientException(string message) : base(message)
        {
        }
    }
}
