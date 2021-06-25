using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace gis_final.Models
{
    public class Role : BaseEntity
    {
        [Required]
        public string Title { get; set; }
        public bool View { get; set; }
        public bool Create { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
        public bool Confirm { get; set; }

        public virtual ICollection<UserRoles> UserRoles { get; set; }
    }
}
