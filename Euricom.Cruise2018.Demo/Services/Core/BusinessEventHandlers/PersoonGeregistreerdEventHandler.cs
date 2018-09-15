using Akka.Actor;
using Euricom.Cruise2018.Demo.BusinessEvents;
using Euricom.Cruise2018.Demo.Commands.PapierSettingPersoon;
using Euricom.Cruise2018.Demo.Infrastructure.Akka;
using MassTransit;
using System.Threading.Tasks;

namespace Euricom.Cruise2018.Demo.Services.Core.BusinessEventHandlers
{
    public class PersoonGeregistreerdEventHandler : BusinessEventHandler, IConsumer<PersoonGeregistreerd>
    {
        public PersoonGeregistreerdEventHandler(ActorSystem actorSystem) : base(actorSystem)
        {
        }

        public Task Consume(ConsumeContext<PersoonGeregistreerd> context)
        {
            ExecuteCommand(ActorAddresses.PapierSettingPersoonCoordinator, CreateCommand(context.Message));

            return Task.FromResult(0);
        }

        private RegistreerPapierSettingPersoon CreateCommand(PersoonGeregistreerd be)
        {
            return new RegistreerPapierSettingPersoon(be.PerNummer, be.Naam, be.Voornaam, be.Straat, be.Nummer,
                be.Bus, be.Postcode, be.Gemeente);
        }
    }
}
