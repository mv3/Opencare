﻿using System;
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
        
        public List<Student> Children { get; set; }
        public List<Student> Students { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            AppUser = await UserManager.FindByNameAsync(id);
            if (AppUser == null)
            {
                return NotFound($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }
            else
            {
                Children = await Context.Student.Where(s => s.ParentID == AppUser.Id).ToListAsync();
                Students = await Context.Student.Where(s => s.Group.TeacherId == AppUser.Id).ToListAsync();
            }

            return Page();
        }
    }
}