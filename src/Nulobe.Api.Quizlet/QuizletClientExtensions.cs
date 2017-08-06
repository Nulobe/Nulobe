using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Quizlet
{
    public static class QuizletClientExtensions
    {
        public static async Task<QuizletSet> CreateSetAsync(
            this QuizletClient client,
            QuizletSet set)
        {
            var formData = Enumerable.Empty<KeyValuePair<string, string>>()
                .Concat(new KeyValuePair<string, string>[]
                {
                    new KeyValuePair<string, string>("title", set.Title),
                    new KeyValuePair<string, string>("lang_terms", "en"),
                    new KeyValuePair<string, string>("lang_definitions", "en")
                })
                .Concat(set.Terms.Select(t => new KeyValuePair<string, string>($"terms[]", t.Name)))
                .Concat(set.Terms.Select(t => new KeyValuePair<string, string>($"definitions[]", t.Definition)));

            var result = await client.PostAsync<QuizletSetResult>("/sets", new FormUrlEncodedContent(formData));
            return new QuizletSet()
            {
                Id = result.Id,
                Title = set.Title,
                Url = result.Url,
                Terms = set.Terms
            };
        }

        private class QuizletSetResult
        {
            [JsonProperty("set_id")]
            public int Id { get; set; }

            public Uri Url { get; set; }
        }
    }
}
