using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Opencare.Authorization;
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

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Fetch Contact from DB to get OwnerID.
            //var user = await Context
            //    .ApplicationUsers.AsNoTracking()
            //    .FirstOrDefaultAsync(m => m.Id == id);

            //if (user == null)
            //{
            //    return NotFound();
            //}

            //var isAuthorized = await AuthorizationService.AuthorizeAsync(
            //                                         User, user,
            //                                         StudentOperations.Update);
            //if (!isAuthorized.Succeeded)
            //{
            //    return new ChallengeResult();
            //}

            Context.Attach(AppUser).State = EntityState.Modified;

            try
            {
                await Context.SaveChangesAsync();
            }
            catch
            {
                if (!Context.ApplicationUsers.Any(e => e.Id == AppUser.Id))
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

        private bool StudentExists(int id)
        {
            return Context.Student.Any(e => e.StudentId == id);
        }
    }
}