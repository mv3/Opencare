using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Opencare.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }

        public EnrollmentStatus Status { get; set; }

        // User ID From AspNetUsers table.
        public string ParentID { get; set; }

        //[Display(Name = "Teacher")]
        //public string TeacherId { get; set; }

        [Display(Name = "Group")]
        public int GroupId { get; set; }

        public Group Group { get; set; }

        public bool IsSignedIn { get; set; }
        
        public ICollection<SignIn> SignIns { get; set; }
    }

    public enum EnrollmentStatus
    {
        Enrolled,
        Pending,
        NotEnrolled
    }
}
