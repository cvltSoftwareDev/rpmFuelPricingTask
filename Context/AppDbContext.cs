using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using rpmFuelPricingTask.DbModels;

namespace rpmFuelPricingTask.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<RecordModel> Records { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false);
                var config = configurationBuilder.Build();
                var dbConnectionString = config.GetConnectionString("Default");
                optionsBuilder.UseSqlServer(dbConnectionString, builder =>
                {
                    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                });
            }
        }
    }
}