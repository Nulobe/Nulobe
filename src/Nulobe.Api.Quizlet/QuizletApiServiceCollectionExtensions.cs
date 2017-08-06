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
            services.AddTransient<IQuizletClientFactory, QuizletClientFactory>();
            services.AddTransient<IQuizletTokenService, QuizletTokenService>();
            services.AddTransient<IQuizletSetService, QuizletSetService>();

            return services;
        }
    }
}
