using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euricom.Cruise2018.Demo.BusinessEvents
{
    public class PersoonGeregistreerd
    {
        public string PerNummer { get; set; }
        public string Naam { get; set; }
        public string Voornaam { get; set; }
        public string Straat { get; set; }
        public string Nummer { get; set; }
        public string Bus { get; set; }
        public string Postcode { get; set; }
        public string Gemeente { get; set; }
    }
}
