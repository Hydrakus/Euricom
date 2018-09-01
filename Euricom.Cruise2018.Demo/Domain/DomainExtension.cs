using Akka.Actor;
using Akka.DI.Core;
using Euricom.Cruise2018.Demo.Infrastructure.Akka;

namespace Euricom.Cruise2018.Demo.Domain
{
    public class DomainExtension : IExtension
    {
        public DomainExtension(ExtendedActorSystem system)
        {
            // Application event publisher
            var appEventPublisher = system.ActorOf(system.DI().Props<ApplicationEventPublisher>(), "applicationeventpublisher");
            system.GetAddressBook().Tell(new AddressBook.Register(ActorAddresses.ApplicationEventPublisher, appEventPublisher), ActorRefs.NoSender);

            // Aggegate Coordinator(s)
            var papierSettingPersoonCoordinator = system.ActorOf(Props.Create<PapierSettingPersoon.PapierSettingPersoonCoordinator>(), "papierSettingpersoon");
            system.GetAddressBook().Tell(new AddressBook.Register(ActorAddresses.PapierSettingPersoonCoordinator, papierSettingPersoonCoordinator), ActorRefs.NoSender);
        }
    }

    public class DomainExtensionProvider : ExtensionIdProvider<DomainExtension>
    {
        public static readonly DomainExtensionProvider Instance = new DomainExtensionProvider();

        private DomainExtensionProvider()
        {
        }

        public override DomainExtension CreateExtension(ExtendedActorSystem system)
        {
            return new DomainExtension(system);
        }
    }
}
