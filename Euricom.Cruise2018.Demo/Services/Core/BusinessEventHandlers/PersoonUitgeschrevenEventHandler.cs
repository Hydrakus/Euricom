using Akka.Actor;
using Euricom.Cruise2018.Demo.BusinessEvents;
using Euricom.Cruise2018.Demo.Commands.PapierSettingPersoon;
using Euricom.Cruise2018.Demo.Infrastructure.Akka;
using MassTransit;
using System.Threading.Tasks;

namespace Euricom.Cruise2018.Demo.Services.Core.BusinessEventHandlers
{
    public class PersoonUitgeschrevenEventHandler : BusinessEventHandler, IConsumer<PersoonUitgeschreven>
    {
        public PersoonUitgeschrevenEventHandler(ActorSystem actorSystem) : base(actorSystem)
        {
        }

        public Task Consume(ConsumeContext<PersoonUitgeschreven> context)
        {

            return Task.FromResult(0);
        }
    }
}
