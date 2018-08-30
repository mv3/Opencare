using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Opencare.Data;
using Opencare.Pages.Students;

namespace Opencare.Pages.Users
{
    public class EditModel : DI_BasePageModel
    {
        public EditModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<ApplicationUser> userManager)
            : base(context, authorizationService, userManager)
        {

        }

        [BindProperty]
        public ApplicationUser AppUser { get; set; }

        public IActionResult OnGet(string id)
        {
            AppUser = Context.ApplicationUsers.FirstOrDefault(
                                             m => m.Id == id);

            return Page();
        }
    }
}