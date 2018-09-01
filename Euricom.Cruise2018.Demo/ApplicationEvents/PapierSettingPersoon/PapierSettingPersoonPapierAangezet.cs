using Euricom.Cruise2018.Demo.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euricom.Cruise2018.Demo.ApplicationEvents.PapierSettingPersoon
{
    public class PapierSettingPersoonPapierAangezet: VersionedEvent
    {
        public string PerNummer { get; private set; }

        public PapierSettingPersoonPapierAangezet(string perNummer)
        {
            PerNummer = perNummer;
        }
    }
}
