using BM2.RecipientData.NAVOUT.SharedKernel.ValueObjects;
using Euricom.Cruise2018.Demo.Commands.PapierSettingPersoon;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euricom.Cruise2018.Demo.Client
{
    class Program
    {
        private static string _toolName = "Euricom.Cruise2018.Demo.Client";
        private static Dictionary<int, string> _menuDictionary;
        private static string rabbitMqAddress = "rabbitmq://localhost";
        private static string rabbitMqQueue = "euricom.cruise2018.demo.commands";
        private static string _pernummer;
        private static IBusControl _bus;

        static void Main(string[] args)
        {
            Console.Title = _toolName;

            ConfigureBus();

            _menuDictionary = new Dictionary<int, string>
            {
                {1, "RegistreerPapierSettingPersoon"},
                {2, "ZetPapierAan"},
                {3, "ZetPapierUit"},
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
                    SendRegistreerPapierSettingPersoonCommand();
                    break;
                case "2":
                    SendZetPapierAanCommand();
                    break;
                case "3":
                    SendZetPapierUitCommand();
                    break;
                case "exit":
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }

            RunMenu(_menuDictionary);  
        }

        private static void SendRegistreerPapierSettingPersoonCommand()
        {
            Console.WriteLine("Geef een persoonnummer in:");
            _pernummer = Console.ReadLine();
            Task<ISendEndpoint> sendEndpointTask = _bus.GetSendEndpoint(new Uri(string.Concat(rabbitMqAddress, "/", rabbitMqQueue)));
            ISendEndpoint sendEndpoint = sendEndpointTask.Result;

            Task sendRegistreerPapierSetting = sendEndpoint.Send<RegistreerPapierSettingPersoon>(new RegistreerPapierSettingPersoon(_pernummer, "Euricom", "Demo", 
                new Adres("Schalïenhoevedreef","20","J","2800", "Mechelen")));
        }

        private static void SendZetPapierAanCommand()
        {
            Task<ISendEndpoint> sendEndpointTask = _bus.GetSendEndpoint(new Uri(string.Concat(rabbitMqAddress, "/", rabbitMqQueue)));
            ISendEndpoint sendEndpoint = sendEndpointTask.Result;

            Task sendZetPapierAan = sendEndpoint.Send<ZetPapierUit>(new ZetPapierUit(_pernummer));
        }

        private static void SendZetPapierUitCommand()
        {
            Task<ISendEndpoint> sendEndpointTask = _bus.GetSendEndpoint(new Uri(string.Concat(rabbitMqAddress, "/", rabbitMqQueue)));
            ISendEndpoint sendEndpoint = sendEndpointTask.Result;

            Task sendZetPapierAan = sendEndpoint.Send<ZetPapierAan>(new ZetPapierAan(_pernummer));
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
