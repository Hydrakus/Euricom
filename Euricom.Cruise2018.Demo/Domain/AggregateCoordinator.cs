using System;
using Akka.Actor;
using Euricom.Cruise2018.Demo.Infrastructure.Akka;
using Euricom.Cruise2018.Demo.Infrastructure.Commands;

namespace Euricom.Cruise2018.Demo.Domain
{
    public abstract class AggregateCoordinator : ReceiveActor
    {
        protected AggregateCoordinator()
        {
            Receive<Passivate>(HandlePassivate);
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
            var errorMsg = string.Format("An unhandled exception occured while executing a message of type '{0}'", message.GetType().FullName);
            Context.System.Log.Error(reason, errorMsg);
        }

        protected void ForwardCommand(string arId, ICommand command)
        {
            var child = Context.Child(arId);
            if (child.Equals(ActorRefs.Nobody))
            {
                child = Create(arId);
            }
            child.Forward(command);
        }

        public abstract Props GetProps(string arId);

        private bool HandlePassivate(Passivate p)
        {
            Context.Unwatch(p.PassivateActorRef);
            Context.Stop(p.PassivateActorRef);

            return true;
        }

        private IActorRef Create(string arId)
        {
            var aggregateRef = Context.ActorOf(GetProps(arId), arId);
            Context.Watch(aggregateRef);

            return aggregateRef;
        }
    }
}
