using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Opencare.Data;
using Opencare.Models;

namespace Opencare.Pages.Groups
{
    public class DetailsModel : PageModel
    {
        private readonly Opencare.Data.ApplicationDbContext _context;

        public DetailsModel(Opencare.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Group Group { get; set; }

        public List<Student> Students { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Group = await _context.Group.FirstOrDefaultAsync(m => m.Id == id);

            if (Group == null)
            {
                return NotFound();
            }

            Students = await _context.Student.Where(s => s.GroupId == id).ToListAsync();

            return Page();
        }
    }
}
