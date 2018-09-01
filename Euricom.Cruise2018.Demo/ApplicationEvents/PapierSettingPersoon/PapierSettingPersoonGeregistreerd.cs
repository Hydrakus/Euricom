using BM2.RecipientData.NAVOUT.SharedKernel.ValueObjects;
using Euricom.Cruise2018.Demo.Infrastructure.Events;

namespace Euricom.Cruise2018.Demo.ApplicationEvents.PapierSettingPersoon
{
    public class PapierSettingPersoonGeregistreerd : VersionedEvent
    {
        public string PerNummer { get; private set; }
        public string Naam { get; private set; }
        public string Voornaam { get; private set; }
        public Adres Adres { get; private set; }

        public PapierSettingPersoonGeregistreerd(string perNummer, string naam, string voornaam, Adres adres)
        {
            PerNummer = perNummer;
            Naam = naam;
            Voornaam = voornaam;
            Adres = adres;
        }
    }
}
