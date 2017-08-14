using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.DependencyInjection;
using Nulobe.Api.Core;
using Nulobe.Framework;
using Nulobe.Api.Quizlet;
using Nulobe.Api.Core.Facts;
using AutoMapper;
using Nulobe.Api.Core.Events;
using System.Reflection;
using System.Security.Claims;

namespace Nulobe.Runner
{
    public class Program
    {
        static void Main(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder().AddConfigurationSources<Program>(new MockHostingEnvironment());
            var configuration = configurationBuilder.Build();

            var services = new ServiceCollection();
            services.AddOptions();
            services.AddAutoMapper(conf =>
            {
                conf.AddCoreApiMapperConfigurations();
            }, Assembly.GetEntryAssembly());
            services.AddCoreApiServices(configuration);
            services.AddQuizletApiServices();
            services.ConfigureQuizlet(configuration);
            services.AddTransient<IRemoteIpAddressAccessor, StubbedRemoteIpAddressAccessor>();
            services.AddTransient<IAccessTokenAccessor, StubbedAccessTokenAccessor>();
            services.AddTransient<IClaimsPrincipalAccessor, StubbedClaimsPrincipalAccessor>();
            var serviceProvider = services.BuildServiceProvider();

            //var quizletSetService = serviceProvider.GetRequiredService<IQuizletSetService>();
            //var result = quizletSetService.CreateSetAsync(new FactQuery()
            //{
            //    Tags = "dairy,nutrition"
            //}).Result;


            var factService = serviceProvider.GetRequiredService<IFactService>();
            var fact = factService.CreateFactAsync(new FactCreate()
            {
                Title = "Test fact",
                Definition = " [2] Test [1] definition",
                Tags = new string[] { "dairy", "environment", "NZ" }
            }).Result;

            var flagService = serviceProvider.GetRequiredService<IFlagEventService>();
            var flag = flagService.CreateEventAsync(new FlagCreate()
            {
                Description = "Yo this shit is straight up wrong",
                FactId = fact.Id
            }).Result;
        }

        private class StubbedAccessTokenAccessor : IAccessTokenAccessor
        {
            public string AccessToken => "J4W8PQjEy9US7ejpTaX4V43dKcdp3sG99EnpFrb6";
        }

        private class MockHostingEnvironment : IHostingEnvironment
        {
            public string EnvironmentName { get => "Development"; set => throw new NotImplementedException(); }
            public string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public string WebRootPath { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public IFileProvider WebRootFileProvider { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public string ContentRootPath { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public IFileProvider ContentRootFileProvider { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        }

        private class StubbedRemoteIpAddressAccessor : IRemoteIpAddressAccessor
        {
            public string RemoteIpAddress => "1.0.0.127";
        }

        private class StubbedClaimsPrincipalAccessor : IClaimsPrincipalAccessor
        {
            public ClaimsPrincipal ClaimsPrincipal
            {
                get
                {
                    var identity = new ClaimsIdentity("test");

                    var claim = new Claim("sub", "test-user");
                    claim.Properties.Add("x", "sub");

                    identity.AddClaim(claim);

                    return new ClaimsPrincipal(identity);
                }
            }
        }
    }
}
