using Akka;
using Akka.Actor;
using Euricom.Cruise2018.Demo.ApplicationEvents.PapierSettingPersoon;
using Euricom.Cruise2018.Demo.Commands.PapierSettingPersoon;
using Euricom.Cruise2018.Demo.Domain.PapierSettingPersoon.Model;
using Euricom.Cruise2018.Demo.Infrastructure.Commands;
using Euricom.Cruise2018.Demo.Infrastructure.Events;

namespace Euricom.Cruise2018.Demo.Domain.PapierSettingPersoon
{
    public class PapierSettingPersoonAR : AggregateRoot<Model.PaperSettingPersoon>
    {
        public PapierSettingPersoonAR(string arId) : base(arId)
        {
        }

        protected override PaperSettingPersoon InitializeState(string aggregateId)
        {
            return Model.PaperSettingPersoon.Create(aggregateId);
        }

        protected override void InitializeBecomeStatus()
        {
            Become(Initialized, nameof(Initialized));
        }

        protected override bool BecomeStatus(VersionedEvent @event)
        {
            return @event.Match()
                .With<PapierSettingPersoonGeregistreerd>(() => Become(Geregistreerd, nameof(Geregistreerd)))
                .With<PapierSettingPersoonPapierAangezet>(() => Become(PapierAan, nameof(PapierAan)))
                .With<PapierSettingPersoonPapierUitgezet>(() => Become(PapierUit, nameof(PapierUit)))
                .With<PapierSettingPersoonUitgeschreven>(() => Become(Uitgeschreven, nameof(Uitgeschreven)))
                .WasHandled;
        }

        private void Initialized()
        {
            Command<RegistreerPapierSettingPersoon>(c => Handle(c));
        }

        private void Geregistreerd()
        {
            Command<ZetPapierAan>(c => Handle(c));
            Command<ZetPapierUit>(c => Handle(c));     
        }

        private void PapierAan()
        {
            Command<ZetPapierUit>(c => Handle(c));        
        }

        private void PapierUit()
        {
            Command<ZetPapierAan>(c => Handle(c));
        }

        private void Uitgeschreven()
        {
            Command<RegistreerPapierSettingPersoon>(c => Handle(c));
        }

        private void Handle(RegistreerPapierSettingPersoon command)
        {
            RaiseEvent(new PapierSettingPersoonGeregistreerd(command.PerNummer, command.Naam, command.Voornaam, command.Straat, command.Nummer,
                command.Bus, command.Postcode, command.Gemeente),
                e => Sender.Tell(CommandFeedback.CreateSuccessFeedback()));
        }

        private void Handle(ZetPapierAan command)
        {
            RaiseEvent(new PapierSettingPersoonPapierAangezet(command.PerNummer),
              e => Sender.Tell(CommandFeedback.CreateSuccessFeedback()));
        }

        private void Handle(ZetPapierUit command)
        {
            RaiseEvent(new PapierSettingPersoonPapierUitgezet(command.PerNummer),
              e => Sender.Tell(CommandFeedback.CreateSuccessFeedback()));
        }
    }
}
