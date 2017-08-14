using Nulobe.Utility.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Framework
{
    public class ClientUnauthenticatedException : ClientException
    {
        public ClientUnauthenticatedException() : base("User is not authenticated")
        {
        }
    }
}
