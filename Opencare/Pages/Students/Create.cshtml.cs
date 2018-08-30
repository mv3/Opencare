using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Opencare.Authorization;
using Opencare.Data;
using Opencare.Models;

namespace Opencare.Pages.Students
{
    public class CreateModel : DI_BasePageModel
    {
        private readonly Opencare.Data.ApplicationDbContext _context;

        public CreateModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<ApplicationUser> userManager) 
            : base(context, authorizationService, userManager)
        {
            
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Student Student { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Student.ParentID = UserManager.GetUserId(User);
            Student.Status = EnrollmentStatus.Pending;

            // requires using ContactManager.Authorization;
            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                        User, Student,
                                                        StudentOperations.Create);
            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }

            Context.Student.Add(Student);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}