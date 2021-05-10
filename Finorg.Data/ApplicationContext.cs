using Finorg.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Finorg.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        { }

        public DbSet<Finance> Finances { get; set; }
    }
}
