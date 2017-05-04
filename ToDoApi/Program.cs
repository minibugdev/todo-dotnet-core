using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace ToDoApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseEnvironment("Development")
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}