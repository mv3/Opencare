using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Opencare.Authorization;
using Opencare.Data;
using Opencare.Models;

namespace Opencare.Pages.Students
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
        public Student Student { get; set; }

        public IEnumerable<SelectListItem> Teachers { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var teachersList = await UserManager.GetUsersInRoleAsync("Teachers");

            Teachers = teachersList.Select(x =>
                        new SelectListItem()
                        {
                            Text = x.FirstName + " " + x.LastName,
                            Value = x.Id
                        });

            Student = await Context.Student.FirstOrDefaultAsync(
                                             m => m.StudentId == id);

            if (Student == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                  User, Student,
                                                  StudentOperations.Update);
            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Fetch Contact from DB to get OwnerID.
            var student = await Context
                .Student.AsNoTracking()
                .FirstOrDefaultAsync(m => m.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                     User, student,
                                                     StudentOperations.Update);
            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }

            Student.ParentID = student.ParentID;

            Context.Attach(Student).State = EntityState.Modified;

            if (student.Status == EnrollmentStatus.Enrolled)
            {
                // If the contact is updated after approval, 
                // and the user cannot approve,
                // set the status back to submitted so the update can be
                // checked and approved.
                var canApprove = await AuthorizationService.AuthorizeAsync(User,
                                        student,
                                        StudentOperations.Approve);

                if (!canApprove.Succeeded)
                {
                    student.Status = EnrollmentStatus.Pending;
                }
            }

            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private bool StudentExists(int id)
        {
            return Context.Student.Any(e => e.StudentId == id);
        }
    }
}
