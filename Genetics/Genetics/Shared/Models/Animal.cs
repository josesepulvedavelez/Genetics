using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetics.Shared.Models
{
    [Table("Animal")]
    public class Animal
    {
        [Key]
        [Required]
        public int AnimalId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Breed { get; set; }

        public DateTime BirthDate { get; set; }

        [StringLength(6)]
        public string Sex { get; set; }

        public decimal Price { get; set; }

        public Boolean Status { get; set; }
    }
}
