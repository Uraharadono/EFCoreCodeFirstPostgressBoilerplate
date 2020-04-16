using EFCoreCodeFirstPostgressBoilerplate.UowRepo.DbContext;
using EFCoreCodeFirstPostgressBoilerplate.UowRepo.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EFCoreCodeFirstPostgressBoilerplate.UowRepo.Extensions
{
    /// <summary>
    /// Extension class to add middleware stuff
    /// </summary>
    public static class UnitOfWorkExtension
    {
        public static void AddUnitOfWork(this IServiceCollection services, IConfiguration configuration)
        {
            // Taken from: https://cmatskas.com/net-core-dependency-injection-with-constructor-parameters-2/
            services.AddTransient<IUnitOfWork>(
                s => new UnitOfWork(
                    MyDbContext.Create(configuration.GetConnectionString("LanguagePackContext"))
                )
            );
        }

        public static void AddRepository(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IRepository<>), typeof(Repository<>));
        }

        /// <summary>
        /// Extension method to add unit of work to middleware
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        public static void AddUnitOfWork<TContext>(this IServiceCollection services)
            where TContext : Microsoft.EntityFrameworkCore.DbContext
        {
            //services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
            //services.AddScoped<IUnitOfWork<TContext>, UnitOfWork<TContext>>();
        }
    }
}
