using Nulobe.Api.Quizlet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class QuizletApiServiceCollectionExtensions
    {
        public static IServiceCollection AddQuizletApiServices(
            this IServiceCollection services)
        {
            services.AddTransient<IQuizletTokenClient, QuizletTokenClient>();

            return services;
        }
    }
}
