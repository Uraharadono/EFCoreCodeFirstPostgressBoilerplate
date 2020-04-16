//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Threading.Tasks;
//using Autofac;
//using Autofac.Extensions.DependencyInjection;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.VisualStudio.Web.CodeGeneration;

namespace EFCoreCodeFirstPostgressBoilerplate.UowRepo.Infrastructure
{
    //public class AutofacContainerBuilder
    //{
    //    public static IContainer Build(IConfiguration config, IWebHostEnvironment env, IServiceCollection services, AppSettings settings)
    //    {
    //        var builder = new ContainerBuilder();
    //        var connection = config.GetConnectionString("LanguagePackContext");
    //        var injectableAssemblies = GetInjectableAssemblies().ToArray();

    //        // When you do service population, it will include your controller types automatically.
    //         builder.Populate(services);

    //        builder.Register(c => settings)
    //            .AsImplementedInterfaces()
    //            .SingleInstance();

    //        builder.RegisterType<HttpContextAccessor>()
    //            .As<IHttpContextAccessor>()
    //            .SingleInstance();

    //        // Unit of work and Repositroy
    //        builder.Register(c => new UnitOfWork(LanguagePackDbContext.Create(connection)))
    //            .AsImplementedInterfaces()
    //            .InstancePerLifetimeScope();
    //        builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

    //        // Inherited infrastructure instances
    //        builder.RegisterAssemblyTypes(injectableAssemblies).Where(IsInjectable)
    //            .AsImplementedInterfaces()
    //            .PropertiesAutowired()
    //            .InstancePerLifetimeScope();

    //        return builder.Build();
    //    }

    //    private static void PrepareController(IComponentContext ctx, BaseController c)
    //    {
    //        if (ctx == null || c == null) return;

    //        c.UnitOfWork = ctx.Resolve<IUnitOfWork>();
    //    }

    //    private static IEnumerable<Assembly> GetInjectableAssemblies()
    //    {
    //        yield return Assembly.GetAssembly(typeof(AppException)); // Util
    //        // yield return Assembly.GetAssembly(typeof(UserDto));      // Services
    //    }

    //    private static bool IsInjectable(Type t)
    //    {
    //        var interfaces = t.GetInterfaces();
    //        var injectable = interfaces.Any(i =>
    //            i.IsAssignableFrom(typeof(IRepository)));
    //        return injectable;
    //    }
    //}
}
