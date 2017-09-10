using Microsoft.Extensions.Configuration;
using Nulobe.Framework;
using Nulobe.Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureAuth0(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services.Configure<Auth0Options>(configuration.GetSection("Nulobe:Auth0"));
        }

        public static IServiceCollection ConfigureQuizlet(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services.Configure<QuizletOptions>(configuration.GetSection("Nulobe:Quizlet"));
        }

        public static IServiceCollection ConfigureDocumentDb(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services.Configure<DocumentDbOptions>(configuration.GetSection("Nulobe:DocumentDb"));
        }

        public static IServiceCollection ConfigureCountries(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services.Configure<CountryOptions>(configuration.GetSection("Countries"));
        }

        public static IServiceCollection ConfigureStorage(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services.Configure<StorageOptions>(configuration.GetSection("AzureStorage"));
        }
    }
}
