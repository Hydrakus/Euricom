using Akka.Actor;
using Euricom.Cruise2018.Demo.Commands.PapierSettingPersoon;
using Euricom.Cruise2018.Demo.Infrastructure.Akka;
using MassTransit;
using System.Threading.Tasks;

namespace Euricom.Cruise2018.Demo.Services.Core.CommandConsumers
{
    public class ZetPapierAanConsumer : CommandConsumer, IConsumer<ZetPapierAan>
    {
        public ZetPapierAanConsumer(ActorSystem actorSystem) : base(actorSystem)
        {
        }

        public Task Consume(ConsumeContext<ZetPapierAan> context)
        {
            ExecuteCommand(ActorAddresses.PapierSettingPersoonCoordinator, context.Message);

            return Task.FromResult(0);
        }
    }
}
