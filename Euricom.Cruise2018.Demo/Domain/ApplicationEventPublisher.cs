using System;
using Akka.Actor;
using Euricom.Cruise2018.Demo.Infrastructure.Events;

namespace Euricom.Cruise2018.Demo.Domain
{
    public class ApplicationEventPublisher : ReceiveActor
    {
        private readonly IApplicationEventPublisher _publisher;

        public ApplicationEventPublisher(IApplicationEventPublisher publisher)
        {
            _publisher = publisher;

            Receive<IApplicationEvent>(e => PublishAppEvent(e));
        }

        private void PublishAppEvent(IApplicationEvent @event)
        {
            _publisher.Publish(@event);
        }

        protected override void PreRestart(Exception reason, object message)
        {
            if (message != null)
            {
                Self.Tell(message);
            }
        }
    }
}
