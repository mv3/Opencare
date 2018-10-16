using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Opencare.Data;
using Opencare.Models;

namespace Opencare.Pages.Diapers
{
    public class EditModel : PageModel
    {
        private readonly Opencare.Data.ApplicationDbContext _context;

        public EditModel(Opencare.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Diaper Diaper { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Diaper = await _context.Diapers.FirstOrDefaultAsync(m => m.id == id);

            if (Diaper == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Diaper).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DiaperExists(Diaper.id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool DiaperExists(int id)
        {
            return _context.Diapers.Any(e => e.id == id);
        }
    }
}
