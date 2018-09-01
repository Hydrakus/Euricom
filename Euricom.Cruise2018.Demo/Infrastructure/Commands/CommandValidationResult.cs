using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euricom.Cruise2018.Demo.Infrastructure.Commands
{
    public class CommandValidationResult
    {
        public bool Valid { get { return ValidationErrors.Count == 0; } }

        public List<string> ValidationErrors { get; private set; }

        public CommandValidationResult()
        {
            ValidationErrors = new List<string>();
        }

        public void AddValidationError(string error)
        {
            ValidationErrors.Add(error);
        }

        public void MergeWith(CommandValidationResult other)
        {
            ValidationErrors.AddRange(other.ValidationErrors);
        }
    }
}
