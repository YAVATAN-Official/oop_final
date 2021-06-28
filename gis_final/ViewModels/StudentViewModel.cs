using gis_final.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gis_final.ViewModels
{
    public class StudentViewModel
    {
        public User User { get; set; }
        public string TagName { get; set; }
        public Role Roles { get; set; }
    }
}
