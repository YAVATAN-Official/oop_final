using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gis_final.Models
{
    public enum EnumStudentGraduationStatus
    {
        Graduate = 0,
        Undergraduate = 1
    }

    public class StudentGraduationStatus : BaseEntity
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public EnumStudentGraduationStatus GraduationStatusId { get; set; }

    }
}
