using System.Text;
using Akka.Actor;
using Akka.Event;

namespace Euricom.Cruise2018.Demo.Infrastructure.Akka
{
    public class DeadLetterLogger : ReceiveActor
    {
        private NLog.ILogger _logger = NLog.LogManager.GetCurrentClassLogger();

        protected override void PreStart()
        {
            Context.System.EventStream.Subscribe(Self, typeof(DeadLetter));
        }

        public DeadLetterLogger()
        {
            Receive<DeadLetter>(m => LogDeadLetter(m));
        }

        private void LogDeadLetter(DeadLetter message)
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("DeadLetter encountered:");

            if (message.Sender != null)
                sb.AppendFormat("*  Sender: {0}", message.Sender.Path);
            else
                sb.Append("*  Sender: null");

            sb.AppendLine();

            if (message.Recipient != null)
                sb.AppendFormat("*  Receiver: {0}", message.Recipient.Path);
            else
                sb.Append("*  Receiver: null");

            sb.AppendLine();

            if (message.Message != null)
                sb.AppendFormat("*  Message type: {0}", message.Message.GetType().FullName);
            else
                sb.Append("*  Message: null");

            _logger.Warn(sb.ToString());
        }
    }
}
