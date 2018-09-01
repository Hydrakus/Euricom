using Akka.Actor;
using Euricom.Cruise2018.Demo.Commands.PapierSettingPersoon;
using Euricom.Cruise2018.Demo.Infrastructure.Akka;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euricom.Cruise2018.Demo.Services.Core.CommandConsumers
{
    public class RegistreerPapierSettingPersoonConsumer : CommandConsumer, IConsumer<RegistreerPapierSettingPersoon>
    {
        public RegistreerPapierSettingPersoonConsumer(ActorSystem actorSystem) : base(actorSystem)
        {
        }

        public Task Consume(ConsumeContext<RegistreerPapierSettingPersoon> context)
        {
            ExecuteCommand(ActorAddresses.PapierSettingPersoonCoordinator, context.Message);

            return Task.FromResult(0);
        }
    }
}
