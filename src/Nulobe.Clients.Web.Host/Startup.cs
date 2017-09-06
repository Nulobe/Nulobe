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
using Nulobe.Framework;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;

namespace Nulobe.Clients.Web.Host
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        public Startup(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;

            var builder = new ConfigurationBuilder()
                .AddConfigurationSources<Startup>(hostingEnvironment)
                .AddCountriesJsonFile();
            _configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureAuth0(_configuration);
            services.ConfigureQuizlet(_configuration);
            services.ConfigureCountries(_configuration);
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

                app.Use(async (context, next) =>
                {
                    var isPageRequest = string.IsNullOrEmpty(Path.GetExtension(context.Request.Path));

                    var scriptRequestExtensions = new string[] { ".js", ".js.map" };
                    var isScriptRequest = scriptRequestExtensions.Any(e => context.Request.Path.Value.EndsWith(e, StringComparison.InvariantCultureIgnoreCase));

                    var isRequestToAngularServer = isPageRequest || isScriptRequest;

                    if (isRequestToAngularServer)
                    {
                        // Assume we are serving up index.html
                        HttpResponseMessage proxyResponse = null;
                        using (var client = new HttpClient())
                        {
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
                                if (ex.InnerException is WebException webException && webException.Status == WebExceptionStatus.ConnectFailure)
                                {
                                    context.Response.StatusCode = 404;
                                    return;
                                }
                            }
                        }

                        if (isPageRequest)
                        {
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
                            foreach (var header in proxyResponse.Headers)
                            {
                                context.Response.Headers.AppendList(header.Key, header.Value.ToList());
                            }

                            context.Response.ContentType = proxyResponse.Content.Headers.ContentType.MediaType;

                            await context.Response.WriteAsync(await proxyResponse.Content.ReadAsStringAsync());
                        }
                        
                    }
                    else
                    {
                        await next();
                    }
                });

                var currentDir = new DirectoryInfo(env.ContentRootPath);
                var angularWebRootDir = Path.Combine(currentDir.Parent.FullName, "Nulobe.Clients.Web", "src");
                app.UseStaticFiles(new StaticFileOptions()
                {
                    FileProvider = new PhysicalFileProvider(angularWebRootDir)
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
            var quizletOptions = serviceProvider.GetRequiredService<IOptions<QuizletOptions>>().Value;
            var countryOptions = serviceProvider.GetRequiredService<IOptions<CountryOptions>>().Value;

            var environmentSettings = new
            {
                baseUrl = _configuration["Nulobe:WebBaseUrl"],
                apiBaseUrl = _configuration["Nulobe:ApiBaseUrl"],
                auth = new
                {
                    auth0 = GetPublicAuthOptions(auth0Options),
                    quizlet = GetPublicAuthOptions(quizletOptions)
                }
            };

            var environmentName = hostingEnvironment.EnvironmentName;

            var jsVariables = new Dictionary<string, string>()
            {
                { "NULOBE_ENV", $"\"{hostingEnvironment.EnvironmentName}\"" },
                { "NULOBE_ENV_SETTINGS", JsonConvert.SerializeObject(environmentSettings) }
            };

            return string.Join(Environment.NewLine, jsVariables.Select(kvp => $"var {kvp.Key} = {kvp.Value};"));
        }

        private object GetPublicAuthOptions(IAuthOptions options)
        {
            return new
            {
                clientId = options.ClientId,
                domain = options.Domain
            };
        }
    }
}
