using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gis_final.Models
{
    public class TeacherField
    {
        //TeacherId
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int FieldId { get; set; }
        public virtual Field Field { get; set; }
    }
}
