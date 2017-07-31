using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Configuration
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddConfigurationSources<TMarker>(
            this IConfigurationBuilder builder,
            IHostingEnvironment hostingEnvironment)
            where TMarker : class
        {
            builder
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", optional: true);

            if (hostingEnvironment.IsDevelopment())
            {
                builder.AddUserSecrets<TMarker>();
            }

            builder
                .AddJsonFile("appsettings.Shared.json", optional: false)
                .AddJsonFile($"appsettings.Shared.{hostingEnvironment.EnvironmentName}.json", optional: true);

            return builder;
        }
    }
}
