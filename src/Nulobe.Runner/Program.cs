using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.DependencyInjection;
using Nulobe.Api.Services;

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
            services.AddApiServices(configuration);
            var serviceProvider = services.BuildServiceProvider();

            //var tagQueryService = serviceProvider.GetRequiredService<ITagQueryService>();
            //var result = tagQueryService.QueryTagsAsync(new TagQuery()
            //{
            //    Fields = "text,usagecount",
            //    OrderBy = "usagecount",
            //    SearchPattern = "x"
            //}).Result;

            var factQueryService = serviceProvider.GetRequiredService<IFactQueryService>();
            var result = factQueryService.QueryFactsAsync(new FactQuery()
            {
                Tags = "dairy,nutrition"
            }).Result;

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
