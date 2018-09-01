using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Euricom.Cruise2018.Demo.Infrastructure.Events;
using Euricom.Cruise2018.Demo.Query;

namespace Euricom.Cruise2018.Demo.Projections
{
    public class ReadModelProjectionHandler<TReadModel> : IReadModelProjectionHandler<TReadModel>
         where TReadModel : class, new()
    {
        private readonly IProjections<TReadModel> _projections;
        private readonly Func<QueryContext> _queryContextFactory;

        public ReadModelProjectionHandler(IProjections<TReadModel> projections, Func<QueryContext> queryContextFactory)
        {
            _projections = projections;
            _queryContextFactory = queryContextFactory;
        }

        public virtual void InsertProjection(VersionedEvent @event)
        {
            var rm = new TReadModel();
            _projections.Project(ref rm, @event);

            using (var db = _queryContextFactory())
            {
                Insert(db, rm);
            }
        }

        public virtual void UpdateProjection(VersionedEvent @event, Expression<Func<TReadModel, bool>> rmSelection, params Expression<Func<TReadModel, object>>[] includeProperties)
        {
            using (var db = _queryContextFactory())
            {
                var rm = GetReadModel(db, rmSelection, includeProperties);

                Update(db, @event, rm);
            }
        }

        public virtual void UpsertProjection(VersionedEvent @event, Expression<Func<TReadModel, bool>> rmSelection, params Expression<Func<TReadModel, object>>[] includeProperties)
        {
            using (var db = _queryContextFactory())
            {
                var rm = GetReadModel(db, rmSelection, includeProperties);

                if (rm == null)
                {
                    rm = new TReadModel();
                    _projections.Project(ref rm, @event);

                    Insert(db, rm);
                }
                else
                {
                    Update(db, @event, rm);
                }
            }
        }

        public virtual void DeleteProjections(VersionedEvent @event, Expression<Func<TReadModel, bool>> rmSelection)
        {
            using (var db = _queryContextFactory())
            {
                var dbSet = db.Set<TReadModel>();
                var rms = dbSet.Where(rmSelection);

                dbSet.RemoveRange(rms);

                db.SaveChanges();
            }
        }

        protected virtual bool CheckEventVersion(long eventVersion, long readModelVersion)
        {
            return eventVersion == readModelVersion + 1;
        }

        private static TReadModel GetReadModel(DbContext db, Expression<Func<TReadModel, bool>> rmSelection, params Expression<Func<TReadModel, object>>[] includeProperties)
        {
            var set = db.Set<TReadModel>().AsQueryable();

            foreach (var include in includeProperties)
            {
                set = set.Include(include);
            }

            return set.FirstOrDefault(rmSelection);
        }

        private static void Insert(DbContext db, TReadModel rm)
        {
            db.Set<TReadModel>().Add(rm);
            db.SaveChanges();
        }

        private void Update(DbContext db, VersionedEvent @event, TReadModel rm)
        {
            if (rm == null)
            {
                throw new Exception(string.Format("Geen {0} gevonden met AR Id '{1}' bij het projecteren van een '{2}' applicatie event.",
                   typeof(TReadModel).Name, @event.AggregateId, @event.GetType()));
            }

            var rmVersion = GetValue<long>(rm, "Version");

            if (CheckEventVersion(@event.Version, rmVersion))
            {
                _projections.Project(ref rm, @event);
                db.SaveChanges();
            }
            else
            {
             
                var idProperty = typeof(TReadModel).Name + "Id";
                var id = GetValue<string>(rm, idProperty);

                throw new Exception(string.Format("Kan {0} '{0}', versie {1} niet updaten. Versie van het '{2}' applicatie event is {3}.",
                    id, rmVersion, @event.GetType(), @event.Version));
            }
        }

        private static T GetValue<T>(TReadModel readModel, string property)
        {
            var propInfo = typeof(TReadModel).GetProperty(property);
            if (propInfo == null)
            {
                throw new Exception(string.Format("Kan property '{0}' van readmodel {1} niet vinden.",
                    property, readModel.GetType().Name));
            }

            var retVal = propInfo.GetValue(readModel);
            if (retVal == null)
            {
                throw new Exception(string.Format("Kan waarde van property '{0}' niet ophalen. Readmodel {1}.",
                    property, readModel.GetType().Name));
            }

            return (T)propInfo.GetValue(readModel);
        }
    }
}
