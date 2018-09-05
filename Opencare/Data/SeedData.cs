using Opencare.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Opencare.Authorization;

namespace Opencare.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // For sample purposes seed both with the same password.
                // Password is set with the following:
                // dotnet user-secrets set SeedUserPW <pw>
                // The admin user can do anything

                var adminID = await EnsureUser(serviceProvider, testUserPw, "admin@o.com", "Admin", "Admin");
                await EnsureRole(serviceProvider, adminID, Constants.AdministratorsRole);

                // allowed user can create and edit contacts that they create
                var teacherID = await EnsureUser(serviceProvider, testUserPw, "teacher@o.com", "Good", "Teacher");
                await EnsureRole(serviceProvider, teacherID, Constants.TeachersRole);

                // allowed user can create and edit contacts that they create
                var parentID = await EnsureUser(serviceProvider, testUserPw, "parent@o.com", "Guy", "A");
                await EnsureRole(serviceProvider, parentID, Constants.ParentsRole);

                SeedDB(context, adminID);
            }
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
                                            string testUserPw, string UserName, string FirstName, string LastName)
        {
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            var user = await userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = UserName,
                    FirstName = FirstName,
                    LastName = LastName
                };
                await userManager.CreateAsync(user, testUserPw);
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
                                                              string uid, string role)
        {
            IdentityResult IR = null;
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            var user = await userManager.FindByIdAsync(uid);

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }

        public static void SeedDB(ApplicationDbContext context, string adminID)
        {
            if (context.Student.Any())
            {
                return;   // DB has been seeded
            }

            context.Student.AddRange(
                new Student
                {
                    FirstName = "Debra",
                    LastName = "Garcia",
                    Birthdate = new DateTime(2016, 05, 05),
                    Status = EnrollmentStatus.Enrolled,
                    ParentID = adminID
                },
                new Student
                {
                    FirstName = "Jon",
                    LastName = "Orton",
                    Birthdate = new DateTime(2017, 05, 05),
                    Status=EnrollmentStatus.Pending,
                    ParentID = adminID
                },
                new Student
                {
                    FirstName = "Diliana",
                    LastName = "Alexieva-Bosseva",
                    Birthdate = new DateTime(2018, 05, 05),
                    Status=EnrollmentStatus.NotEnrolled,
                    ParentID = adminID
                }
            );
            context.SaveChanges();
        }
    }
}