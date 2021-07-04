using gis_final.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gis_final.ViewModels
{
    public class CourseStudentsViewModel
    {
        public User Student { get; set; }
        public Field Field { get; set; }
        public Course Course { get; set; }
        public string Year { get; set; }
        public EnumPeriod Term { get; set; }
    }
}
