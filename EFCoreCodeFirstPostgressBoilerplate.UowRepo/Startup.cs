using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCoreCodeFirstPostgressBoilerplate.UowRepo.DbContext;
using EFCoreCodeFirstPostgressBoilerplate.UowRepo.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EFCoreCodeFirstPostgressBoilerplate.UowRepo
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostingEnvironment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            HostingEnvironment = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // The validation system in .NET Core 3.0 and later treats non-nullable parameters or bound properties as if they had a [Required] attribute.
            // Value types such as decimal and int are non-nullable. To turn it off:
            // AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
            services.AddControllers();// we need only controllers for our api now

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });

            // Database context
            services.AddDbContext<MyDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("LanguagePackContext"));
            });

            // Unit of work and Repository
            services.AddUnitOfWork(Configuration);
            services.AddRepository();

            // Resolve BaseController as it isn't initialized as of now 
            // TODO: FIGURE THIS
            // Get all controllers in Startup assembly that implement "BaseController" but check that they are no abstract as it will pull BaseController as well
            //var controllersTypesInAssembly = typeof(Startup).Assembly.GetExportedTypes()
            //    .Where(type => typeof(BaseController).IsAssignableFrom(type) && !type.GetTypeInfo().IsAbstract).ToArray();
            //foreach (var implementationType in controllersTypesInAssembly)
            //{
            //    foreach (var interfaceType in implementationType.GetInterfaces())
            //    {
            //        services.AddSingleton(interfaceType, implementationType);
            //    }
            //}
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
