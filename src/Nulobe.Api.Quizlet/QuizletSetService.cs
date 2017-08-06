using Nulobe.Api.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Quizlet
{
    public class QuizletSetService : IQuizletSetService
    {
        private readonly IQuizletClientFactory _quizletClientFactory;
        private readonly IFactQueryService _factQueryService;

        public QuizletSetService(
            IQuizletClientFactory quizletClientFactory,
            IFactQueryService factQueryService)
        {
            _quizletClientFactory = quizletClientFactory;
            _factQueryService = factQueryService;
        }

        public async Task<QuizletSet> CreateSetAsync(FactQuery query)
        {
            var result = await _factQueryService.QueryFactsAsync(query);
            if (!result.Any())
            {
                throw new Exception("No results");
            }

            using (var quizletClient = _quizletClientFactory.Create())
            {
                return await quizletClient.CreateSetAsync(new QuizletSet()
                {
                    Terms = result.Select(f => new QuizletTerm()
                    {
                        Name = f.Title,
                        Definition = f.Definition
                    })
                });
            }
        }
    }
}
