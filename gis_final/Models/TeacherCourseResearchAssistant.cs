using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gis_final.Models
{
    public class TeacherCourseResearchAssistant : BaseEntity
    {
        // USERID
        public int ResearchAssistantId { get; set; }
        public int TeacherFieldCourseId{ get; set; }
        public virtual TeacherFieldCourse TeacherFieldCourse { get; set; }
    }
}
