using Nulobe.Api.Core;
using Nulobe.Api.Core.Facts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Nulobe.Api.Quizlet
{
    public class QuizletSetService : IQuizletSetService
    {
        private static readonly Regex SourceReferenceRegex = new Regex(@"\[(\d+)\]");

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
            if (!result.Facts.Any())
            {
                throw new Exception("No results");
            }

            using (var quizletClient = _quizletClientFactory.Create())
            {
                var title = string.Join(" ", query.Tags.Split(',').Select(t => $"#{t}")) + " by nulobe";
                return await quizletClient.CreateSetAsync(new QuizletSet()
                {
                    Title = title,
                    Terms = result.Facts.Select(f => {
                        var definition = Regex
                            .Replace(f.DefinitionMarkdown, @"\[(\d+)\]", string.Empty) // Remove indexed sources
                            .Replace("  ", " ") // Remove multi-space (i.e. left by indexed sources)
                            .Trim();

                        return new QuizletTerm()
                        {
                            Name = f.Title,
                            Definition = definition
                        };
                    })
                });
            }
        }
    }
}
