using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace gis_final.Models
{
    public enum EnumCourseType
    {
        [Display(Name = "Master")]
        YL = 0,
        [Display(Name = "Ph.D.")]
        DR = 1,
        [Display(Name = "Master & Ph.D.")]
        YL_DR = 2
    }

    public class Course : BaseEntity
    {
        public string Title { get; set; }
        public EnumCourseType CourseTypeId { get; set; }

        public virtual ICollection<FieldCourses> FieldCourses { get; set; }
    }
}
