using System;
using System.Collections.Generic;
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
    public class IndexModel : DI_BasePageModel
    {
        public IndexModel(ApplicationDbContext context,
        IAuthorizationService authorizationService,
        UserManager<ApplicationUser> userManager)
        : base(context, authorizationService, userManager)
        {
        }

        public IList<ApplicationUser> Users { get; set; }

        public async Task OnGetAsync()
        {
            var users = Context.ApplicationUsers;
            Users = users.ToList();
        }
    }
}