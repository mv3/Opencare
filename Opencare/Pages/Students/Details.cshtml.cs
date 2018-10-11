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
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Opencare.Pages.Students
{
    public class DetailsModel : DI_BasePageModel
    {
        public DetailsModel(ApplicationDbContext context,
        IAuthorizationService authorizationService,
        UserManager<ApplicationUser> userManager)
        : base(context, authorizationService, userManager)
        {
        }

        [BindProperty]
        public Student Student { get; set; }

        [BindProperty]
        [Display(Name = "Parent")]
        public string ParentName { get; set; }

        public List<DocumentType> DocumentTypes { get; set; }

        public class UploadDocument
        {
            [Required]
            public int DocumentTypeId { get; set; }

            [Required]
            public IFormFile Document { get; set; }
        }

        [BindProperty]
        public UploadDocument StudentDocument { get; set; }

        public List<StudentDocument> StudentDocs { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            DocumentTypes = await Context.DocumentType.ToListAsync();

            Student = await Context.Student
                .Include(s => s.Parent)
                .Include(s => s.SignIns)
                .FirstOrDefaultAsync(m => m.StudentId == id);

            if (Student == null)
            {
                return NotFound();
            }

            StudentDocs = await Context.StudentDocuments
                .Where(d => d.Student == Student)
                .Include(d=>d.UploadUser)
                .Include(d=>d.DocumentType)
                .ToListAsync();

            ParentName = Student.Parent.FirstName + " " + Student.Parent.LastName;

            var isAuthorized = User.IsInRole(Constants.TeachersRole) ||
                           User.IsInRole(Constants.AdministratorsRole);

            var currentUserId = UserManager.GetUserId(User);

            if (!isAuthorized
                && currentUserId != Student.ParentID
                && Student.Status != EnrollmentStatus.Enrolled)
            {
                return new ChallengeResult();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, EnrollmentStatus status)
        {
            var student = await Context.Student.FirstOrDefaultAsync(
                                                      m => m.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            var contactOperation = (status == EnrollmentStatus.Enrolled)
                                                       ? StudentOperations.Approve
                                                       : StudentOperations.Reject;

            var isAuthorized = await AuthorizationService.AuthorizeAsync(User, student,
                                        contactOperation);
            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }
            student.Status = status;
            Context.Student.Update(student);



            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostNewDocAsync(int id )
        {
            if (ModelState.IsValid)
            {
                ApplicationUser UpUser = await UserManager.FindByIdAsync(UserManager.GetUserId(User));
                var student = await Context.Student.FirstOrDefaultAsync(
                                                      m => m.StudentId == id);
                var ext = Path.GetExtension(StudentDocument.Document.FileName);

                var docType = await Context.DocumentType.Where(d => d.Id == StudentDocument.DocumentTypeId).FirstOrDefaultAsync();
                
                var SD = new StudentDocument
                {
                    DocumentTypeId = StudentDocument.DocumentTypeId,
                    UploadDT = DateTime.Now,
                    UploadUser = UpUser,
                    Student = student,
                    FileName = student.FirstName + "_" + student.LastName + "-" + docType.Name + "-" + DateTime.Now.ToString("yyyy-dd-M-HH-mm-ss") + ext,
                    ContentType = StudentDocument.Document.ContentType
            };

                using (var memoryStream = new MemoryStream())
                {
                    await StudentDocument.Document.CopyToAsync(memoryStream);
                    SD.Document = memoryStream.ToArray();
                }
                

                Context.Add(SD);
                await Context.SaveChangesAsync();
                return RedirectToPage("./Details", new { id });
            }
            return RedirectToPage("./Details", new { id });

        }

        public FileResult OnGetDownload(int id)
        {
            var doc = Context.StudentDocuments.Where(d => d.Id == id).Include(d => d.Student).FirstOrDefault();
            return File(doc.Document, doc.ContentType, doc.FileName);
            //System.Net.Mime.MediaTypeNames.Application.Octet
        }
    }
}
