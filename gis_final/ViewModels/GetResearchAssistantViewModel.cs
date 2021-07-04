using gis_final.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gis_final.ViewModels
{
    public class GetResearchAssistantViewModel
    {
        public User Assistant { get; set; }
        public User Teacher { get; set; }
        public Field Field { get; set; }
        public Course Course { get; set; }
        public TeacherFieldCourseResearchAssistant TeacherFieldCourseResearchAssistant { get; set; }

    }
}
