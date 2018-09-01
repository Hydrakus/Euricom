namespace Euricom.Cruise2018.Demo.Infrastructure.Events
{
    public interface IApplicationEventPublisher
    {
        void Publish(IApplicationEvent @event);
    }
}
