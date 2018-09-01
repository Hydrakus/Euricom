using Akka.Actor;
using Akka.DI.AutoFac;
using Autofac;
using Euricom.Cruise2018.Demo.Infrastructure.Akka;
using Topshelf;
using System;
using MassTransit;

namespace Euricom.Cruise2018.Demo.Services.Core
{
    public class CoreService : ServiceControl
    {
        private BootStrapper _bootStrapper;
        private ActorSystem _actorSystem;
        private IBusControl _serviceBus;

        public bool Start(HostControl hostControl)
        {
            // Akka
            _actorSystem = ActorSystem.Create("Euricom-Cruise2018-Demo", ConfigurationHelper.GetAkkaConfigurationSettings());

            // Autofac
            _bootStrapper = BootStrapper.Instance;
            _bootStrapper.ConfigureContainer(_actorSystem);

            var dependencyResolver = new AutoFacDependencyResolver(_bootStrapper.Container, _actorSystem);

            // Akka Infrastructure
            var deadLetterLogger = _actorSystem.ActorOf(Props.Create<DeadLetterLogger>(), "deadLetterLogger");
            var addressBook = _actorSystem.ActorOf(Props.Create(() => new AddressBook(null)), AddressBook.Name);

            Domain.DomainExtensionProvider.Instance.Apply(_actorSystem);
            Projections.ProjectionExtensionProvider.Instance.Apply(_actorSystem);

            System.Threading.Thread.Sleep(5000);

            // Service bus
            _serviceBus = _bootStrapper.Container.Resolve<IBusControl>();
            _serviceBus.Start();

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            // Service bus
            _serviceBus.Stop();

            // Akka
            _actorSystem.Terminate().Wait();

            // Autofac
            _bootStrapper.Dispose();

            return true;
        }
    }
}
