using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Nulobe.Utility.Validation;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Nulobe.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ClientException ex)
            {
                var result = GetClientExceptionResult(context, ex);
                
                context.Response.StatusCode = result.StatusCode;

                var json = JsonConvert.SerializeObject(
                    new
                    {
                        Message = result.Message,
                        Data = result.Data ?? new { }
                    },
                    new JsonSerializerSettings()
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });

                await context.Response.WriteAsync(json);
            }
        }

        private ClientExceptionResult GetClientExceptionResult(HttpContext context, ClientException ex)
        {
            ClientExceptionResult result = null;
            if (TryHandleInvalidModelException(ex, out result)) { }
            else
            {
                result = new ClientExceptionResult()
                {
                    Message = "Bad request",
                    StatusCode = 400
                };
            }
            return result;
        }

        private bool TryHandleInvalidModelException(ClientException ex, out ClientExceptionResult result)
        {
            var invalidModelEx = ex as ClientModelValidationException;
            if (invalidModelEx == null)
            {
                result = null;
                return false;
            }
            else
            {
                result = new ClientExceptionResult()
                {
                    Message = "Validation failed on the request",
                    StatusCode = 400,
                    Data = invalidModelEx.ModelErrors
                };
                return true;
            }
        }

        private class ClientExceptionResult
        {
            public string Message { get; set; }

            public int StatusCode { get; set; }

            public object Data { get; set; }
        }
    }

    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
