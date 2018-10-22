using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Opencare.Data;
using Opencare.Models;

namespace Opencare.Pages.Notes
{
    public class DetailsModel : PageModel
    {
        private readonly Opencare.Data.ApplicationDbContext _context;

        public DetailsModel(Opencare.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public StudentNote StudentNote { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            StudentNote = await _context.StudentNote.FirstOrDefaultAsync(m => m.Id == id);

            if (StudentNote == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
