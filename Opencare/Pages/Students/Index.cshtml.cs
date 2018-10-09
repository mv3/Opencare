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

namespace Opencare.Pages.Students
{
    public class IndexModel : DI_BasePageModel
    {
        public IndexModel(ApplicationDbContext context,
        IAuthorizationService authorizationService,
        UserManager<ApplicationUser> userManager)
        : base(context, authorizationService, userManager)
        {
        }

        public IList<Student> Student { get;set; }
        public IList<Student> Children { get; set; }
        public IList<Group> Groups { get; set; }
        public IList<Models.SignIn> SignIns { get; set; }

        public async Task OnGetAsync()
        {
            var currentUserId = UserManager.GetUserId(User);

            var currUser = await UserManager.GetUserAsync(User);

            if (User.IsInRole(Constants.TeachersRole))
            {
                Student = await Context.Student.Where(s => s.Group.TeacherId == currentUserId && s.Deleted == false).ToListAsync();
                Groups = await Context.Group.Where(g => g.TeacherId == currentUserId).ToListAsync();
            }
            else if(User.IsInRole(Constants.AdministratorsRole))
            {
                Student = await Context.Student.Where(s => s.Deleted == false).ToListAsync();
                Groups = await Context.Group.ToListAsync();
            }

            if (User.IsInRole(Constants.ParentsRole))
            {
                Children = await Context.Student
                    .Where(s => s.ParentID == currentUserId && s.Deleted == false)
                    .Include(s => s.Group)
                    .ToListAsync();             
            }
        }

        public async Task OnGetSignIn(int id)
        {
            var student = await Context.Student.Where(s => s.StudentId == id).FirstOrDefaultAsync();
            if (student.IsSignedIn)
            {
                student.IsSignedIn = false;
            }
            else
            {
                student.IsSignedIn = true;
            }


            var signIn = new SignIn { IsSignedIn = student.IsSignedIn, Time = DateTime.Now, Student = student };

            await Context.SignIns.AddAsync(signIn);
            await Context.SaveChangesAsync();

            var currentUserId = UserManager.GetUserId(User);
            Children = await Context.Student
                    .Where(s => s.ParentID == currentUserId && s.Deleted == false)
                    .Include(s => s.Group)
                    .ToListAsync();
        }
    }
}
