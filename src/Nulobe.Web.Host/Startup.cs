using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using HtmlAgilityPack;
using System.IO;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Sockets;
using System.Net;
using Microsoft.Extensions.Options;
using Nulobe.Web.Common;

namespace Nulobe.Web.Host
{
    public class Startup
    {
        private readonly IConfigurationRoot _configuration;

        public Startup(IHostingEnvironment hostingEnvironment)
        {
            var builder = new ConfigurationBuilder().AddConfigurationSources<Startup>(hostingEnvironment);
            _configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureAuth0(_configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.Run(async context =>
                {
                    using (var client = new HttpClient())
                    {
                        var isPageRequest = string.IsNullOrEmpty(Path.GetExtension(context.Request.Path));

                        HttpResponseMessage proxyResponse = null;
                        try
                        {
                            proxyResponse = await client.GetAsync(new UriBuilder()
                            {
                                Scheme = "http",
                                Host = "localhost",
                                Port = 4200,
                                Path = isPageRequest ? "/" : context.Request.Path.ToString(),
                                Query = context.Request.Query.ToString()
                            }.ToString());
                        }
                        catch (Exception ex)
                        {
                            var webException = ex.InnerException as WebException;
                            if (webException != null && webException.Status == WebExceptionStatus.ConnectFailure)
                            {
                                context.Response.StatusCode = 404;
                                return;
                            }
                        }

                        if (isPageRequest)
                        {
                            // Assume we are serving up index.html
                            context.Response.ContentType = "text/html";

                            var doc = new HtmlDocument();
                            doc.Load(await proxyResponse.Content.ReadAsStreamAsync());

                            var appSettingsScript = doc.CreateElement("script");
                            appSettingsScript.AppendChild(doc.CreateTextNode(GetEnvironmentSettingsJavascript(context)));

                            var head = doc.DocumentNode.SelectSingleNode("/html/head");
                            head.AppendChild(appSettingsScript);

                            doc.Save(context.Response.Body);
                        }
                        else
                        {
                            await context.Response.WriteAsync(await proxyResponse.Content.ReadAsStringAsync());
                        }
                    }
                });
            }
            else
            {
                app.UseStaticFiles();

                app.Run(context =>
                {
                    context.Response.ContentType = "text/html";

                    using (var fs = File.OpenRead("wwwroot/index.html"))
                    {
                        var doc = new HtmlDocument();
                        doc.Load(fs);

                        var appSettingsScript = doc.CreateElement("script");
                        appSettingsScript.AppendChild(doc.CreateTextNode(GetEnvironmentSettingsJavascript(context)));

                        var head = doc.DocumentNode.SelectSingleNode("/html/head");
                        head.AppendChild(appSettingsScript);

                        doc.Save(context.Response.Body);
                    }

                    return Task.FromResult(0);
                });
            }
        }
        
        private string GetEnvironmentSettingsJavascript(HttpContext context)
        {
            var serviceProvider = context.RequestServices;
            var hostingEnvironment = serviceProvider.GetRequiredService<IHostingEnvironment>();
            var auth0Options = serviceProvider.GetRequiredService<IOptions<Auth0Options>>().Value;

            var environmentSettings =
                new Dictionary<string, object>()
                {
                    { "ENVIRONMENT", serviceProvider.GetRequiredService<IHostingEnvironment>().EnvironmentName },
                    { "API_BASE_URL", _configuration["Nulobe:ApiBaseUrl"] },
                    { "AUTH_CLIENT_ID", auth0Options.ClientId },
                    { "AUTH_DOMAIN", auth0Options.Domain }
                }
                .Select(kvp => $"{kvp.Key}: \"{kvp.Value}\"");

            return "var NULOBE_ENV = { " + string.Join(", ", environmentSettings) + " };";
        }
    }
}
