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

namespace Opencare.Pages.Bottles
{
    public class EditModel : PageModel
    {
        private readonly Opencare.Data.ApplicationDbContext _context;

        public EditModel(Opencare.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Bottle Bottle { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Bottle = await _context.Bottles.FirstOrDefaultAsync(m => m.id == id);

            if (Bottle == null)
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

            _context.Attach(Bottle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BottleExists(Bottle.id))
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

        private bool BottleExists(int id)
        {
            return _context.Bottles.Any(e => e.id == id);
        }
    }
}
