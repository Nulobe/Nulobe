using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Configuration
{
    public static class FrameworkConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddCountriesJsonFile(
            this IConfigurationBuilder builder)
        {
            return builder.AddJsonFile("appsettings.Shared.Countries.json", optional: false);
        }
    }
}
