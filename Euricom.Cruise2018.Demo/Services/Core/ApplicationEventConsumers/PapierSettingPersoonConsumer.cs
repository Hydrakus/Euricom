using Akka.Actor;
using Euricom.Cruise2018.Demo.ApplicationEvents.PapierSettingPersoon;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euricom.Cruise2018.Demo.Services.Core.ApplicationEventConsumers
{
    public class PapierSettingPersoonConsumer : ApplicationEventConsumer, IConsumer<PapierSettingPersoonGeregistreerd>
    {
        public PapierSettingPersoonConsumer(ActorSystem actorSystem) : base(actorSystem)
        {

        }

        public Task Consume(ConsumeContext<PapierSettingPersoonGeregistreerd> context)
        {
            ProjectEvent(context.Message);

            return Task.FromResult(0);
        }
    }
}
