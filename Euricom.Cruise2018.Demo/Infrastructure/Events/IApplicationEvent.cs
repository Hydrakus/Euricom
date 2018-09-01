using Akka.Routing;

namespace Euricom.Cruise2018.Demo.Infrastructure.Events
{
    /// <summary>
    /// Represents a domain/application event message.
    /// </summary>
    public interface IApplicationEvent : IEvent, IConsistentHashable
    {
        /// <summary>
        /// Gets the identifier of the aggregate originating the event.
        /// </summary>
        string AggregateId { get; }
    }
}
