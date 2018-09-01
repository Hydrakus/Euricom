using Autofac;

namespace Euricom.Cruise2018.Demo.Domain
{
    public class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationEventPublisher>();
        }
    }
}
