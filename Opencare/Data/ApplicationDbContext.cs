using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Opencare.Models;

namespace Opencare.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Core Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Core Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Opencare.Models.Student> Student { get; set; }

        public DbSet<Opencare.Data.ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Opencare.Models.Group> Group { get; set; }
        
        public DbSet<Opencare.Models.SignIn> SignIns { get; set; }

        public DbSet<Opencare.Models.StudentDocument> StudentDocuments { get; set; }

        public DbSet<Opencare.Models.DocumentType> DocumentType { get; set; }

        public DbSet<Opencare.Models.Diaper> Diapers { get; set; }
        public DbSet<Opencare.Models.Bottle> Bottles { get; set; }
        public DbSet<Opencare.Models.StudentNote> StudentNote { get; set; }
    }
}
