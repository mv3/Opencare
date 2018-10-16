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
    public class IndexModel : PageModel
    {
        private readonly Opencare.Data.ApplicationDbContext _context;

        public IndexModel(Opencare.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Bottle> Bottle { get;set; }

        public async Task OnGetAsync(int? id)
        {
            Bottle = await _context.Bottles
                .Where(d => d.Student.StudentId == id)
                .ToListAsync();
        }
    }
}
