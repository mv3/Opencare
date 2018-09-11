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

        public async Task OnGetAsync()
        {
            var students = from c in Context.Student
                           select c;

            var currentUserId = UserManager.GetUserId(User);

            if (User.IsInRole("Teachers"))
            {
                students = students.Where(s => s.Group.TeacherId == currentUserId);
            }
            else if(User.IsInRole("Parents"))
            {
                students = students.Where(c => c.ParentID == currentUserId);
            }

            Student = await students.ToListAsync();
        }
    }
}
