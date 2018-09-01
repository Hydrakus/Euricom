using Euricom.Cruise2018.Demo.Infrastructure.Events;

namespace Euricom.Cruise2018.Demo.ApplicationEvents.PapierSettingPersoon
{
    public class PapierSettingPersoonPapierUitgezet : VersionedEvent
    {
        public string PerNummer { get; private set; }

        public PapierSettingPersoonPapierUitgezet(string perNummer)
        {
            PerNummer = perNummer;
        }
    }
}
