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

namespace Opencare.Pages.SignIns
{
    public class EditModel : PageModel
    {
        private readonly Opencare.Data.ApplicationDbContext _context;

        public EditModel(Opencare.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SignIn SignIn { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SignIn = await _context.SignIns.FirstOrDefaultAsync(m => m.SignInId == id);

            if (SignIn == null)
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

            _context.Attach(SignIn).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SignInExists(SignIn.SignInId))
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

        private bool SignInExists(int id)
        {
            return _context.SignIns.Any(e => e.SignInId == id);
        }
    }
}
