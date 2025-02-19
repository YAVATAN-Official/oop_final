﻿using System.Collections.Generic;

namespace gis_final.Models
{
    public class FieldCourses : BaseEntity
    {
        public int FieldId { get; set; }
        public virtual Field Field { get; set; }
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

        public virtual ICollection<TeacherFieldCourse> TeacherFieldCourses { get; set; }
    }
}
