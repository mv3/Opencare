using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Opencare.Data;
using Opencare.Models;

namespace Opencare.Pages.SignIns
{
    public class DetailsModel : PageModel
    {
        private readonly Opencare.Data.ApplicationDbContext _context;

        public DetailsModel(Opencare.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
