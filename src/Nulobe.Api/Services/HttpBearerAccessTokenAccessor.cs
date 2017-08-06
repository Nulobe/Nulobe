using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Nulobe.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nulobe.Api.Services
{
    public class HttpBearerAccessTokenAccessor : IAccessTokenAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpBearerAccessTokenAccessor(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string AccessToken {
            get
            {
                var httpContext = _httpContextAccessor.HttpContext;
                var headers = httpContext.Request.Headers;

                StringValues authorizationHeader;
                if (headers.TryGetValue("Authorization", out authorizationHeader))
                {
                    var value = authorizationHeader.FirstOrDefault();
                    if (!string.IsNullOrEmpty(value))
                    {
                        var split = value.Split(' ').Select(s => s.Trim());
                        if (split.Count() == 2 && split.First().Equals("Bearer"))
                        {
                            return split.Skip(1).First();
                        }
                    }
                    return authorizationHeader.Skip(1).Take(1).ToString();
                }

                return null;
            }
        }
    }
}
