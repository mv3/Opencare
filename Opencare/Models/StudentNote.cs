using Opencare.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Opencare.Models
{
    public class StudentNote
    {
        public int Id { get; set; }
        public string Note { get; set; }

        [Display(Name = "User")]
        public ApplicationUser AppUser { get; set; }
        public Student Student { get; set; }
        public DateTime Date { get; set; }
    }
}
