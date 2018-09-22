using Euricom.Cruise2018.Demo.ApplicationEvents.PapierSettingPersoon;
using RM = Euricom.Cruise2018.Demo.Query.PapierSettingPersoon;

namespace Euricom.Cruise2018.Demo.Projections.PapierSettingPersoon
{
    public class PapierSettingPersoonProjections : Projections<RM.PapierSettingPersoon>
    {
        public void Project(ref RM.PapierSettingPersoon rm, PapierSettingPersoonGeregistreerd @event)
        {
            rm.PerNummer = @event.PerNummer;
            rm.Naam = @event.Naam;
            rm.Voornaam = @event.Voornaam;
            rm.IsActief = true;

            rm.Adres = new Query.Adres()
            {
                Straat = @event.Straat,
                Nummer = @event.Nummer,
                Gemeente = @event.Gemeente,
                Postcode = @event.Postcode,
                Bus = @event.Bus,
            };

            rm.Version = @event.Version;
            rm.PapierSettingPersoonId = @event.AggregateId;
        }

        public void Project(ref RM.PapierSettingPersoon rm, PapierSettingPersoonPapierAangezet @event)
        {
            rm.PerNummer = @event.PerNummer;
            rm.PapierAan = true;

            rm.Version = @event.Version;
            rm.PapierSettingPersoonId = @event.AggregateId;
        }

        public void Project(ref RM.PapierSettingPersoon rm, PapierSettingPersoonPapierUitgezet @event)
        {
            rm.PerNummer = @event.PerNummer;
            rm.PapierAan = false;

            rm.Version = @event.Version;
            rm.PapierSettingPersoonId = @event.AggregateId;
        }
    }
}
