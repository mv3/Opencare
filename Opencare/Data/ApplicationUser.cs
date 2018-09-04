using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Opencare.Data
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [PersonalData]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
    }
}
