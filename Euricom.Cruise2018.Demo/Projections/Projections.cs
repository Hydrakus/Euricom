using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Euricom.Cruise2018.Demo.Infrastructure.Events;

namespace Euricom.Cruise2018.Demo.Projections
{
    public interface IProjections<TReadModel>
    {
        void Project<TEvent>(ref TReadModel rm, TEvent @event) where TEvent : VersionedEvent;
    }

    public abstract class Projections<TReadModel> : IProjections<TReadModel>
    {
        private Dictionary<Type, MethodInfo> _projectMethods = new Dictionary<Type, MethodInfo>();
        private readonly Type _readModelType;
        private readonly Type _projectionsType;

        public Projections()
        {
            _readModelType = typeof(TReadModel);
            _projectionsType = GetType();
        }

        public void Project<TEvent>(ref TReadModel rm, TEvent @event) where TEvent : VersionedEvent
        {
            var eventType = @event.GetType();

            if (!_projectMethods.Any(kvp => kvp.Key == eventType))
            {
                var mi = _projectionsType.GetMethod("Project", new Type[] { _readModelType.MakeByRefType(), eventType });
                if (mi != null)
                {
                    _projectMethods.Add(eventType, mi);
                }
                else
                {
                    throw new NotImplementedException(string.Format("No projection defined for event type '{0}' in '{1}'",
                        eventType.FullName, _projectionsType.FullName));
                }
            }

            _projectMethods[eventType].Invoke(this, new object[] { rm, @event });
        }

    }
}