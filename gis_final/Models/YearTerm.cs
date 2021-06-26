using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gis_final.Models
{
    public class YearTerm : BaseEntity
    {
        public enum EnumPeriod
        {
            Winter = 0,
            Spring = 1
        }

        // e. 2020-2021
        public string Year { get; set; }
        public EnumPeriod TermId { get; set; }
    }
}
