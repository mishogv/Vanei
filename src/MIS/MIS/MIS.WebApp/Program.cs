namespace MIS.WebApp
{
    using System.IO;

    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .ConfigureAppConfiguration((hosting, config) =>
                       {
                           config.SetBasePath(Directory.GetCurrentDirectory());
                           config.AddJsonFile("ApiConfigurations.json", true, true);
                       })
                .UseStartup<Startup>();
    }
}
