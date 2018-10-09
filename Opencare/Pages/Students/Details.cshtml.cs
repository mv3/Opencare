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
using Opencare.Models;
using System.ComponentModel.DataAnnotations;

namespace Opencare.Pages.Students
{
    public class DetailsModel : DI_BasePageModel
    {
        public DetailsModel(ApplicationDbContext context,
        IAuthorizationService authorizationService,
        UserManager<ApplicationUser> userManager)
        : base(context, authorizationService, userManager)
        {
        }

        public Student Student { get; set; }

        [Display(Name = "Parent")]
        public string ParentName { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {

            Student = await Context.Student.Include(s=>s.Parent).FirstOrDefaultAsync(m => m.StudentId == id);

            if (Student == null)
            {
                return NotFound();
            }

            ParentName = Student.Parent.FirstName + " " + Student.Parent.LastName;

            var isAuthorized = User.IsInRole(Constants.TeachersRole) ||
                           User.IsInRole(Constants.AdministratorsRole);

            var currentUserId = UserManager.GetUserId(User);

            if (!isAuthorized
                && currentUserId != Student.ParentID
                && Student.Status != EnrollmentStatus.Enrolled)
            {
                return new ChallengeResult();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, EnrollmentStatus status)
        {
            var student = await Context.Student.FirstOrDefaultAsync(
                                                      m => m.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            var contactOperation = (status == EnrollmentStatus.Enrolled)
                                                       ? StudentOperations.Approve
                                                       : StudentOperations.Reject;

            var isAuthorized = await AuthorizationService.AuthorizeAsync(User, student,
                                        contactOperation);
            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }
            student.Status = status;
            Context.Student.Update(student);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
