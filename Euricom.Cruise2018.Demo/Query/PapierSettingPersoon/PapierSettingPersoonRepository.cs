using System.Linq;

namespace Euricom.Cruise2018.Demo.Query.PapierSettingPersoon
{
    public interface IPapierSettingPersoonRepository
    {
        IQueryable<PapierSettingPersoon> GetQueryable();
    }

    public class PapierSettingPersoonRepository : IPapierSettingPersoonRepository
    {
        private readonly QueryContext _queryContext;

        public PapierSettingPersoonRepository(QueryContext queryContext)
        {
            _queryContext = queryContext;
        }

        public IQueryable<PapierSettingPersoon> GetQueryable()
        {
            return _queryContext.PapierSettingsPersoon.AsNoTracking().AsQueryable();
        }
    }
}
