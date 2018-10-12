using Opencare.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opencare.Models
{
    public class Diaper
    {
        public int id { get; set; }

        public DateTime Time { get; set; }
        public bool Wet { get; set; }
        public bool Dirty { get; set; }
        public string Notes { get; set; }

        public Student Student { get; set; }

        public ApplicationUser Changer { get; set; }
    }
}
