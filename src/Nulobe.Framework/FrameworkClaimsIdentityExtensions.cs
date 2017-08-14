using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Framework
{
    public static class FrameworkClaimsIdentityExtensions
    {
        public static string GetId(
            this ClaimsIdentity identity)
        {
            var nameClaim = identity.Claims
                .Where(c => c.Properties.Any((kvp) => kvp.Value.Equals("sub", StringComparison.InvariantCultureIgnoreCase)))
                .FirstOrDefault();

            if (nameClaim == null)
            {
                return null;
            }

            return $"{nameClaim.Issuer}|{nameClaim.Value}";
        }
    }
}
