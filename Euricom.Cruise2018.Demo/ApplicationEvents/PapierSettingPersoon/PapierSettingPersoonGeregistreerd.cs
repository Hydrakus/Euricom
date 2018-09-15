using Euricom.Cruise2018.Demo.Infrastructure.Events;

namespace Euricom.Cruise2018.Demo.ApplicationEvents.PapierSettingPersoon
{
    public class PapierSettingPersoonGeregistreerd : VersionedEvent
    {
        public string PerNummer { get; private set; }
        public string Naam { get; private set; }
        public string Voornaam { get; private set; }
        public string Straat { get; private set; }
        public string Nummer { get; private set; }
        public string Bus { get; private set; }
        public string Postcode { get; private set; }
        public string Gemeente { get; private set; }

        public PapierSettingPersoonGeregistreerd(string perNummer, string naam, string voornaam, string straat, 
            string nummer, string bus, string postcode, string gemeente)
        {
            PerNummer = perNummer;
            Naam = naam;
            Voornaam = voornaam;
            Straat = straat;
            Nummer = nummer;
            Bus = bus;
            Postcode = postcode;
            Gemeente = gemeente;
        }
    }
}
