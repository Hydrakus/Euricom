using System.Collections.Generic;
using System.Text;

namespace Euricom.Cruise2018.Demo.Infrastructure.Commands
{
    public enum CommandResult
    {
        Success,
        Failure
    }

    public sealed class CommandFeedback
    {
        public CommandResult Result { get; private set; }

        public string FaultCode { get; private set; }

        public string FaultDescription { get; private set; }

        private CommandFeedback(CommandResult result)
        {
            Result = result;
        }

        private CommandFeedback(string faultCode, string faultDescription)
            : this(CommandResult.Failure)
        {
            FaultCode = faultCode;
            FaultDescription = faultDescription;
        }

        public static CommandFeedback CreateSuccessFeedback()
        {
            return new CommandFeedback(CommandResult.Success);
        }

        public static CommandFeedback CreateFailureFeedback(string faultDescription)
        {
            return new CommandFeedback(CommandResult.Failure)
            {
                FaultDescription = faultDescription
            };
        }

        public static CommandFeedback CreateFailureFeedback(List<string> faultDescription)
        {
            var feedback = new CommandFeedback(CommandResult.Failure);

            var sb = new StringBuilder();
            faultDescription.ForEach(fd => sb.AppendLine("- " + fd));

            feedback.FaultDescription = sb.ToString();

            return feedback;
        }

        public static CommandFeedback CreateFailureFeedback(string faultCode, string faultDescription)
        {
            return new CommandFeedback(faultCode, faultDescription);
        }
    }
}
