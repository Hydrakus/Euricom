using RM = Euricom.Cruise2018.Demo.Query.PapierSettingPersoon;
using Autofac;

namespace Euricom.Cruise2018.Demo.Projections.PapierSettingPersoon
{
    public class PapierSettingPersoonProjector : ReadModelProjector
    {
        public PapierSettingPersoonProjector(ILifetimeScope lifetimeScope) : base(lifetimeScope)
        {
        }

        protected override IReadModelProjectionLogic ReadModelProjectionLogicFactory(ILifetimeScope lifetimeScope)
        {
            var projHandler = lifetimeScope.Resolve<IReadModelProjectionHandler<RM.PapierSettingPersoon>>();

            return new PapierSettingPersoonProjectionLogic(projHandler);
        }
    }
}
