using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Nulobe.Utility.Validation;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Nulobe.Api.Core.Sources.Impl
{
    public class DoiSourceHandler : BaseSourceHandler
    {
        public DoiSourceHandler() : base(SourceType.Doi)
        {
        }

        public override void PreValidate(JObject source, ModelErrorDictionary errors)
        {
            string doi = null;
            var doiToken = source.SelectToken(SourceFields.Doi);
            if (doiToken == null)
            {
                errors.AddRequired(SourceFields.Doi);
            }
            else if (doiToken is JValue doiValue)
            {
                if (doiToken.Type == JTokenType.String)
                {
                    doi = doiToken.ToObject<string>();
                }
                else
                {
                    errors.AddStringExpected(SourceFields.Doi);
                }
            }
            else
            {
                errors.AddStringExpected(SourceFields.Doi);
            }

            // TODO: Validate DOI format
        }

        public override async Task ProcessAsync(JObject source)
        {
            var doi = source.SelectToken(SourceFields.Doi).ToObject<string>();

            string citation = null;
            using (var client = new HttpClient())
            {
                var requestUri = "https://doi.org/" + doi;
                var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/x-bibliography"));

                try
                {
                    var response = await client.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        citation = await response.Content.ReadAsStringAsync();
                        citation = citation.Trim();
                    }
                }
                catch (Exception)
                {
                }
            }

            if (citation != null)
            {
                source.Add(SourceFields.CitationFromDoi, new JValue(citation));
            }
        }

        public override void PostValidate(JObject source, ModelErrorDictionary errors)
        {
            if (source.SelectToken(SourceFields.CitationFromDoi) == null)
            {
                errors.Add($"Could not find document with DOI {source.SelectToken(SourceFields.Doi).ToObject<string>()}", SourceFields.Doi);
            }
        }
    }
}
