using System;
using System.Reflection;
using Akka.Actor;
using Autofac;
using Euricom.Cruise2018.Demo.Services.Core.ApplicationEventConsumers;
using Euricom.Cruise2018.Demo.Services.Core.CommandConsumers;
using MassTransit;
using MassTransit.RabbitMqTransport;
using IContainer = Autofac.IContainer;

namespace Euricom.Cruise2018.Demo.Services.Core
{
    internal class BootStrapper : IDisposable
    {
        private static readonly Lazy<BootStrapper> _instance = new Lazy<BootStrapper>(() => new BootStrapper());

        internal static BootStrapper Instance { get { return _instance.Value; } }
        internal IContainer Container { get; private set; }

        private BootStrapper()
        {
            Container = new ContainerBuilder().Build();
        }

        internal void ConfigureContainer(ActorSystem actorSystem)
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(actorSystem).ExternallyOwned();

            builder.RegisterModule(new Infrastructure.Autofac.TypeBasedParameterInjectionModule<Common.Logging.ILog>(
                (t) => Common.Logging.LogManager.GetLogger(t)));

            //// Domain module
            builder.RegisterModule(new Domain.DomainModule());

            //// Projection module
            builder.RegisterModule(new Projections.ProjectionModule());

            //// Query DbContext
            builder.RegisterType<Query.QueryContext>()
                .AsSelf()
                .ExternallyOwned()
                .InstancePerDependency();

            // Infrastructure
            builder.RegisterType<Infrastructure.Bus.ApplicationEventBusPublisher>()
                .As<Infrastructure.Events.IApplicationEventPublisher>()
                .SingleInstance();

            //Bus
            builder.RegisterConsumers(Assembly.GetExecutingAssembly());

            builder.Register(context =>
            {
                var busControl = Bus.Factory.CreateUsingRabbitMq(rabbit =>
                {
                    IRabbitMqHost rabbitMqHost = rabbit.Host(new Uri("rabbitmq://localhost"), settings =>
                    {
                        settings.Password("guest");
                        settings.Username("guest");
                    });

                    rabbit.ReceiveEndpoint(rabbitMqHost, "euricom.cruise2018.demo.commands", conf =>
                    {
                        conf.Consumer<RegistreerPapierSettingPersoonConsumer>(context);
                        conf.Consumer<ZetPapierAanConsumer>(context);
                        conf.Consumer<ZetPapierUitConsumer>(context);
                    });

                    rabbit.ReceiveEndpoint(rabbitMqHost, "euricom.cruise2018.demo.applicationevents", conf =>
                    {
                        conf.Consumer<PapierSettingPersoonConsumer>(context);
                        conf.Consumer<PapierSettingPersoonPapierAangezetConsumer>(context);
                        conf.Consumer<PapierSettingPersoonPapierUitgezetConsumer>(context);
                    });
                });

                return busControl;
            })
            .SingleInstance()
            .As<IBusControl>()
            .As<IBus>();

            Container = builder.Build();
        }

        public void Dispose()
        {
            if (Container != null)
            {
                Container.Dispose();
            }
        }
    }
}
