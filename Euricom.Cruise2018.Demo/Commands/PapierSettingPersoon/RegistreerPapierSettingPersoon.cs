using BM2.RecipientData.NAVOUT.SharedKernel.ValueObjects;
using Euricom.Cruise2018.Demo.Infrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euricom.Cruise2018.Demo.Commands.PapierSettingPersoon
{
    public class RegistreerPapierSettingPersoon : IPapierSettingPersoonCommand
    {
        public string PerNummer { get; private set; }
        public string Naam { get; private set; }
        public string Voornaam { get; private set; }
        public Adres Adres { get; private set; }


        public RegistreerPapierSettingPersoon(string perNummer, string naam, string voornaam, Adres adres)
        {
            PerNummer = perNummer;
            Naam = naam;
            Voornaam = voornaam;
            Adres = adres;
        }

        public CommandValidationResult Validate()
        {
            var commandValidationResult = new CommandValidationResult();

            if (!CommandValidations.ValidateRequiredString(Naam, 50))
                commandValidationResult.AddValidationError(CommandValidations.GetRequiredStringValidationErrorMessage(nameof(Naam), Naam, 50));

            if (!CommandValidations.ValidateRequiredString(Voornaam, 50))
                commandValidationResult.AddValidationError(CommandValidations.GetRequiredStringValidationErrorMessage(nameof(Voornaam), Voornaam, 50));

            if (!CommandValidations.ValidateRequiredString(PerNummer, 11))
                commandValidationResult.AddValidationError(CommandValidations.GetRequiredStringValidationErrorMessage(nameof(PerNummer), PerNummer, 11));
            
            return commandValidationResult;
        }
    }
}
