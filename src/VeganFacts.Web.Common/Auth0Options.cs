using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VeganFacts.Web.Common
{
    public class Auth0Options
    {
        public string Domain { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }
    }
}
