using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Opencare.Models
{
    public class StudentDocument
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Document Type")]
        public string DocumentType { get; set; }


        //[Display(Name = "Document Size (bytes)")]
        //[DisplayFormat(DataFormatString = "{0:N1}")]
        //public long DocumentSize { get; set; }

        [Display(Name = "Uploaded (UTC)")]
        [DisplayFormat(DataFormatString = "{0:F}")]
        public DateTime UploadDT { get; set; }

        [Required]
        public IFormFile Document { get; set; }
    }
}
