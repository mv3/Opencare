using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opencare.Models
{
    public class SignIn
    {
        public int SignInId { get; set; }

        public bool IsSignedIn { get; set; }

        public DateTime Time { get; set; }

        public Student Student { get; set; }
    }
}
