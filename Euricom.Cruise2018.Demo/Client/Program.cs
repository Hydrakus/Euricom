using Euricom.Cruise2018.Demo.BusinessEvents;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Euricom.Cruise2018.Demo.Client
{
    class Program
    {
        private static string _toolName = "Euricom.Cruise2018.Demo.Client";
        private static Dictionary<int, string> _menuDictionary;
        private static string rabbitMqAddress = "rabbitmq://localhost";
        private static string rabbitMqQueue = "euricom.cruise2018.demo.businessevents";
        private static string _pernummer;
        private static IBusControl _bus;

        static void Main(string[] args)
        {
            Console.Title = _toolName;

            ConfigureBus();

            _menuDictionary = new Dictionary<int, string>
            {
                {1, "PersoonGeregistreerd"},
                {2, "PapierSettingGekozen_PapierAan"},
                {3, "PapierSettingGekozen_PapierUit"},
                {4, "PersoonUitgeschreven" }
            };

            RunMenu(_menuDictionary);
        }

        private static void RunMenu(Dictionary<int, string> menuDictionary)
        {
            Console.Clear();

            ConsoleHelper.BuildMenu("Enter number of command", menuDictionary);
            var menuChoice = Console.ReadLine();

            switch (menuChoice)
            {
                case "1":
                    SendPersoonGeregistreerd();
                    break;
                case "2":
                    SendPapierSettingGekozen(true);
                    break;
                case "3":
                    SendPapierSettingGekozen(false);
                    break;
                case "4":
                    SendPersoonUitgeschreven();
                    break;
                case "exit":
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }

            RunMenu(_menuDictionary);  
        }

        private static void SendPersoonGeregistreerd()
        {
            Console.WriteLine("Geef een persoonnummer in:");
            _pernummer = Console.ReadLine();
            Task<ISendEndpoint> sendEndpointTask = _bus.GetSendEndpoint(new Uri(string.Concat(rabbitMqAddress, "/", rabbitMqQueue)));
            ISendEndpoint sendEndpoint = sendEndpointTask.Result;

            sendEndpoint.Send(new PersoonGeregistreerd()
            {
                PerNummer = _pernummer,
                Naam = "Euricom",
                Voornaam = "Demo",
                Straat = "Schalïenhoevedreef",
                Nummer = "20",
                Bus = "J",
                Postcode = "2800",
                Gemeente = "Mechelen"
            });
        }

        private static void SendPapierSettingGekozen(bool papierAan)
        {
            Task<ISendEndpoint> sendEndpointTask = _bus.GetSendEndpoint(new Uri(string.Concat(rabbitMqAddress, "/", rabbitMqQueue)));
            ISendEndpoint sendEndpoint = sendEndpointTask.Result;

            sendEndpoint.Send(new PapiersettingGekozen
            {
                PerNummer = _pernummer,
                PapierAan = papierAan
            });
        }

        private static void SendPersoonUitgeschreven()
        {
            Task<ISendEndpoint> sendEndpointTask = _bus.GetSendEndpoint(new Uri(string.Concat(rabbitMqAddress, "/", rabbitMqQueue)));
            ISendEndpoint sendEndpoint = sendEndpointTask.Result;

            sendEndpoint.Send(new PersoonUitgeschreven {
                PerNummer = _pernummer
            });
        }

        private static void ConfigureBus()
        {
            Uri rabbitMqRootUri = new Uri(rabbitMqAddress);

            _bus = Bus.Factory.CreateUsingRabbitMq(rabbit =>
            {
                rabbit.Host(rabbitMqRootUri, settings =>
                {
                    settings.Password("guest");
                    settings.Username("guest");
                });
            });
        }
    }
}
