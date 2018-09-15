using Euricom.Cruise2018.Demo.Infrastructure.Commands;

namespace Euricom.Cruise2018.Demo.Commands.PapierSettingPersoon
{
    public class RegistreerPapierSettingPersoon : IPapierSettingPersoonCommand
    {
        public string PerNummer { get; private set; }
        public string Naam { get; private set; }
        public string Voornaam { get; private set; }
        public string Straat { get; private set; }
        public string Nummer { get; private set; }
        public string Bus { get; private set; }
        public string Postcode { get; private set; }
        public string Gemeente { get; private set; }


        public RegistreerPapierSettingPersoon(string perNummer, string naam, string voornaam, string straat,
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
