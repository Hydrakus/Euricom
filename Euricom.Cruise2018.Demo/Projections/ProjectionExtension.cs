﻿using Akka.Actor;
using Akka.DI.Core;
using Euricom.Cruise2018.Demo.Infrastructure.Akka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euricom.Cruise2018.Demo.Projections
{
    public class ProjectionExtension : IExtension
    {
        public ProjectionExtension(ExtendedActorSystem system)
        {
            // Projection coordinator
            var projectionCoordinator = system.ActorOf<ProjectionCoordinator>("projectioncoordinator");
            system.GetAddressBook().Tell(new AddressBook.Register(ActorAddresses.ProjectionCoordinator, projectionCoordinator), ActorRefs.NoSender);

            // Projectors
            var klantProjector = system.ActorOf(
               system.DI().Props<PapierSettingPersoon.PapierSettingPersoonProjector>(), "papiersettingpersoonprojector");

        }
    }

    public class ProjectionExtensionProvider : ExtensionIdProvider<ProjectionExtension>
    {
        public static readonly ProjectionExtensionProvider Instance = new ProjectionExtensionProvider();

        private ProjectionExtensionProvider() { }

        public override ProjectionExtension CreateExtension(ExtendedActorSystem system)
        {
            return new ProjectionExtension(system);
        }
    }
}
