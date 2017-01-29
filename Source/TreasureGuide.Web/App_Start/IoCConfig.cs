using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using TreasureGuide.Entities;

namespace TreasureGuide.Web
{
    public static class IoCConfig
    {
        public static IContainer Container { get; private set; }

        public static void SetupIoC()
        {
            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<TreasureEntities>().InstancePerRequest();
            
            Container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(Container);
        }
    }
}