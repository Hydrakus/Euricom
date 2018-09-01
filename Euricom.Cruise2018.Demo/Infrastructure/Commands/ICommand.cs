using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euricom.Cruise2018.Demo.Infrastructure.Commands
{
    public interface ICommand
    {
        CommandValidationResult Validate();
    }
}
