using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Expense.Client;
using Report.Contexts;
using Report.Models;
using OpenTracing;
using Jaeger.Samplers;
using Jaeger.Reporters;
using Jaeger.Senders;
using Jaeger;
using OpenTracing.Contrib.NetCore.CoreFx;
using System;
using Microsoft.Extensions.Logging;


namespace Report
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
      services.AddTransient<IReportContext>(s => new ReportContext(new ExpenseClient(Configuration.GetConnectionString("Expenses"))));
      services.AddOpenTracing();
      services.AddSingleton<ITracer>(serviceProvider =>
      {
        string serviceName = serviceProvider.GetRequiredService<IHostingEnvironment>().ApplicationName;
        var sampler = new ConstSampler(sample: true);
        var reporter = new RemoteReporter.Builder()
                  .WithSender(new UdpSender(Configuration.GetConnectionString("JaegerURL"), Int32.Parse(Configuration.GetConnectionString("JaegerPort")), 0))
                  .Build();
        var tracer = new Tracer.Builder(serviceName)
                                      .WithSampler(sampler)
                                      .WithReporter(reporter)
                                      .Build();

        return tracer;
      });
      services.Configure<HttpHandlerDiagnosticOptions>(options => 
      { 
        options.IgnorePatterns.Add(request => request.RequestUri.ToString().Contains(Configuration.GetConnectionString("JaegerURL"))); 
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseMvc();
    }
  }
}
