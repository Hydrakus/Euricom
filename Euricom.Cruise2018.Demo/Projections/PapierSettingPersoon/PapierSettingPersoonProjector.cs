using RM = Euricom.Cruise2018.Demo.Query.PapierSettingPersoon;
using Autofac;
using Euricom.Cruise2018.Demo.ApplicationEvents.PapierSettingPersoon;
using Akka.Actor;
using System;
using Euricom.Cruise2018.Demo.Query;
using Euricom.Cruise2018.Demo.Infrastructure.Events;
using System.Linq.Expressions;
using System.Linq;

namespace Euricom.Cruise2018.Demo.Projections.PapierSettingPersoon
{
    public class PapierSettingPersoonProjector : ReadModelProjector,
        IProject<PapierSettingPersoonGeregistreerd>,
        IProject<PapierSettingPersoonPapierAangezet>,
        IProject<PapierSettingPersoonPapierUitgezet>,
        IProject<PapierSettingPersoonUitgeschreven>
    {
        private readonly IProjections<RM.PapierSettingPersoon> _projections;
        private readonly Func<QueryContext> _queryContextFactory;

        public PapierSettingPersoonProjector(IActorRef projectionCoordinator, IProjections<RM.PapierSettingPersoon> projections,
            Func<QueryContext> queryContextFactory)
            : base(projectionCoordinator)
        {
            _projections = projections;
            _queryContextFactory = queryContextFactory;
        }

        public void Project(PapierSettingPersoonGeregistreerd @event)
        {
            UpsertPapierSettingPersoon(@event, (rm) => rm.PapierSettingPersoonId == @event.AggregateId);
        }

        public void Project(PapierSettingPersoonPapierAangezet @event)
        {
            UpdatePapierSettingPersoon(@event, (rm) => rm.PapierSettingPersoonId == @event.AggregateId);
        }

        public void Project(PapierSettingPersoonPapierUitgezet @event)
        {
            UpdatePapierSettingPersoon(@event, (rm) => rm.PapierSettingPersoonId == @event.AggregateId);
        }

        public void Project(PapierSettingPersoonUitgeschreven @event)
        {
            UpdatePapierSettingPersoon(@event, (rm) => rm.PapierSettingPersoonId == @event.AggregateId);
        }

        private void UpsertPapierSettingPersoon(VersionedEvent @event, Expression<Func<RM.PapierSettingPersoon, bool>> rmRecordSelection)
        {
            using (var db = _queryContextFactory())
            {
                var PapierSettingPersoon = db.PapierSettingsPersoon.FirstOrDefault(rmRecordSelection);

                if (PapierSettingPersoon == null)
                {
                    PapierSettingPersoon = new Query.PapierSettingPersoon.PapierSettingPersoon();
                    _projections.Project(ref PapierSettingPersoon, @event);

                    db.PapierSettingsPersoon.Add(PapierSettingPersoon);
                    db.SaveChanges();
                }
                else
                {
                    CheckVersionProjectAndUpdate(PapierSettingPersoon, @event, db);
                }
            }
        }

        private void UpdatePapierSettingPersoon(VersionedEvent @event, Expression<Func<RM.PapierSettingPersoon, bool>> rmRecordSelection)
        {
            using (var db = _queryContextFactory())
            {
                var PapierSettingPersoon = db.PapierSettingsPersoon.FirstOrDefault(rmRecordSelection);

                if (PapierSettingPersoon == null)
                    throw new Exception(string.Format("Geen PapierSettingPersoon gevonden met AR Id '{0}' bij het projecteren van een '{1}' applicatie event.",
                        @event.AggregateId, @event.GetType().ToString()));

                CheckVersionProjectAndUpdate(PapierSettingPersoon, @event, db);
            }
        }

        private void CheckVersionProjectAndUpdate(RM.PapierSettingPersoon rm, VersionedEvent @event, QueryContext db)
        {
            if (CheckEventVersion(rm.Version, @event.Version))
            {
                _projections.Project(ref rm, @event);
                db.SaveChanges();
            }
            else
            {
                throw new Exception(string.Format("Kan PapierSettingPersoon '{0}', versie {1} niet updaten. Versie van het '{2}' applicatie event is {3}.",
                    rm.PapierSettingPersoonId, rm.Version, @event.GetType().ToString(), @event.Version));
            }
        }
    }
}
