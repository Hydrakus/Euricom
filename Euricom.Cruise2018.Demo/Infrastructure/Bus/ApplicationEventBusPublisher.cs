using Euricom.Cruise2018.Demo.Infrastructure.Events;
using MassTransit;

namespace Euricom.Cruise2018.Demo.Infrastructure.Bus
{
    public class ApplicationEventBusPublisher : IApplicationEventPublisher
    {
        private readonly IBus _bus;

        public ApplicationEventBusPublisher(IBus bus)
        {
            _bus = bus;
        }

        public void Publish(IApplicationEvent @event)
        {
            _bus.Publish(@event, @event.GetType());
        }
    }
}

