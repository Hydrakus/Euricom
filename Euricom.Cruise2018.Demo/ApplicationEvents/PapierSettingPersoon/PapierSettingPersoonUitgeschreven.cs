using Euricom.Cruise2018.Demo.Infrastructure.Events;

namespace Euricom.Cruise2018.Demo.ApplicationEvents.PapierSettingPersoon
{
    public class PapierSettingPersoonUitgeschreven : VersionedEvent
    {
        public string PerNummer { get; private set; }

        public PapierSettingPersoonUitgeschreven(string perNummer)
        {
            PerNummer = perNummer;
        }
    }
}
