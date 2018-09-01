namespace Euricom.Cruise2018.Demo.Projections
{
    public interface IProject { }

    public interface IProject<T> : IProject
    {
        void Project(T @event);
    }
}