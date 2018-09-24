using System;
using Euricom.Cruise2018.Demo.Infrastructure.Events;

namespace Euricom.Cruise2018.Demo.Projections
{
    public sealed class ProjectApplicationEvent
    {
        public IApplicationEvent Event { get; private set; }

        public ProjectApplicationEvent(IApplicationEvent @event)
        {
            Event = @event;
        }
    }
}