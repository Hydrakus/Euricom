using System;
using System.Linq.Expressions;
using Euricom.Cruise2018.Demo.Infrastructure.Events;

namespace Euricom.Cruise2018.Demo.Projections
{
    public interface IReadModelProjectionHandler<TReadModel>
    {
        void InsertProjection(VersionedEvent @event);

        void UpdateProjection(VersionedEvent @event, Expression<Func<TReadModel, bool>> rmSelection, params Expression<Func<TReadModel, object>>[] includeProperties);

        void UpsertProjection(VersionedEvent @event, Expression<Func<TReadModel, bool>> rmSelection, params Expression<Func<TReadModel, object>>[] includeProperties);

        void DeleteProjections(VersionedEvent @event, Expression<Func<TReadModel, bool>> rmSelection);
    }
}