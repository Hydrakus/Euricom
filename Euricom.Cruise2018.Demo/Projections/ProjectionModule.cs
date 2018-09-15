using Autofac;
using Euricom.Cruise2018.Demo.Projections.PapierSettingPersoon;

namespace Euricom.Cruise2018.Demo.Projections
{
    public class ProjectionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Projections
            builder.RegisterType<PapierSettingPersoon.PapierSettingPersoonProjections>()
                .As<IProjections<Query.PapierSettingPersoon.PapierSettingPersoon>>()
                .InstancePerDependency();

            // Actors using injected dependencies
            builder.RegisterType<PapierSettingPersoonProjector>();
        }
    }
}
