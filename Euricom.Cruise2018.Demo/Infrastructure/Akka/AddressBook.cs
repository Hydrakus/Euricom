using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Akka.Actor;

namespace Euricom.Cruise2018.Demo.Infrastructure.Akka
{
    public class AddressBook : ReceiveActor
    {
        #region messages
        public class Register
        {
            public readonly string Name;
            public readonly IActorRef Ref;

            public Register(string name, IActorRef @ref)
            {
                Name = name;
                Ref = @ref;
            }
        }

        public class UnRegister
        {
            public readonly string Name;

            public UnRegister(string name)
            {
                Name = name;
            }
        }

        public class Get
        {
            public readonly string Name;

            public Get(string name)
            {
                Name = name;
            }
        }

        public class Found
        {
            public readonly string Name;
            public readonly IActorRef Ref;

            public Found(string name, IActorRef @ref)
            {
                Name = name;
                Ref = @ref;
            }
        }

        public class NotFound
        {
            public readonly string Name;

            public NotFound(string name)
            {
                Name = name;
            }
        }
        #endregion

        public const string Name = "address-book";
        private readonly IDictionary<string, IActorRef> _addresses;

        public AddressBook(IEnumerable<KeyValuePair<string, IActorRef>> entries = null)
        {
            _addresses = new ConcurrentDictionary<string, IActorRef>(entries ?? Enumerable.Empty<KeyValuePair<string, IActorRef>>());

            Receive<Register>(reg => _addresses.Add(reg.Name, reg.Ref));
            Receive<UnRegister>(uReg => _addresses.Remove(uReg.Name));
            Receive<Get>(get =>
            {
                IActorRef path;
                var reply = _addresses.TryGetValue(get.Name, out path)
                    ? (object)new Found(get.Name, path)
                    : new NotFound(get.Name);

                Sender.Tell(reply);
            });
        }
    }

    public static class AddressBookExtensions
    {
        public static ICanTell GetAddressBook(this ActorSystem system)
        {
            return system.ActorSelection("/user/" + AddressBook.Name);
        }

        public static IActorRef GetActorFromAddressBook(this ActorSystem system, string actorName)
        {
            return system.GetAddressBook().Ask<AddressBook.Found>(new AddressBook.Get(actorName)).Result.Ref;
        }
    }
}
