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
using Opencare.Authorization;
using Opencare.Data;
using Opencare.Pages.Students;

namespace Opencare.Pages.Users
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


        [TempData]
        public string StatusMessage { get; set; }

        public string Username { get; set; }

        public ApplicationUser AppUser { get; set; }

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

            [Required]
            [RegularExpression(@"^(\d{4})$", ErrorMessage = "Enter a valid 4 digit PIN")]
            [Display(Name = "PIN")]
            public string PIN { get; set; }

            [Display(Name = "Is Teacher")]
            public bool IsTeacher { get; set; }
            
            [Display(Name = "Is Administrator")]
            public bool IsAdmin { get; set; }

            [Display(Name = "Is Parent")]
            public bool IsParent { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            var userName = await UserManager.GetUserNameAsync(user);
            var email = await UserManager.GetEmailAsync(user);
            var phoneNumber = await UserManager.GetPhoneNumberAsync(user);
            var isTeacher = await UserManager.IsInRoleAsync(user, "Teachers");
            var isAdmin = await UserManager.IsInRoleAsync(user, "Administrators");
            var isParent = await UserManager.IsInRoleAsync(user, "Parents");

            Username = userName;

            Input = new InputModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = email,
                PhoneNumber =  phoneNumber,
                IsTeacher = isTeacher,
                IsAdmin = isAdmin,
                IsParent = isParent,
                PIN = user.PIN              
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            if (Input.FirstName != user.FirstName)
            {
                user.FirstName = Input.FirstName;
            }

            if (Input.LastName != user.LastName)
            {
                user.LastName = Input.LastName;
            }

            var email = await UserManager.GetEmailAsync(user);
            if (Input.Email != email)
            {
                var setEmailResult = await UserManager.SetEmailAsync(user, Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    var userId = await UserManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{userId}'.");
                }
            }

            var phoneNumber = await UserManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await UserManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await UserManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }

            if (Input.PIN != user.PIN)
            {
                user.PIN = Input.PIN;
            }

            await UserManager.UpdateAsync(user);

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
                    await UserManager.AddToRoleAsync(user, "Teachers");
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

            StatusMessage = "Your profile has been updated";
            return RedirectToPage("./Index");
        }
    }
}