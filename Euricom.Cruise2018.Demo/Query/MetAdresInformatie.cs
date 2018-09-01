using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Euricom.Cruise2018.Demo.Query
{
    [ComplexType]
    public class Adres
    { 
        [Required]
        [StringLength(50)]
        public string Straat { get; set; }
  
        [Required]
        [StringLength(10)]
        public string Nummer { get; set; }

        [StringLength(10)]
        public string Bus { get; set; }

        [Required]
        [StringLength(10)]
        public string Postcode { get; set; }

        [Required]
        [StringLength(50)]
        public string Gemeente { get; set; }
    }
}
