using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Opencare.Data;
using Opencare.Models;

namespace Opencare.Pages.SignIn
{
    public class WelcomeModel : PageModel
    {
        protected ApplicationDbContext Context { get; }
        protected IAuthorizationService AuthorizationService { get; }
        protected UserManager<ApplicationUser> UserManager { get; }

        public WelcomeModel(
           ApplicationDbContext context,
           IAuthorizationService authorizationService,
           UserManager<ApplicationUser> userManager) : base()
        {
            Context = context;
            UserManager = userManager;
            AuthorizationService = authorizationService;
        }

        public IList<Student> Students { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Students = await Context.Student.Where(s => s.ParentID == id).ToListAsync();

            return Page();
        }
    }
}