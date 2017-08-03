using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Nulobe.Clients.Web.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(directory.FullName)
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();

            host.Run();
        }
    }
}
