using Akka.Actor;
using Euricom.Cruise2018.Demo.BusinessEvents;
using Euricom.Cruise2018.Demo.Commands.PapierSettingPersoon;
using Euricom.Cruise2018.Demo.Infrastructure.Akka;
using MassTransit;
using System.Threading.Tasks;

namespace Euricom.Cruise2018.Demo.Services.Core.BusinessEventHandlers
{
    public class PapierSettingGekozenEventHandler : BusinessEventHandler, IConsumer<PapiersettingGekozen>
    {
        public PapierSettingGekozenEventHandler(ActorSystem actorSystem) : base(actorSystem)
        {
        }

        public Task Consume(ConsumeContext<PapiersettingGekozen> context)
        {
            if(context.Message.PapierAan)
            {
                ExecuteCommand(ActorAddresses.PapierSettingPersoonCoordinator, 
                    new ZetPapierAan(context.Message.PerNummer));
            }
            else
            {
                ExecuteCommand(ActorAddresses.PapierSettingPersoonCoordinator,
                   new ZetPapierUit(context.Message.PerNummer));
            }

            return Task.FromResult(0);
        }
    }
}
