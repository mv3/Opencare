using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Opencare.Data;

namespace Opencare.Models
{
    public class StudentDocument
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Document Type")]
        public string DocumentType { get; set; }
        
        [Display(Name = "Uploaded (UTC)")]
        [DisplayFormat(DataFormatString = "{0:F}")]
        public DateTime UploadDT { get; set; }

        [Required]
        public byte[] Document { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; }

        public ApplicationUser UploadUser { get; set; }

        public Student Student { get; set; }
    }
}
