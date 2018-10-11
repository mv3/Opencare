using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Opencare.Models
{
    public class DocumentType
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
