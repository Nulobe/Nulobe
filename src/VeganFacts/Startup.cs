﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VeganFacts.DocumentDb.Client;
using VeganFacts.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace VeganFacts
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IHostingEnvironment hostingEnvironment)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", optional: true);

            _configuration = builder.Build();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddTransient<IDocumentClientFactory, DocumentClientFactory>();

            services.AddTransient<IFactService, FactService>();
            services.AddTransient<IFactQueryService, FactService>();

            services.Configure<FactServiceOptions>(_configuration.GetSection("FactService"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IDocumentClientFactory documentClientFactory,
            IOptions<FactServiceOptions> factServiceOptions)
        {
            loggerFactory.AddConsole();

            using (var client = documentClientFactory.Create(factServiceOptions.Value))
            {
                client.EnsureCollectionAsync(factServiceOptions.Value).Wait();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
