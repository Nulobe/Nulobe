using Microsoft.AspNetCore.Http;
using Nulobe.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nulobe.Api.Services
{
    public class HttpRemoteIpAddressAccessor : IRemoteIpAddressAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpRemoteIpAddressAccessor(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string RemoteIpAddress => _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
    }
}
