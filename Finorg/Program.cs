using Finorg.Data;
using Finorg.Data.Repositories;
using Finorg.Data.Repositories.Interfaces;
using Finorg.Services;
using Finorg.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace Finorg
{
    public class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            var connectionString = "";

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, s) =>
                {
                    connectionString = context.Configuration["ConnectionStrings:DataConnection"];

                    s.AddTransient<IIntegrationService, IntegrationService>();
                    s.AddScoped<IFinanceRepository, FinanceRepository>();
                    s.AddScoped<IFinanceService, FinanceService>();
                    s.AddScoped<IRequestService, RequestService>();
                    s.AddScoped<IDocumentService, DocumentService>();
                    s.AddDbContext<ApplicationContext>(options =>
                        options.UseSqlServer(connectionString),
                            ServiceLifetime.Transient);
                })
                .Build();

            var dbContext = new DesignDbContext();
            dbContext.CreateDbContext(args);

            var integration = ActivatorUtilities.CreateInstance<IntegrationService>(host.Services);
            integration.Start();
        }

        static void BuildConfig(IConfigurationBuilder builder)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                .AddEnvironmentVariables();
        }
    }
}
