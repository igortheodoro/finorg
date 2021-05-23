using Finorg.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Finorg
{
    public class DesignDbContext : IDesignTimeDbContextFactory<ApplicationContext>
    {

        public ApplicationContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = configuration.GetConnectionString("DataConnection");

            var builder = new DbContextOptionsBuilder<ApplicationContext>();
            builder.UseSqlServer(connectionString);
            return new ApplicationContext(builder.Options);
        }
    }
}
