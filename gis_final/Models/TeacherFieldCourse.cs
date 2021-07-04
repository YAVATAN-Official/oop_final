using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gis_final.Models
{
    public enum EnumDays
    {
        Sunday = 0,
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6
    }

    public enum EnumStatus
    {
        Active = 0,
        Passive = 1
    }

    public class TeacherFieldCourse : BaseEntity
    {
        //Teacher
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int FieldCoursesId { get; set; }
        public virtual FieldCourses FieldCourses { get; set; }
        public string Time { get; set; }
        public EnumDays DayId { get; set; }
        public EnumStatus StatusId { get; set; }

        public virtual ICollection<Schedule> Schedules { get; set; }
        public virtual ICollection<TeacherFieldCourseResearchAssistant> TeacherFieldCourseResearchAssistants { get; set; }
    }
}
