using Opencare.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Opencare.Models
{
    public class Notification
    {
        public int Id { get; set; }

        [Display(Name = "User")]
        public ApplicationUser AppUser { get; set; }
        public ApplicationUser PostingAppUser { get; set; }

        public string Details { get; set; }

        public DateTime Date { get; set; }
        
    }
}
