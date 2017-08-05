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
using Nulobe.Api.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Nulobe.Framework;

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

            services.ConfigureAuth0(_configuration);
            services.ConfigureQuizlet(_configuration);

            services.AddCoreApiServices(_configuration);
            services.AddQuizletApiServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IDocumentClientFactory documentClientFactory,
            IOptions<FactServiceOptions> factServiceOptions,
            IOptions<Auth0Options> auth0Options)
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
                });

            app.UseMvc();
        }
    }
}
