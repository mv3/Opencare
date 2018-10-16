using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Opencare.Data;
using Opencare.Models;

namespace Opencare.Pages.Bottles
{
    public class DetailsModel : PageModel
    {
        private readonly Opencare.Data.ApplicationDbContext _context;

        public DetailsModel(Opencare.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
