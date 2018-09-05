using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Opencare.Data;
using Opencare.Models;

namespace Opencare.Pages.Groups
{
    public class CreateModel : PageModel
    {
        private readonly Opencare.Data.ApplicationDbContext _context;
        private UserManager<ApplicationUser> UserManager { get; }

        public CreateModel(Opencare.Data.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            UserManager = userManager;
        }
        
        [BindProperty]
        public Group Group { get; set; }

        public IEnumerable<SelectListItem> Teachers { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var teacherList = await UserManager.GetUsersInRoleAsync("Teachers");

            Teachers = teacherList.Select(x =>
                        new SelectListItem()
                        {
                            Text = x.FirstName + " " + x.LastName,
                            Value = x.Id
                        });

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Group.Add(Group);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}