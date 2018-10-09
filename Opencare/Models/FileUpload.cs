using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Opencare.Models
{
    public class FileUpload
    {
        [Required]
        [Display(Name = "Document Type")]
        [StringLength(60, MinimumLength = 3)]
        public string DocumentType { get; set; }

        [Required]
        [Display(Name = "Student Document")]
        public IFormFile UploadStudentDocument { get; set; }
    }
}
