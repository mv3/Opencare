using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Opencare.Data;

namespace Opencare.Pages.PINSignIn
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

        public IList<ApplicationUser> Parents { get; set; }
        public ApplicationUser SignInUser { get; set; }

        public async Task OnGetAsync(string search = null)
        {
            Parents = await UserManager.GetUsersInRoleAsync("Parents");

            if (search != null)
            {
                Parents = Parents.Where(u => u.FirstName.ToLower().Contains(search.ToLower())
                    || u.LastName.ToLower().Contains(search.ToLower())).ToList();
            }
        }

        
    }
}