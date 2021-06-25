using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gis_final.Models
{
    public class Faculty : BaseEntity
    {
        public string Title { get; set; }

        public virtual ICollection<Field> Fields { get; set; }
    }
}
