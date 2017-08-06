using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Nulobe.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Api.Quizlet
{
    public class QuizletClient : IDisposable
    {
        private readonly HttpClient _client;

        public QuizletClient(
            IAccessTokenAccessor accessTokenAccessor,
            IOptions<QuizletOptions> options)
        {
            _client = new HttpClient()
            {
                BaseAddress = options.Value.ApiBaseUri
            };

            var accessToken = accessTokenAccessor.AccessToken;
            if (string.IsNullOrEmpty(accessToken))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }

        public async Task<TResult> PostAsync<TResult>(string path, HttpContent content)
        {
            var response = await _client.PostAsync(path, content);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResult>(responseString);
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
