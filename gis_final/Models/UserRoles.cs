using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gis_final.Models
{
    public class UserRoles
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        public bool View { get; set; }
        public bool Create { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
        public bool Confirm { get; set; }
    }
}
