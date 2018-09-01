
using System;

namespace BM2.RecipientData.NAVOUT.SharedKernel.ValueObjects
{
    public class Adres : IEquatable<Adres>
    {
        public string Straat { get; private set; }
        public string Nummer { get; private set; }
        public string Bus { get; private set; }
        public string Postcode { get; private set; }
        public string Gemeente { get; private set; }

        public Adres(string straat, string nummer, string bus,
                     string postcode, string gemeente)
        {
            Straat = straat;
            Nummer = nummer;
            Bus = bus;
            Postcode = postcode;
            Gemeente = gemeente;
        }

        public bool Equals(Adres other)
        {
            if (other == null)
                return false;

            return string.Equals(Straat, other.Straat) && string.Equals(Nummer, other.Nummer) && string.Equals(Bus, other.Bus) &&
                   string.Equals(Postcode, other.Postcode) && string.Equals(Gemeente, other.Gemeente);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var adres = obj as Adres;

            if (adres == null)
                return false;

            return Equals(adres);
        }

        public override int GetHashCode()
        {
            var code = string.Concat(Straat, Nummer, Bus, Postcode, Gemeente);

            return code.GetHashCode();
        }
    }
}
