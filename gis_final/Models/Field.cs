using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gis_final.Models
{
    public class Field : BaseEntity
    {
        public string Title { get; set; }

        public int FacultyId { get; set; }
        public virtual Faculty Faculty { get; set; }

        public virtual ICollection<FieldCourses> FieldCourses { get; set; }
        public virtual ICollection<TeacherFieldCourse> TeacherFieldCourses { get; set; }
    }
}
