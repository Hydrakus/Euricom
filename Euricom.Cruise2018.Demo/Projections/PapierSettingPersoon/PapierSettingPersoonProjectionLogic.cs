using Euricom.Cruise2018.Demo.ApplicationEvents.PapierSettingPersoon;
using System;
using RM = Euricom.Cruise2018.Demo.Query.PapierSettingPersoon;

namespace Euricom.Cruise2018.Demo.Projections.PapierSettingPersoon
{
    public class PapierSettingPersoonProjectionLogic : ReadModelProjectionLogic, 
        IProject<PapierSettingPersoonGeregistreerd>,
        IProject<PapierSettingPersoonPapierAangezet>,
        IProject<PapierSettingPersoonPapierUitgezet>
    {
        private readonly IReadModelProjectionHandler<RM.PapierSettingPersoon> _rmProjectionHandler;

        public PapierSettingPersoonProjectionLogic(IReadModelProjectionHandler<RM.PapierSettingPersoon> rmProjectionHandler)
        {
            _rmProjectionHandler = rmProjectionHandler;
        }

        public void Project(PapierSettingPersoonGeregistreerd @event)
        {
            _rmProjectionHandler.InsertProjection(@event);
        }

        public void Project(PapierSettingPersoonPapierAangezet @event)
        {
            _rmProjectionHandler.UpdateProjection(@event, (rm) => rm.PapierSettingPersoonId == @event.AggregateId);
        }

        public void Project(PapierSettingPersoonPapierUitgezet @event)
        {
            _rmProjectionHandler.UpdateProjection(@event, (rm) => rm.PapierSettingPersoonId == @event.AggregateId);
        }
    }
}
