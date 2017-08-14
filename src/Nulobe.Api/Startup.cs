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

namespace Nulobe.Api
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IHostingEnvironment hostingEnvironment)
        {
            var builder = new ConfigurationBuilder().AddConfigurationSources<Startup>(hostingEnvironment);
            _configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddAutoMapper(conf =>
            {
                conf.AddCoreApiMapperConfigurations();
            });

            services.ConfigureAuth0(_configuration);
            services.ConfigureQuizlet(_configuration);
            services.ConfigureDocumentDb(_configuration);

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
            IOptions<EventServiceOptions> eventServiceOptions,
            IOptions<Auth0Options> auth0Options)
        {
            loggerFactory.AddConsole();

            using (var client = documentClientFactory.Create(documentDbOptions.Value))
            {
                client.EnsureCollectionAsync(documentDbOptions.Value, factServiceOptions.Value.FactCollectionName).Wait();
                client.EnsureCollectionAsync(documentDbOptions.Value, factServiceOptions.Value.FactAuditCollectionName).Wait();
                client.EnsureCollectionAsync(documentDbOptions.Value, eventServiceOptions.Value.EventCollectionName).Wait();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandlingMiddleware();

            app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin() // Can make this more secure
                .AllowCredentials());

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
    }
}
