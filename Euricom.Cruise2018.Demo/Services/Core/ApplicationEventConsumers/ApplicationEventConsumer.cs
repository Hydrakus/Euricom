using Akka.Actor;
using Euricom.Cruise2018.Demo.Infrastructure.Akka;
using Euricom.Cruise2018.Demo.Infrastructure.Events;
using Euricom.Cruise2018.Demo.Projections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euricom.Cruise2018.Demo.Services.Core.ApplicationEventConsumers
{
    public abstract class ApplicationEventConsumer
    {
        private readonly ActorSystem _actorSystem;

        public ApplicationEventConsumer(ActorSystem actorSystem)
        {
            _actorSystem = actorSystem;
        }

        protected void ProjectEvent(IApplicationEvent @event)
        {
            var projectionCoordinator = _actorSystem.GetActorFromAddressBook(ActorAddresses.ProjectionCoordinator);

            var projectFeedback = projectionCoordinator.Ask<ProjectionCoordinator.ProjectionsCompleted>(
                new ProjectionCoordinator.Project(Guid.NewGuid(), @event), TimeSpan.FromSeconds(20)).Result;

            if (!projectFeedback.Successfully)
            {
                throw new Exception(string.Format("Unable to successfully project {0} application event!", @event.GetType().FullName));
            }
        }
    }
}
