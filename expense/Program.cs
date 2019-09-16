using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OpenTracing;
using OpenTracing.Util;
using Jaeger.Samplers;
using Jaeger;
using OpenTracing.Contrib.NetCore.CoreFx;

namespace Expense
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateWebHostBuilder(args).Build().Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseUrls("http://*:5001")
            .UseStartup<Startup>()
            .ConfigureServices(services =>
            {
              services.AddOpenTracing();
              services.AddSingleton<ITracer>(serviceProvider =>
                {
                    string serviceName = serviceProvider.GetRequiredService<IHostingEnvironment>().ApplicationName;

                    // This will log to a default localhost installation of Jaeger.
                    var tracer = new Tracer.Builder(serviceName)
                        .WithSampler(new ConstSampler(true))
                        .Build();

                    GlobalTracer.Register(tracer);

                    return tracer;
                });
                services.Configure<HttpHandlerDiagnosticOptions>(options =>
                {
                    options.IgnorePatterns.Add(x => !x.RequestUri.IsLoopback);
                });
            });
  }
}
