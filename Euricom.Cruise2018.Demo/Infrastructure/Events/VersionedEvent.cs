namespace Euricom.Cruise2018.Demo.Infrastructure.Events
{
    /// <summary>
    /// Represents an event message that belongs to an ordered event stream.
    /// </summary>
    public interface IVersionedEvent : IApplicationEvent
    {
        /// <summary>
        /// Gets the version or order of the event in the stream.
        /// </summary>
        long Version { get; }
    }

    public abstract class VersionedEvent : IVersionedEvent
    {
        public string AggregateId { get; set; }

        public long Version { get; set; }

        public object ConsistentHashKey
        {
            get { return AggregateId; }
        }
    }
}
