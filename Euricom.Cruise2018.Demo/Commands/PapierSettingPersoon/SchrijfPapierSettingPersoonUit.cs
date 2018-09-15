using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Euricom.Cruise2018.Demo.Infrastructure.Commands;

namespace Euricom.Cruise2018.Demo.Commands.PapierSettingPersoon
{
    public class SchrijfPapierSettingPersoonUit : IPapierSettingPersoonCommand
    {
        public string PerNummer { get; private set; }

        public SchrijfPapierSettingPersoonUit(string perNummer)
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
