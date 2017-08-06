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
            services.AddCoreApiServices(configuration);
            services.AddQuizletApiServices();
            services.ConfigureQuizlet(configuration);
            services.AddTransient<IAccessTokenAccessor, StubbedAccessTokenAccessor>();
            var serviceProvider = services.BuildServiceProvider();

            var quizletSetService = serviceProvider.GetRequiredService<IQuizletSetService>();
            var result = quizletSetService.CreateSetAsync(new FactQuery()
            {
                Tags = "dairy,nutrition"
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
    }
}
