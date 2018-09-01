using Euricom.Cruise2018.Demo.Infrastructure.Events;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;

namespace Euricom.Cruise2018.Demo.Projections
{
    public interface IReadModelProjectionLogic
    {
        void Project(IApplicationEvent @event);
    }

    public abstract class ReadModelProjectionLogic : IReadModelProjectionLogic
    {
        private readonly Dictionary<Type, MethodInfo> _projectMethods = new Dictionary<Type, MethodInfo>();

        protected ReadModelProjectionLogic()
        {
            RegisterProjectMethods();
        }

        public void Project(IApplicationEvent @event)
        {
            _projectMethods[@event.GetType()].Invoke(this, new[] { (object)@event });
        }

        private void RegisterProjectMethods()
        {
            var projectTypes = this.GetType()
                .GetInterfaces()
                .Where(i => i.IsGenericType)
                .Where(i => i.GetGenericTypeDefinition() == typeof(IProject<>));

            foreach (var projectType in projectTypes)
            {
                var projectMethod = this.GetType().GetInterfaceMap(projectType).TargetMethods.First();
                _projectMethods.Add(projectMethod.GetParameters().First().ParameterType, projectMethod);
            }
        }
    }
}
