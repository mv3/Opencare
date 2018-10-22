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

        public List<Diaper> Diapers { get; set; }
        [BindProperty]
        public Diaper Diaper { get; set; }

        public List<StudentNote> Notes { get; set; }
        [BindProperty]
        public StudentNote Note { get; set; }

        public class AddBottle : Bottle
        {
            public string OtherType { get; set; }
        }

        public List<Bottle> Bottles { get; set; }
        [BindProperty]
        public AddBottle Bottle { get; set; }
        public IEnumerable<SelectListItem> BottleTypes { get; set; }

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

            Diapers = await Context.Diapers
                .Where(d => d.Student == Student && d.Time.Date == DateTime.Today.Date)
                .Include(d=>d.Changer)
                .ToListAsync();
            Diaper = new Diaper { Time = DateTime.Now };

            Notes = await Context.StudentNote
                .Where(n => n.Student == Student && n.Date.Date == DateTime.Today.Date)
                .Include(n=>n.AppUser)
                .ToListAsync();
            Note = new StudentNote { Date = DateTime.Now };

            Bottles = await Context.Bottles
                .Where(d => d.Student == Student && d.Time.Date == DateTime.Today.Date)
                .Include(b=>b.Teacher)
                .ToListAsync();
            Bottle = new AddBottle { Time = DateTime.Now };

            var bottleTypeList = await Context.Bottles.Select(b => b.Type).Distinct().ToListAsync();

            BottleTypes = bottleTypeList.Select(x =>
                        new SelectListItem()
                        {
                            Text = x.ToString(),
                            Value = x.ToString()
                        });


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

        public async Task<IActionResult> OnPostNewDiaperAsync(int id)
        {
            ModelState.Remove("Document");
            if (ModelState.IsValid)
            {
                ApplicationUser UpUser = await UserManager.FindByIdAsync(UserManager.GetUserId(User));
                var student = await Context.Student.FirstOrDefaultAsync(
                                                      m => m.StudentId == id);

                var diaper = new Diaper
                {
                    Time = Diaper.Time,
                    Wet = Diaper.Wet,
                    Dirty = Diaper.Dirty,
                    Note = Diaper.Note,
                    Changer = UpUser,
                    Student = student

                };



                Context.Add(diaper);
                await Context.SaveChangesAsync();
                return RedirectToPage("./Details", new { id });
            }
            return RedirectToPage("./Details", new { id });

        }

        public async Task<IActionResult> OnPostNewBottleAsync(int id)
        {
            ModelState.Remove("Document");
            if (ModelState.IsValid)
            {
                ApplicationUser UpUser = await UserManager.FindByIdAsync(UserManager.GetUserId(User));
                var student = await Context.Student.FirstOrDefaultAsync(
                                                      m => m.StudentId == id);

                var bottle = new Bottle
                {
                    Time = Bottle.Time,
                    Ounces = Bottle.Ounces,
                    Note = Bottle.Note,
                    Teacher = UpUser,
                    Student = student
                };

                if (Bottle.Type == "Other")
                {
                    bottle.Type = Bottle.OtherType;
                }
                else
                {
                    bottle.Type = Bottle.Type;
                }

                Context.Add(bottle);
                await Context.SaveChangesAsync();
                return RedirectToPage("./Details", new { id });
            }
            return RedirectToPage("./Details", new { id });

        }

        public async Task<IActionResult> OnPostNewNoteAsync(int id)
        {
            ModelState.Remove("Document");
            if (ModelState.IsValid)
            {
                ApplicationUser UpUser = await UserManager.FindByIdAsync(UserManager.GetUserId(User));
                var student = await Context.Student.FirstOrDefaultAsync(
                                                      m => m.StudentId == id);

                var note = new StudentNote
                {
                    Date = Note.Date,
                    Note = Note.Note,                   
                    AppUser = UpUser,
                    Student = student
                };

                Context.Add(note);
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
