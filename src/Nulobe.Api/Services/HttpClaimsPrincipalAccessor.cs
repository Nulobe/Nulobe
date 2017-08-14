using Nulobe.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Nulobe.Api.Services
{
    public class HttpClaimsPrincipalAccessor : IClaimsPrincipalAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpClaimsPrincipalAccessor(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ClaimsPrincipal ClaimsPrincipal => _httpContextAccessor.HttpContext.User;
    }
}
