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
using Opencare.Pages.Students;

namespace Opencare.Pages.Users
{
    public class DetailsModel : DI_BasePageModel
    {
        public DetailsModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<ApplicationUser> userManager)
            : base(context, authorizationService, userManager)
        {

        }

        public ApplicationUser AppUser { get; set; }

        public List<Student> Students { get; set; }

        public List<Student> Children { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            AppUser = await UserManager.FindByIdAsync(id);
            if (AppUser == null)
            {
                return NotFound($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }
            else
            {
                Students = await Context.Student.Where(s => s.TeacherId == AppUser.Id).ToListAsync();
                Children = await Context.Student.Where(s => s.ParentID == AppUser.Id).ToListAsync();
            }

            return Page();
        }
    }
}