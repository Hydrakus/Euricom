using System;
using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Euricom.Cruise2018.Demo.Infrastructure.Events;

namespace Euricom.Cruise2018.Demo.Projections
{
    public class ProjectionCoordinator : ReceiveActor, IWithUnboundedStash
    {
        public sealed class Subscribe
        {
            public Type EventType { get; private set; }

            public Subscribe(Type eventType)
            {
                EventType = eventType;
            }
        }

        public sealed class StartProjecting
        { }

        public sealed class Project
        {
            public Guid Id { get; private set; }
            public IApplicationEvent Event { get; private set; }

            public Project(Guid id, IApplicationEvent @event)
            {
                Id = id;
                Event = @event;
            }
        }

        public sealed class ProjectionsCompleted
        {
            public bool Successfully { get; private set; }

            public ProjectionsCompleted(bool successfully)
            {
                Successfully = successfully;
            }
        }

        private Dictionary<Type, List<IActorRef>> _subscriptions;
        private Guid _correlationId;
        private IActorRef _projectRequestor;
        private List<IActorRef> _subscribersForEvent;
        private bool _projectionsOK;
        private ICancelable _scheduler;

        public IStash Stash { get; set; }

        public ProjectionCoordinator()
        {
            _subscriptions = new Dictionary<Type, List<IActorRef>>();

            Become(WaitingForSubscribers);
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy((ex) => Akka.Actor.SupervisorStrategy.DefaultStrategy.Decider.Decide(ex), false);
        }

        protected override void Unhandled(object message)
        {
            var errorMsg = string.Format("Unhandled message type '{0}'", message.GetType().FullName);
            Context.System.Log.Error(errorMsg);

            base.Unhandled(message);
        }

        protected override void PreRestart(Exception reason, object message)
        {
            string errorMsg;
            var @event = message as IApplicationEvent;
            if (@event != null)
            {
                errorMsg = string.Format("An unhandled exception occured while projecting an application event of type '{0}' for aggregate id '{1}'.",
                    @event.GetType().FullName, @event.AggregateId);
            }
            else
            {
                errorMsg = string.Format("An unhandled exception occured while projecting an application event. Message type '{0}'.",
                    message.GetType().FullName);
            }

            Context.System.Log.Error(reason, errorMsg);

            Sender.Tell(new ProjectionsCompleted(false));
        }

        private void WaitingForSubscribers()
        {
            Receive<Subscribe>(m => SubscribeForEvent(m.EventType, Sender));
            Receive<StartProjecting>(m => HandleStartProjecting());
            Receive<Project>(m => Stash.Stash());
        }

        private void HandleStartProjecting()
        {
            Context.System.Log.Info("Ready for Project requests!");
            Become(WaitingForProjectionRequest);
            Stash.UnstashAll();
        }

        private void WaitingForProjectionRequest()
        {
            Receive<Project>(m => SendEventToProjectors(m.Id, m.Event));
        }

        private void WaitingForProjectorResponses()
        {
            Receive<Project>(m => SendEventToProjectors(m.Id, m.Event));

            Receive<ProjectionSucceeded>(m => HandleProjectionSucceeded(m));
            Receive<ProjectionFailed>(m => HandleProjectionFailed(m));
        }

        private void SubscribeForEvent(Type eventType, IActorRef subscriber)
        {
            _scheduler.CancelIfNotNull();

            if (!_subscriptions.Keys.Contains(eventType))
            {
                _subscriptions.Add(eventType, new List<IActorRef> { subscriber });
                _scheduler = Context.System.Scheduler.ScheduleTellOnceCancelable(TimeSpan.FromSeconds(5), Self, new StartProjecting(), Self);
                return;
            }

            if (!_subscriptions[eventType].Contains(subscriber))
            {
                _subscriptions[eventType].Add(subscriber);
            }
            _scheduler = Context.System.Scheduler.ScheduleTellOnceCancelable(TimeSpan.FromSeconds(5), Self, new StartProjecting(), Self);
        }

        private void SendEventToProjectors(Guid correlationId, IApplicationEvent @event)
        {
            var eventType = @event.GetType();

            _correlationId = correlationId;
            _subscriptions[eventType].ForEach(p => p.Tell(new ProjectApplicationEvent(correlationId, @event)));
            _subscribersForEvent = new List<IActorRef>();
            _subscribersForEvent.AddRange(_subscriptions[eventType]);
            _projectRequestor = Sender;
            _projectionsOK = true;

            Become(WaitingForProjectorResponses);
        }

        private bool HandleProjectionSucceeded(ProjectionSucceeded m)
        {
            if (m.CorrelationId != _correlationId)
            {
                Context.System.Log.Warning("ProjectionSucceeded message received with wrong CorrelationId!");
                return false;
            }

            _subscribersForEvent.Remove(Sender);
            if (AllProjectorsAnswered()) NotifyProjectionRequestor();

            return true;
        }

        private bool HandleProjectionFailed(ProjectionFailed m)
        {
            if (m.CorrelationId != _correlationId)
            {
                Context.System.Log.Warning("ProjectionFailed message received with wrong CorrelationId!");
                return false;
            }

            _projectionsOK = false;
            _subscribersForEvent.Remove(Sender);
            if (AllProjectorsAnswered()) NotifyProjectionRequestor();

            return true;
        }

        private bool AllProjectorsAnswered()
        {
            return _subscribersForEvent.Count == 0;
        }

        private void NotifyProjectionRequestor()
        {
            _projectRequestor.Tell(new ProjectionsCompleted(_projectionsOK));

            Become(WaitingForProjectionRequest);
        }
    }
}