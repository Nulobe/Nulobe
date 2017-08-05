using Microsoft.Extensions.Configuration;
using Nulobe.Framework;
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
            return services.Configure<Auth0Options>(configuration.GetSection("Nulobe:Auth:Auth0"));
        }

        public static IServiceCollection ConfigureQuizlet(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services.Configure<QuizletOptions>(configuration.GetSection("Nulobe:Auth:Quizlet"));
        }
    }
}
