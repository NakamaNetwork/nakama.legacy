using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace TreasureGuide.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .CaptureStartupErrors(true)
                .UseSetting(WebHostDefaults.DetailedErrorsKey, "true")
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();

            host.Run();
        }
    }
}
