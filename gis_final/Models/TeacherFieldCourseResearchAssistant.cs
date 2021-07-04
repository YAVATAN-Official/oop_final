using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gis_final.Models
{
    public class TeacherFieldCourseResearchAssistant : BaseEntity
    {
        // Assistant
        public int AssistantId { get; set; }
        public int TeacherFieldCourseId{ get; set; }
        public virtual TeacherFieldCourse TeacherFieldCourse { get; set; }
    }
}
