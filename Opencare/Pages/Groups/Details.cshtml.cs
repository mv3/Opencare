using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Opencare.Data;
using Opencare.Models;

namespace Opencare.Pages.Groups
{
    public class DetailsModel : PageModel
    {
        private readonly Opencare.Data.ApplicationDbContext _context;

        private UserManager<ApplicationUser> UserManager { get; }

        public DetailsModel(Opencare.Data.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            UserManager = userManager;
        }

        public Group Group { get; set; }

        public List<Student> Students { get; set; }

        public ApplicationUser Teacher { get; set; }

        [Display(Name = "Teacher")]
        public string TeacherName { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Group = await _context.Group.FirstOrDefaultAsync(m => m.Id == id);

            if (Group == null)
            {
                return NotFound();
            }

            Students = await _context.Student.Where(s => s.GroupId == id).ToListAsync();

            if (Group.TeacherId == null)
            {
                TeacherName = "Not Assigned";
            }
            else
            {
                Teacher = await UserManager.FindByIdAsync(Group.TeacherId.ToString());
            }            

            if(Teacher != null)
            {
                TeacherName = Teacher.FirstName + " " + Teacher.LastName;
            }            

            return Page();
        }
    }
}
