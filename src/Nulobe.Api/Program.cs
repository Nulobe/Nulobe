using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Nulobe.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
#if DEBUG
            EnsureDocumentDbRunningAsync().Wait();
#endif

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();

            host.Run();
        }


#if DEBUG

        private const string DocumentDbEmulatorExePath = @"C:\Program Files\DocumentDB Emulator\DocumentDB.Emulator.exe";

        private static async Task EnsureDocumentDbRunningAsync()
        {
            var existingProcess = System.Diagnostics.Process.GetProcessesByName("DocumentDB.Emulator").FirstOrDefault();
            if (existingProcess == null || !(await TestConnectionAsync(existingProcess)))
            {
                Console.WriteLine("DocumentDB Emulator is not running, attempting to start");

                if (File.Exists(DocumentDbEmulatorExePath))
                {
                    var process = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
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

        private static async Task<bool> TestConnectionAsync(System.Diagnostics.Process process)
        {
            using (var client = new Microsoft.Azure.Documents.Client.DocumentClient(new Uri("https://localhost:8081"), "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="))
            {
                var id = Guid.NewGuid().ToString();
                try
                {
                    var createDbTask = client.CreateDatabaseIfNotExistsAsync(new Microsoft.Azure.Documents.Database() { Id = id });
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

#endif
}
