using Akka.Actor;
using Euricom.Cruise2018.Demo.ApplicationEvents.PapierSettingPersoon;
using MassTransit;
using System.Threading.Tasks;

namespace Euricom.Cruise2018.Demo.Services.Core.ApplicationEventConsumers
{
    public class PapierSettingPersoonPapierUitgezetConsumer : ApplicationEventConsumer, IConsumer<PapierSettingPersoonPapierUitgezet>
    {
        public PapierSettingPersoonPapierUitgezetConsumer(ActorSystem actorSystem) : base(actorSystem)
        {

        }

        public Task Consume(ConsumeContext<PapierSettingPersoonPapierUitgezet> context)
        {
            ProjectEvent(context.Message);

            return Task.FromResult(0);
        }
    }
}
