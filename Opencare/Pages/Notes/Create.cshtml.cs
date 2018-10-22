using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Opencare.Data;
using Opencare.Models;

namespace Opencare.Pages.Notes
{
    public class CreateModel : PageModel
    {
        private readonly Opencare.Data.ApplicationDbContext _context;

        public CreateModel(Opencare.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public StudentNote StudentNote { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.StudentNote.Add(StudentNote);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}