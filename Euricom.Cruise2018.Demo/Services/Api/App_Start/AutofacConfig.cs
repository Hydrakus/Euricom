using Autofac;
using Autofac.Integration.WebApi;
using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace Euricom.Cruise2018.Demo.Services.Api
{
    public class AutofacConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            //Autofac Mappings
            builder.RegisterType<Query.QueryContext>().AsSelf().InstancePerRequest().ExternallyOwned();

            var queryAssembly = Assembly.Load("Euricom.Cruise2018.Demo.Query");

            builder.RegisterAssemblyTypes(queryAssembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerRequest();

            var container = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}