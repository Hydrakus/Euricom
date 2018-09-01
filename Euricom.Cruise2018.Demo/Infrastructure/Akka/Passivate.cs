using System;
using Akka.Actor;

namespace Euricom.Cruise2018.Demo.Infrastructure.Akka
{
    public sealed class Passivate
    {
        public IActorRef PassivateActorRef { get; private set; }

        public Passivate(IActorRef passivatedActorRef)
        {
            PassivateActorRef = passivatedActorRef;
        }
    }
}
