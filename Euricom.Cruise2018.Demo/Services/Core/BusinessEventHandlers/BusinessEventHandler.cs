using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;
using Euricom.Cruise2018.Demo.Infrastructure.Akka;
using Euricom.Cruise2018.Demo.Infrastructure.Commands;

namespace Euricom.Cruise2018.Demo.Services.Core.BusinessEventHandlers
{
    public abstract class BusinessEventHandler
    {
        private readonly ActorSystem _actorSystem;

        protected BusinessEventHandler(ActorSystem actorSystem)
        {
            _actorSystem = actorSystem;
        }

        protected void ExecuteCommand(string arCoordinatorAddress, ICommand command)
        {
            var cmdValResult = command.Validate();

            if (!cmdValResult.Valid)
            {
                HandleFailure(command, cmdValResult.ValidationErrors);
            }

            var arCoordinator = _actorSystem.GetActorFromAddressBook(arCoordinatorAddress);
            var cmdFeedback = arCoordinator.Ask<CommandFeedback>(command, TimeSpan.FromSeconds(10)).Result;

            if (cmdFeedback.Result == CommandResult.Failure)
            {
                HandleFailure(command, new[] { cmdFeedback.FaultDescription });
            }
        }

        private static void HandleFailure(ICommand command, IEnumerable<string> errorMessages)
        {
            var errMsg = new StringBuilder();

            errMsg.AppendFormat("Failed to execute {0} command!", command.GetType());
            errMsg.AppendLine();

            foreach (var message in errorMessages)
            {
                errMsg.AppendLine("Fault description:");
                errMsg.AppendFormat("- {0}{1}", message, Environment.NewLine);
            }

            throw new Exception(errMsg.ToString());
        }
    }
}
