using Akka.Actor;
using Euricom.Cruise2018.Demo.Commands.PapierSettingPersoon;
using Euricom.Cruise2018.Demo.Infrastructure.Akka;
using MassTransit;
using System.Threading.Tasks;

namespace Euricom.Cruise2018.Demo.Services.Core.CommandConsumers
{
    public class ZetPapierUitConsumer : CommandConsumer, IConsumer<ZetPapierUit>
    {
        public ZetPapierUitConsumer(ActorSystem actorSystem) : base(actorSystem)
        {
        }

        public Task Consume(ConsumeContext<ZetPapierUit> context)
        {
            ExecuteCommand(ActorAddresses.PapierSettingPersoonCoordinator, context.Message);

            return Task.FromResult(0);
        }
    }
}
