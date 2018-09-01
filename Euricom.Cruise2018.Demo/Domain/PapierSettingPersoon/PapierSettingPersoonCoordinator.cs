using System;
using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Euricom.Cruise2018.Demo.Commands;

namespace Euricom.Cruise2018.Demo.Domain.PapierSettingPersoon
{
    public class PapierSettingPersoonCoordinator : AggregateCoordinator
    {
        public PapierSettingPersoonCoordinator()
        {
            Receive<IPapierSettingPersoonCommand>(c => OnReceiveCommand(c));
        }

        public override Props GetProps(string arId)
        {
            return Props.Create(() => new PapierSettingPersoonAR(arId));
        }

        private void OnReceiveCommand(IPapierSettingPersoonCommand command)
        {
            ForwardCommand(BuildArId(command), command);
        }

        private static string BuildArId(IPapierSettingPersoonCommand command)
        {
            return command.PerNummer;
        }
    }
}
