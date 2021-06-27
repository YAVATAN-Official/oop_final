using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace gis_final.Models
{
    public enum EnumDegree
    {
        [Display(Name = "Master With Thesis")]
        MasterWithThesis = 0,
        [Display(Name = "Master Without Thesis")]
        MasterWithoutThesis = 1
    }

    public class StudentDegree : BaseEntity
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public EnumDegree DegreeId { get; set; }
    }
}
