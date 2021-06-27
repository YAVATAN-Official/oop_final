using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gis_final.Models
{
    public class UserTags : BaseEntity
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int TagId { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
