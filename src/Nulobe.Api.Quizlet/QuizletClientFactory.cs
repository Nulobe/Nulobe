using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Quizlet
{
    public class QuizletClientFactory : IQuizletClientFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public QuizletClientFactory(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public QuizletClient Create() => ActivatorUtilities.CreateInstance<QuizletClient>(_serviceProvider);
    }
}
