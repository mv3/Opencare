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
using Opencare.Models;

namespace Opencare.Pages.PINSignIn
{
    public class WelcomeModel : PageModel
    {
        protected ApplicationDbContext Context { get; }
        protected IAuthorizationService AuthorizationService { get; }
        protected UserManager<ApplicationUser> UserManager { get; }

        public WelcomeModel(
           ApplicationDbContext context,
           IAuthorizationService authorizationService,
           UserManager<ApplicationUser> userManager) : base()
        {
            Context = context;
            UserManager = userManager;
            AuthorizationService = authorizationService;
        }

        public IList<Student> Students { get; set; }
        public ApplicationUser SignInUser { get; set; }

        [BindProperty]
        [Required]
        [RegularExpression(@"^(\d{4})$", ErrorMessage = "Enter a valid 4 digit PIN")]
        [DataType(DataType.Password)]
        [Display(Name = "PIN")]
        public string UserPIN { get; set; }

        public bool isSignedIn { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            SignInUser = await UserManager.FindByIdAsync(id);

            Students = await Context.Student.Where(s => s.ParentID == id).ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            Students = await Context.Student.Where(s => s.ParentID == id).ToListAsync();
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            if (UserPIN != user.PIN)
            {
                return Page();
            }

            isSignedIn = true;
            return Page();
        }

    }
}