using Akka.Actor;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Euricom.Cruise2018.Demo.Projections
{
    public abstract class ReadModelProjector : ReceiveActor
    {
        private readonly Dictionary<Type, MethodInfo> _projectMethods = new Dictionary<Type, MethodInfo>();
        protected readonly IActorRef _projectionCoordinator;

        public ReadModelProjector(IActorRef projectionCoordinator)
        {
            _projectionCoordinator = projectionCoordinator;

            RegisterProjectMethods();
            RegisterWithCoordinator();

            Receive<ProjectApplicationEvent>(m => ProjectAE(m));
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

        private void RegisterWithCoordinator()
        {
            var aeTypes = this.GetType()
                .GetInterfaces()
                .Where(i => i.IsGenericType)
                .Where(i => i.GetGenericTypeDefinition() == typeof(IProject<>))
                .Select(i => i.GetGenericArguments()[0])
                .ToList();

            aeTypes.ForEach(ae => _projectionCoordinator.Tell(new ProjectionCoordinator.Subscribe(ae)));
        }

        private void ProjectAE(ProjectApplicationEvent message)
        {
            try
            {
                _projectMethods[message.Event.GetType()].Invoke(this, new object[] { (object)message.Event });

                Sender.Tell(new ProjectionSucceeded());
            }
            catch (Exception ex)
            {
                Context.System.Log.Error(ex, "An exception occured while projecting an application event of type '{0}' for aggregate id '{1}'!",
                    message.Event.GetType().ToString(), message.Event.AggregateId);

                Sender.Tell(new ProjectionFailed());
            }
        }

        protected virtual bool CheckEventVersion(long readModelVersion, long eventVersion)
        {
            return eventVersion == readModelVersion + 1;
        }
    }
}
