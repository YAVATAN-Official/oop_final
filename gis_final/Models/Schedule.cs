using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gis_final.Models
{
    public class Schedule : BaseEntity
    {
        public enum EnumScoreStatus
        {
            Passed = 0,
            Refused = 1
        }

        public int TeacherFieldCourseId { get; set; }
        public virtual TeacherFieldCourse TeacherFieldCourse { get; set; }
        // StudentId
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int YearTermId { get; set; }
        public virtual YearTerm YearTerm { get; set; }
        public decimal Score { get; set; }
        public EnumScoreStatus EnumScoreStatusId { get; set; }
    }
}
