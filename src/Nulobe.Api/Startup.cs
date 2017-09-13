using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nulobe.DocumentDb.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Nulobe.Framework;
using Nulobe.Api.Services;
using Nulobe.Api.Core.Facts;
using Nulobe.Api.Core.Events;
using AutoMapper;
using Nulobe.Api.Middleware;
using System.Diagnostics;
using Microsoft.Azure.Documents.Client;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Nulobe.Microsoft.WindowsAzure.Storage;

namespace Nulobe.Api
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

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.Configure<MvcOptions>(options =>
            {
                if (!_hostingEnvironment.IsDevelopment())
                {
                    options.Filters.Add(new RequireHttpsAttribute());
                }
            });

            services.AddAutoMapper(conf =>
            {
                conf.AddCoreApiMapperConfigurations();
            });

            services.ConfigureAuth0(_configuration);
            services.ConfigureQuizlet(_configuration);
            services.ConfigureDocumentDb(_configuration);
            services.ConfigureStorage(_configuration);
            services.ConfigureCountries(_configuration);

            services.AddCoreApiServices(_configuration);
            services.AddQuizletApiServices();

            services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IClaimsPrincipalAccessor, HttpClaimsPrincipalAccessor>();
            services.AddScoped<IRemoteIpAddressAccessor, HttpRemoteIpAddressAccessor>();
            services.AddScoped<IAccessTokenAccessor, HttpBearerAccessTokenAccessor>();
            services.AddScoped<Auditor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IDocumentClientFactory documentClientFactory,
            IOptions<DocumentDbOptions> documentDbOptions,
            IOptions<FactServiceOptions> factServiceOptions,
            IOptions<Auth0Options> auth0Options)
        {
            loggerFactory.AddConsole();

            using (var client = documentClientFactory.Create(documentDbOptions.Value))
            {
                client.EnsureCollectionAsync(documentDbOptions.Value, Core.Facts.Constants.FactCollectionName).Wait();
            }

            if (env.IsDevelopment())
            {
                EnsureDocumentDbRunningAsync().Wait();
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandlingMiddleware();

            app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin() // Can make this more secure
                .AllowCredentials()
                .WithExposedHeaders("X-Continuation-Token"));

            app.MapWhen(
                context =>
                {
                    var skipAuthentication = context.Request.Path.StartsWithSegments(new PathString("/quizlet"), StringComparison.InvariantCultureIgnoreCase);
                    return !skipAuthentication;
                },
                innerApp =>
                {
                    innerApp.UseJwtBearerAuthentication(new JwtBearerOptions()
                    {
                        Audience = auth0Options.Value.ClientId,
                        Authority = auth0Options.Value.GetAuthority()
                    });

                    innerApp.UseMvc();
                });

            app.UseMvc();
        }

        private const string DocumentDbEmulatorExePath = @"C:\Program Files\Azure Cosmos DB Emulator\CosmosDB.Emulator.exe";

        private static async Task EnsureDocumentDbRunningAsync()
        {
            var existingProcess = Process.GetProcessesByName("CosmosDB.Emulator").FirstOrDefault();
            if (existingProcess == null || !(await TestConnectionAsync(existingProcess)))
            {
                Console.WriteLine("DocumentDB Emulator is not running, attempting to start");

                if (File.Exists(DocumentDbEmulatorExePath))
                {
                    var process = Process.Start(new ProcessStartInfo()
                    {
                        FileName = DocumentDbEmulatorExePath,
                        CreateNoWindow = true
                    });
                }
                else
                {
                    throw new Exception($"DocumentDB emulator is not installed at path {DocumentDbEmulatorExePath}");
                }
            }
            else
            {
                Console.WriteLine("DocumentDB Emulator is already running");
            }
        }

        private static async Task<bool> TestConnectionAsync(Process process)
        {
            using (var client = new DocumentClient(new Uri("https://localhost:8081"), "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="))
            {
                var id = Guid.NewGuid().ToString();
                try
                {
                    var createDbTask = client.CreateDatabaseIfNotExistsAsync(new Database() { Id = id });
                    if (await Task.WhenAny(createDbTask, Task.Delay(TimeSpan.FromSeconds(10))) != createDbTask)
                    {
                        throw new TimeoutException("DocumentDB test timed out after 10 seconds");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to create DocumentDB database, killing process");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    process.Kill();
                    process.WaitForExit();
                    return false;
                }
                await client.DeleteDatabaseAsync($"dbs/{id}");
                return true;
            }
        }
    }
}
