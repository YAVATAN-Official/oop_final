using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace gis_final.Models
{
    public class Tag : BaseEntity
    {
        [Required]
        public string Title { get; set; }

        public virtual ICollection<UserTags> UserTags { get; set; }
    }
}
