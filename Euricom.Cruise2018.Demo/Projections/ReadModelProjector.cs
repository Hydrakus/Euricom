using Akka.Actor;
using Autofac;
using Euricom.Cruise2018.Demo.Infrastructure.Akka;
using System;
using System.Linq;

namespace Euricom.Cruise2018.Demo.Projections
{
    public abstract class ReadModelProjector : ReceiveActor
    {
        private readonly IActorRef _projectionCoordinator;
        private readonly ILifetimeScope _lifetimeScope;

        private IReadModelProjectionLogic _rmProjLogic;

        protected ReadModelProjector(ILifetimeScope lifetimeScope)
        {
            _projectionCoordinator = Context.System.GetActorFromAddressBook(ActorAddresses.ProjectionCoordinator);
            _lifetimeScope = lifetimeScope;
        }

        protected override void PreStart()
        {
            _rmProjLogic = ReadModelProjectionLogicFactory(_lifetimeScope);

            RegisterWithCoordinator();

            Become(ReadyToProject);
        }

        private void ReadyToProject()
        {
            Receive<ProjectApplicationEvent>(m => ProjectAE(m));
        }

        protected abstract IReadModelProjectionLogic ReadModelProjectionLogicFactory(ILifetimeScope lifetimeScope);

        private void RegisterWithCoordinator()
        {
            var aeTypes = _rmProjLogic.GetType()
                .GetInterfaces()
                .Where(i => i.IsGenericType)
                .Where(i => i.GetGenericTypeDefinition() == typeof(IProject<>))
                .Select(i => i.GetGenericArguments()[0])
                .ToList();

            aeTypes.ForEach(ae => _projectionCoordinator.Tell(new ProjectionCoordinator.Subscribe(ae)));
        }

        private void ProjectAE(ProjectApplicationEvent message)
        {
            _rmProjLogic.Project(message.Event);

            Sender.Tell(new ProjectionSucceeded(message.CorrelationId));
        }

        protected override void PreRestart(Exception reason, object message)
        {
            string errorMsg;
            var @event = message as ProjectApplicationEvent;
            if (@event != null)
            {
                errorMsg = string.Format("An unhandled exception occured while projecting an application event of type '{0}' for aggregate id '{1}'.",
                    @event.Event.GetType().FullName, @event.Event.AggregateId);

                Sender.Tell(new ProjectionFailed(@event.CorrelationId));
            }
            else
            {
                errorMsg = string.Format("An unhandled exception occured while projecting an application event. Message type '{0}'.",
                    message.GetType().FullName);
            }
            Context.System.Log.Error(reason, errorMsg);
        }
    }
}
