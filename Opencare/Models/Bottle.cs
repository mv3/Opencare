using Opencare.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Opencare.Models
{
    public class Bottle
    {
        public int id { get; set; }
        public string Type { get; set; }
        public double Ounces { get; set; }
        public DateTime Time { get; set; }
        public string Note { get; set; }

        public ApplicationUser Teacher { get; set; }
        public Student Student { get; set; }
    }
}
