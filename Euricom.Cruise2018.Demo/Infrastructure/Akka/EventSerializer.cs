using System;
using System.Text;
using Akka.Actor;
using Akka.Serialization;
using Newtonsoft.Json;

namespace Euricom.Cruise2018.Demo.Infrastructure.Akka
{
    public class EventSerializer : Serializer
    {
        private readonly JsonSerializerSettings _serializerSettings;

        public override int Identifier { get { return 800; } }
        public override bool IncludeManifest { get { return true; } }

        public EventSerializer(ExtendedActorSystem system)
            : base(system)
        {
            _serializerSettings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                Formatting = Formatting.None,
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            };
        }

        public override object FromBinary(byte[] bytes, Type type)
        {
            var json = Encoding.UTF8.GetString(bytes);
            var @event = JsonConvert.DeserializeObject(json, type, _serializerSettings);

            return @event;
        }

        public override byte[] ToBinary(object obj)
        {
            var json = JsonConvert.SerializeObject(obj, Formatting.None, _serializerSettings);
            var bytes = Encoding.UTF8.GetBytes(json);

            return bytes;
        }
    }
}
