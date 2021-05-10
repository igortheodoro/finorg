using Finorg.Data;
using Finorg.Data.Repositories;
using Finorg.Data.Repositories.Interfaces;
using Finorg.Services;
using Finorg.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace Finorg
{
    public class Program
    {
        private readonly IIntegrationService _integrationService;
        private static IConfiguration _configuration;

        public Program(IIntegrationService integrationService)
        {
            _integrationService = integrationService;
        }

        static void Main(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();

            CreateHostBuilder(args).Build().Services.GetRequiredService<Program>();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices(s =>
                   {
                       s.AddTransient<Program>();
                       
                       s.AddTransient<IIntegrationService, IntegrationService>();
                       s.AddScoped<IFinanceRepository, FinanceRepository>();
                       s.AddScoped<IFinanceService, FinanceService>();
                       s.AddScoped<IRequestService, RequestService>();
                       s.AddScoped<IDocumentService, DocumentService>();

                       s.AddDbContext<ApplicationContext>(options =>
                        options.UseSqlServer(_configuration["ConnectionStrings:DataConnection"]),
                            ServiceLifetime.Transient);
                   }
                );
        }
    }
}
