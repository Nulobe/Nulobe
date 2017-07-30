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

namespace VeganFacts.Web.Host
{
    public class Startup
    {
        private readonly IConfigurationRoot _configuration;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile($"appsettings.local.json", optional: true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
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

                        var proxyResponse = await client.GetAsync(new UriBuilder()
                        {
                            Scheme = "http",
                            Host = "localhost",
                            Port = 4200,
                            Path = isPageRequest ? "/" : context.Request.Path.ToString(),
                            Query = context.Request.Query.ToString()
                        }.ToString());

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
            var environmentSettings =
                new Dictionary<string, object>()
                {
                    { "Name", context.RequestServices.GetRequiredService<IHostingEnvironment>().EnvironmentName },
                    { "ApiBaseUrl", _configuration["VeedealsApi:BaseUrl"] }
                }
                .Select(kvp => $"{kvp.Key}: \"{kvp.Value}\"");

            return "var VEGANFACTS_ENV = { " + string.Join(", ", environmentSettings) + " };";
        }
    }
}
