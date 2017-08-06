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
        private readonly string _baseUri;
        private readonly HttpClient _client;

        public QuizletClient(
            IAccessTokenAccessor accessTokenAccessor,
            IOptions<QuizletOptions> options)
        {
            _baseUri = options.Value.ApiBaseUri.ToString();
            _client = new HttpClient();

            var accessToken = accessTokenAccessor.AccessToken;
            if (!string.IsNullOrEmpty(accessToken))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }

        public async Task<TResult> PostAsync<TResult>(string path, HttpContent content)
        {
            var response = await _client.PostAsync(_baseUri + path, content);
            var json = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<TResult>(json);
            }
            else
            {
                var error = JsonConvert.DeserializeObject<QuizletError>(json);
                throw new QuizletException(error);
            }
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
