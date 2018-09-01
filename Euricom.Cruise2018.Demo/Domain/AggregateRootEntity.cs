using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Euricom.Cruise2018.Demo.Infrastructure.Events;

namespace Euricom.Cruise2018.Demo.Domain
{
    public interface IAggregateRootEntity
    {
        string Id { get; }
        long Version { get; }
    }

    public abstract class AggregateRootEntity : IAggregateRootEntity
    {
        private readonly Dictionary<Type, Action<IVersionedEvent>> _applyMethods = new Dictionary<Type, Action<IVersionedEvent>>();

        public string Id { get; private set; }
        public long Version { get; private set; }

        protected AggregateRootEntity(string id)
        {
            Id = id;
            Version = 0;

            RegisterApplyMethods();
        }

        public void ApplyEvent(VersionedEvent @event)
        {
            Version = @event.Version;
            _applyMethods[@event.GetType()](@event);
        }

        private void RegisterApplyMethods()
        {
            var applyMethods = GetType()
                .GetRuntimeMethods()
                .Where(m => !m.IsStatic
                    && m.Name == "Apply"
                    && m.GetParameters().Length == 1
                    && typeof(IVersionedEvent).IsAssignableFrom(m.GetParameters().Single().ParameterType)
                    && m.ReturnType == typeof(void))
                .Select(m => new { Method = m, EventType = m.GetParameters().Single().ParameterType });

            foreach (var apply in applyMethods)
            {
                MethodInfo applyMethod = apply.Method;
                _applyMethods.Add(apply.EventType, m => applyMethod.Invoke(this, new[] { m }));
            }
        }
    }
}
