using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euricom.Cruise2018.Demo.Query.PapierSettingPersoon
{
    [Table("PapierSettingPersoon", Schema = "RM")]
    public class PapierSettingPersoon
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Index("CIX_RM_PapierSettingsPersoon", IsClustered = true, IsUnique = true)]
        [Column(Order = 0)]
        public int Id { get; set; }

        [Key]
        public string PapierSettingPersoonId { get; set; }

        public long Version { get; set; }

        [Required]
        [StringLength(11)]
        public string PerNummer { get; set; }

        [Required]
        [StringLength(50)]
        public string Naam { get; set; }

        [Required]
        [StringLength(50)]
        public string Voornaam { get; set; }

        public bool? PapierAan { get; set; }

        public bool IsActief { get; set; }

        public Adres Adres { get; set; }
    }
}
