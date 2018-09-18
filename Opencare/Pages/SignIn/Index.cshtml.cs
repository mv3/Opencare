using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Opencare.Data;
using Opencare.Models;

namespace Opencare.Pages.SignIn
{
    public class IndexModel : PageModel
    {
        protected ApplicationDbContext Context { get; }
        protected IAuthorizationService AuthorizationService { get; }
        protected UserManager<ApplicationUser> UserManager { get; }

        public IndexModel(
           ApplicationDbContext context,
           IAuthorizationService authorizationService,
           UserManager<ApplicationUser> userManager) : base()
        {
            Context = context;
            UserManager = userManager;
            AuthorizationService = authorizationService;
        }

        public IList<Student> Students { get; set; }
        public ApplicationUser AppUser { get; set; }        

        public async Task<IActionResult> OnGetAsync()
        {
            AppUser = await UserManager.FindByIdAsync(id);

            Students = await Context.Student.Where(s => s.ParentID == id).ToListAsync();

            return Page();
        }        
    }
}