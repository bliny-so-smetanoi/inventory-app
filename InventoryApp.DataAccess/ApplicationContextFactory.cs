using InventoryApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace InventoryApp.DataAccess
{
    public class ApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
    {
        public ApplicationContext CreateDbContext(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.core.json")
                .Build();
            Console.WriteLine("factory");
            var connectionString = config.GetConnectionString("SqlConnection");
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            optionsBuilder.UseNpgsql().UseSnakeCaseNamingConvention();

            return new ApplicationContext(optionsBuilder.Options);
        }

    }
}
