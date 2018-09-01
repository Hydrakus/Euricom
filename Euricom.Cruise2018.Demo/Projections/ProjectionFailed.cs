using System;

namespace Euricom.Cruise2018.Demo.Projections
{
    public sealed class ProjectionFailed
    {
        public Guid CorrelationId { get; private set; }

        public ProjectionFailed(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
    }
}