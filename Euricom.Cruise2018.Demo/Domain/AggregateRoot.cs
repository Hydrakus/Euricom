using System;
using Akka.Actor;
using Akka.Persistence;
using Euricom.Cruise2018.Demo.Infrastructure.Akka;
using Euricom.Cruise2018.Demo.Infrastructure.Events;
using Euricom.Cruise2018.Demo.Infrastructure.Commands;

namespace Euricom.Cruise2018.Demo.Domain
{
    public abstract class AggregateRoot<TAggregateRootEntity> : ReceivePersistentActor
        where TAggregateRootEntity : AggregateRootEntity
    {
        private readonly string _persistenceId;
        private readonly string _aggregateId;

        private readonly IActorRef _applicationEventPublisher;

        public override string PersistenceId
        {
            get { return _persistenceId; }
        }

        protected TAggregateRootEntity State { get; private set; }

        protected string Status { get; private set; }

        protected AggregateRoot(string arId)
        {
            _aggregateId = arId;
            _persistenceId = String.Format("{0}@{1}", GetType().Name, arId);

            _applicationEventPublisher = Context.System.GetActorFromAddressBook(ActorAddresses.ApplicationEventPublisher);

            State = InitializeState(arId);

            InitializeBecomeStatus();

            RecoverAny(m =>
            {
                if (m is VersionedEvent)
                {
                    RecoverEvent((VersionedEvent)m);
                }
                else if (m is RecoveryCompleted)
                {
                    return; // Ignore
                }
                else
                {
                    Log.Warning("Encountered unknown message while recovering! Type: {0}", m.GetType().FullName);
                }
            });

            Context.SetReceiveTimeout(TimeSpan.FromSeconds(5));
        }

        protected abstract TAggregateRootEntity InitializeState(string aggregateId);

        protected void RaiseEvent(VersionedEvent @event, Action<VersionedEvent> onAfterPersistPublish = null)
        {
            @event.AggregateId = State.Id;
            @event.Version = State.Version + 1;

            Persist(@event, e =>
            {
                State.ApplyEvent(@event);
                BecomeStatus(@event);

                _applicationEventPublisher.Tell(@event);

                if (onAfterPersistPublish != null)
                {
                    onAfterPersistPublish(@event);
                }
            });
        }

        protected virtual bool BecomeStatus(VersionedEvent @event)
        {
            return true;
        }

        protected virtual void InitializeBecomeStatus()
        {
        }

        protected void Become(Action configure, string newAggregateStatus)
        {
            Status = newAggregateStatus;
            Become(() =>
            {
                configure();
                Command<ReceiveTimeout>(m => Context.Parent.Tell(new Passivate(Self)));
            });
        }

        protected void RecoverEvent(VersionedEvent @event)
        {
            State.ApplyEvent(@event);

            if (!BecomeStatus(@event))
            {
                throw new AggregateRecoveryException(@event);
            }
        }

        protected override void OnPersistFailure(Exception cause, object @event, long sequenceNr)
        {
            var errorMsg = string.Format("Unable to persist event of type '{0}', AR status was '{1}', AR id was '{2}'. Sequence nr '{3}'. Cause: {4}{5}",
                @event.GetType().FullName, Status, _aggregateId, sequenceNr, Environment.NewLine, cause);

            HandleFailure(errorMsg, @event);
        }

        protected override void Unhandled(object message)
        {
            var errorMsg = string.Format("Unhandled message type '{0}', AR status was '{1}', AR id was '{2}'.", message.GetType().FullName, Status, _aggregateId);

            HandleFailure(errorMsg, message);

            base.Unhandled(message);
        }

        protected override void PreRestart(Exception reason, object message)
        {
            var errorMsg = string.Format("Unhandled error occured. Message type '{0}'. AR status was '{1}', AR id was '{2}'. Reason: {3}{4}",
                message.GetType().FullName, Status, _aggregateId, Environment.NewLine, reason);

            HandleFailure(errorMsg, message);
        }

        private void HandleFailure(string error, object message)
        {
            if (message is ICommand)
            {
                Sender.Tell(CommandFeedback.CreateFailureFeedback(error));
            }
            else
            {
                Log.Error(error);
            }
        }
    }

    public class AggregateRecoveryException : Exception
    {
        public AggregateRecoveryException(VersionedEvent @event)
            : base(string.Format("Could not recover from event type '{0}'", @event.GetType().FullName))
        { }
    }
}
