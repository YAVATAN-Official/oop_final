using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gis_final.Models
{
    public class StudentConselor:BaseEntity
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int TeacherId { get; set; }
    }
}
