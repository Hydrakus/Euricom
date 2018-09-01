using System;
using Euricom.Cruise2018.Demo.Infrastructure.Events;

namespace Euricom.Cruise2018.Demo.Projections
{
    public sealed class ProjectApplicationEvent
    {
        public Guid CorrelationId { get; private set; }
        public IApplicationEvent Event { get; private set; }

        public ProjectApplicationEvent(Guid correlationId, IApplicationEvent @event)
        {
            CorrelationId = correlationId;
            Event = @event;
        }
    }
}