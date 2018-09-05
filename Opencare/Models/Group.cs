using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Opencare.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Room { get; set; }

        [Display(Name = "Minimum Age")]
        public int MinAge { get; set; }
        [Display(Name = "Maximum Age")]
        public int MaxAge { get; set; }

        [Display(Name = "Teacher")]
        public string TeacherId { get; set; }
    }
}
