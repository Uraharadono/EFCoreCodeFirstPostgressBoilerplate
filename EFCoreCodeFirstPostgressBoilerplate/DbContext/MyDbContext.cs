using EFCoreCodeFirstPostgressBoilerplate.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCoreCodeFirstPostgressBoilerplate.DbContext
{
    public class MyDbContext: Microsoft.EntityFrameworkCore.DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Since we have multiple schemas in project, we need to specify which one we are using
            // https://docs.microsoft.com/en-us/ef/core/modeling/entity-types?tabs=data-annotations
            modelBuilder.HasDefaultSchema("public");


            // We need this cause of since pgsql 10 it uses "Generates" for primary keys.
            // If you don't have pgsql 10, then we have to use "serial" for primary key serial.
            // https://stackoverflow.com/questions/51852587/create-an-automatically-increasing-primary-key-column-in-postgresql
            // https://www.npgsql.org/efcore/modeling/generated-properties.html
            modelBuilder.UseSerialColumns();

            // Since there is not data annotation for default values in Entity Framework Core code first default values for PostgreSQL.
            // This is just example with JSONB type of data so you don't have to search for solution.
            // https://stackoverflow.com/questions/53298033/entity-framework-core-code-first-default-values-for-postgresql
            //modelBuilder.Entity<WeatherForecast>(entity =>
            //{
            //    entity.Property(p => p.Content)
            //        // .HasColumnType("jsonb")
            //        .HasDefaultValueSql("'{}'::jsonb")
            //        .ValueGeneratedOnAdd();
            //});
        }

        // We can put configuration here as well instead of Startup.cs
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //    => optionsBuilder.UseNpgsql("Host=my_host;Database=my_db;Username=my_user;Password=my_pw");

        public DbSet<Place> Places { get; set; }
        public DbSet<WeatherForecast> WeatherForecasts { get; set; }
    }
}
