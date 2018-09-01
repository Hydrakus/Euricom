using System.Configuration;
using Akka.Configuration;
using Akka.Configuration.Hocon;

namespace Euricom.Cruise2018.Demo.Infrastructure.Akka
{
    public class ConfigurationHelper
    {
        public static Config GetAkkaConfigurationSettings()
        {
            var configuration = GetAkkaConfig("akka/akka.actor");
            configuration = configuration
              .WithFallback(GetAkkaConfig("akka/akka.logging"))
              .WithFallback(GetAkkaConfig("akka/akka.persistence"));

            return configuration;
        }

        private static Config GetAkkaConfig(string sectionName)
        {
            return ((AkkaConfigurationSection)ConfigurationManager.GetSection(sectionName)).AkkaConfig;
        }
    }
}
