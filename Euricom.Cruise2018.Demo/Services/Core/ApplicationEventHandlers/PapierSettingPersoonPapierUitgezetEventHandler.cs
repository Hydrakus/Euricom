using Akka.Actor;
using Euricom.Cruise2018.Demo.ApplicationEvents.PapierSettingPersoon;
using MassTransit;
using System.Threading.Tasks;

namespace Euricom.Cruise2018.Demo.Services.Core.ApplicationEventHandlers
{
    public class PapierSettingPersoonPapierUitgezetHandler : ApplicationEventHandler, IConsumer<PapierSettingPersoonPapierUitgezet>
    {
        public PapierSettingPersoonPapierUitgezetHandler(ActorSystem actorSystem) : base(actorSystem)
        {

        }

        public Task Consume(ConsumeContext<PapierSettingPersoonPapierUitgezet> context)
        {
            ProjectEvent(context.Message);

            return Task.FromResult(0);
        }
    }
}
