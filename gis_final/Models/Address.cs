using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace gis_final.Models
{
    public class Address : BaseEntity
    {
        [Required]
        public string Title { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        [Required]
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        [Required, MaxLength(12)]
        public int PostalCode { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
