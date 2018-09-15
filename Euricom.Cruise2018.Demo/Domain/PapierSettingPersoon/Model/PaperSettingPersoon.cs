using Euricom.Cruise2018.Demo.ApplicationEvents.PapierSettingPersoon;
using Euricom.Cruise2018.Demo.Domain.ValueObjects;

namespace Euricom.Cruise2018.Demo.Domain.PapierSettingPersoon.Model
{
    public class PaperSettingPersoon : AggregateRootEntity
    {
        public string PerNummer { get; private set; }
        public string Naam { get; private set; }
        public string Voornaam { get; private set; }
        public bool? PapierAan { get; set; }
        public Adres Adres { get; private set; }
        public bool IsActief { get; private set; }

        public PaperSettingPersoon(string id) : base(id)
        {
        }

        public static PaperSettingPersoon Create(string id)
        {
            return new PaperSettingPersoon(id);
        }

        private void Apply(PapierSettingPersoonGeregistreerd @event)
        {
            PerNummer = @event.PerNummer;
            Naam = @event.Naam;
            Voornaam = @event.Voornaam;
            Adres = new Adres(@event.Straat, @event.Nummer, @event.Bus, @event.Postcode, @event.Gemeente);
            IsActief = true;
        }

        private void Apply(PapierSettingPersoonPapierAangezet @event)
        {
            PerNummer = @event.PerNummer;
            PapierAan = true;
        }

        private void Apply(PapierSettingPersoonPapierUitgezet @event)
        {
            PerNummer = @event.PerNummer;
            PapierAan = false;
        }

        private void Apply(PapierSettingPersoonUitgeschreven @event)
        {
            PerNummer = @event.PerNummer;
            IsActief = false;
        }
    }
}
