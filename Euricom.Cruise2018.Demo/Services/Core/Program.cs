using Euricom.Cruise2018.Demo.Query.PapierSettingPersoon;
using System;
using System.Linq;
using Topshelf;

namespace Euricom.Cruise2018.Demo.Services.Core
{
    class Program
    {
        private static readonly NLog.ILogger Logger = NLog.LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            HostFactory.Run(host =>
            {
                host.Service<CoreService>(_ => new CoreService(), svcCfg =>
                {
                    svcCfg.BeforeStartingService(_ => Logger.Warn("Starting service.."));
                    svcCfg.AfterStartingService(_ => Logger.Warn("Service started!"));
                    svcCfg.BeforeStoppingService(_ => Logger.Warn("Stopping service.."));
                    svcCfg.AfterStoppingService(_ => Logger.Warn("Service stopped!"));
                });

                host.RunAsLocalSystem();
                host.StartAutomatically();

                host.SetServiceName("Euricom.Cruise2018.Demo.Services.Core");
                host.SetDisplayName("Euricom Cruise 2018 Core Service");
                host.SetDescription("Euricom Cruise 2018 Core Service");
                host.UseNLog();
            });

#if DEBUG
            Console.WriteLine();
            Console.WriteLine("Press the <any> key to exit..");
            Console.ReadLine();
#endif
        }
    }
}
