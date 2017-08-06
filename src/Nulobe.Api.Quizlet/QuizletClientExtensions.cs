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
            var formDictionary = Enumerable.Empty<KeyValuePair<string, string>>()
                .Concat(set.Terms.Select((t, i) => new KeyValuePair<string, string>($"term[{i}]", t.Name)))
                .Concat(set.Terms.Select((t, i) => new KeyValuePair<string, string>($"definition[{i}]", t.Definition)))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            var result = await client.PostAsync<QuizletSetResult>("/sets", new FormUrlEncodedContent(formDictionary));
            return new QuizletSet()
            {
                Id = result.Id,
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
