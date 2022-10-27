using alpinetest;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace alpinetest.Db
{

    public class WeatherDbContext : DbContext
    {

        //Constructor with DbContextOptions and the context class itself
        public WeatherDbContext(DbContextOptions<WeatherDbContext> options) : base(options)
        { }
        public DbSet<WeatherForecast>? WeatherForecast { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("Database");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}