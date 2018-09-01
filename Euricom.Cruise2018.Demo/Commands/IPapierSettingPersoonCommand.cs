using Euricom.Cruise2018.Demo.Infrastructure.Commands;

namespace Euricom.Cruise2018.Demo.Commands
{
    public interface IPapierSettingPersoonCommand : ICommand
    {
        string PerNummer { get; }
    }
}
