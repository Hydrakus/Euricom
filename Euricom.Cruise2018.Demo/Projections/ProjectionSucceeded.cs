using System;

namespace Euricom.Cruise2018.Demo.Projections
{
    public sealed class ProjectionSucceeded
    {
        public Guid CorrelationId { get; private set; }

        public ProjectionSucceeded(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
    }
}