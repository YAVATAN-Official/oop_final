﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace gis_final.Models
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
