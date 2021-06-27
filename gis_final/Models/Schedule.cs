using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace gis_final.Models
{
    public enum EnumScoreStatus
    {
        Passed = 0,
        Refused = 1
    }

    public class Schedule : BaseEntity
    {
        public int TeacherFieldCourseId { get; set; }
        public virtual TeacherFieldCourse TeacherFieldCourse { get; set; }
        // StudentId
        public int StudentId { get; set; }
        public int YearTermId { get; set; }
        public virtual YearTerm YearTerm { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Score { get; set; }
        public EnumScoreStatus EnumScoreStatusId { get; set; }
    }
}
