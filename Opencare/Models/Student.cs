using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Opencare.Data;

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

        [DataType(DataType.Date)]
        public DateTime BeginCareDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndCareDate { get; set; }

        public string Address { get; set; }

        public string PhysicianName { get; set; }
        public string PhysicianPhone { get; set; }

        public bool FieldTripAuthorized { get; set; }

        public EnrollmentStatus Status { get; set; }

        // User ID From AspNetUsers table.
        public string ParentID { get; set; }
        public ApplicationUser Parent { get; set; }

        [Display(Name = "Group")]
        public int GroupId { get; set; }

        public Group Group { get; set; }

        [Display(Name = "Signed In")]
        public bool IsSignedIn { get; set; }
                
        public ICollection<SignIn> SignIns { get; set; }

        public ICollection<StudentDocument> Documents { get; set; }

        public bool Deleted { get; set; }
    }

    public enum EnrollmentStatus
    {
        Enrolled,
        Pending,
        NotEnrolled
    }
}
