using BM2.RecipientData.NAVOUT.SharedKernel.ValueObjects;
using Euricom.Cruise2018.Demo.ApplicationEvents.PapierSettingPersoon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euricom.Cruise2018.Demo.Domain.PapierSettingPersoon.Model
{
    public class PaperSettingPersoon : AggregateRootEntity
    {
        public string PerNummer { get; private set; }
        public string Naam { get; private set; }
        public string Voornaam { get; private set; }
        public bool? PapierAan { get; set; }
        public Adres Adres { get; private set; }

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
            Adres = @event.Adres;
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
    }
}
