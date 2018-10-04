using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Opencare.Data;
using Opencare.Pages.Students;

namespace Opencare.Pages.Users
{
    public class CreateModel : DI_BasePageModel
    {
        //private readonly Opencare.Data.ApplicationDbContext _context;

        public CreateModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<ApplicationUser> userManager)
            : base(context, authorizationService, userManager)
        {

        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "First name")]
            public string FirstName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Last name")]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Is Teacher")]
            public bool IsTeacher { get; set; }

            [Display(Name = "Is Administrator")]
            public bool IsAdmin { get; set; }

            [Display(Name = "Is Parent")]
            public bool IsParent { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [RegularExpression(@"^(\d{4})$", ErrorMessage = "Enter a valid 4 digit PIN")]
            [Display(Name = "PIN")]
            public string PIN { get; set; }
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    PIN = Input.PIN
                };
                var result = await UserManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    // Add user roles
                    user = await UserManager.FindByEmailAsync(Input.Email);

                    if (Input.IsTeacher)
                    {
                        if (!await UserManager.IsInRoleAsync(user, "Teachers"))
                        {
                            await UserManager.AddToRoleAsync(user, "Teachers");
                        }
                    }
                    else
                    {
                        if (await UserManager.IsInRoleAsync(user, "Teachers"))
                        {
                            await UserManager.RemoveFromRoleAsync(user, "Teachers");
                        }
                    }

                    if (Input.IsAdmin)
                    {
                        if (!await UserManager.IsInRoleAsync(user, "Administrators"))
                        {
                            await UserManager.AddToRoleAsync(user, "Administrators");
                        }
                    }
                    else
                    {
                        if (await UserManager.IsInRoleAsync(user, "Administrators"))
                        {
                            await UserManager.RemoveFromRoleAsync(user, "Administrators");
                        }
                    }

                    if (Input.IsParent)
                    {
                        if (!await UserManager.IsInRoleAsync(user, "Parents"))
                        {
                            await UserManager.AddToRoleAsync(user, "Parents");
                        }
                    }
                    else
                    {
                        if (await UserManager.IsInRoleAsync(user, "Parents"))
                        {
                            await UserManager.RemoveFromRoleAsync(user, "Parents");
                        }
                    }



                    return RedirectToPage("./Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}