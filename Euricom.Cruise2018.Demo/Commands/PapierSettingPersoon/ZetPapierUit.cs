using Euricom.Cruise2018.Demo.Infrastructure.Commands;

namespace Euricom.Cruise2018.Demo.Commands.PapierSettingPersoon
{
    public class ZetPapierUit : IPapierSettingPersoonCommand
    {
        public string PerNummer { get; private set; }

        public ZetPapierUit(string perNummer)
        {
            PerNummer = perNummer;
        }

        public CommandValidationResult Validate()
        {
            var commandValidationResult = new CommandValidationResult();

            if (!CommandValidations.ValidateRequiredString(PerNummer, 11))
                commandValidationResult.AddValidationError(CommandValidations.GetRequiredStringValidationErrorMessage(nameof(PerNummer), PerNummer, 11));

            return commandValidationResult;
        }
    }
}
