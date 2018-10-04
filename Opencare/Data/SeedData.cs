using Opencare.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Opencare.Authorization;
using System.Collections.Generic;

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
                var signInID = await EnsureUser(serviceProvider, testUserPw, "signin@o.com", "SignIn", "SignIn");
                await EnsureRole(serviceProvider, signInID, Constants.SignInRole);

                List<string> teachers = new List<string>();
                List<string> parents = new List<string>();

                // Teachers
                var teacherID = await EnsureUser(serviceProvider, testUserPw, "teacher@o.com", "Baby", "Teacher");
                await EnsureRole(serviceProvider, teacherID, Constants.TeachersRole);
                teachers.Add(teacherID);
                var teacherID2 = await EnsureUser(serviceProvider, testUserPw, "teacher2@o.com", "Tot", "Teacher");
                await EnsureRole(serviceProvider, teacherID2, Constants.TeachersRole);
                teachers.Add(teacherID2);
                var teacherID3 = await EnsureUser(serviceProvider, testUserPw, "teacher3@o.com", "Bigkid", "Teacher");
                await EnsureRole(serviceProvider, teacherID3, Constants.TeachersRole);
                teachers.Add(teacherID3);

                // Parents
                var parentID = await EnsureUser(serviceProvider, testUserPw, "parent@o.com", "Parent", "Mom");
                await EnsureRole(serviceProvider, parentID, Constants.ParentsRole);
                parents.Add(parentID);
                var parentID2 = await EnsureUser(serviceProvider, testUserPw, "parent2@o.com", "Parent", "Mann");
                await EnsureRole(serviceProvider, parentID2, Constants.ParentsRole);
                parents.Add(parentID2);


                SeedDB(context, adminID, teachers, parents);
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
                    LastName = LastName,
                    PIN = "0000",
                    Email = UserName
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

        public static void SeedDB(ApplicationDbContext context, string adminID, List<string> teachers, List<string> parents)
        {
            if (context.Student.Any())
            {
                return;   // DB has been seeded
            }
            
            var group1 = new Group
            {
                Name = "Infants",
                Room = "1",
                MinAge = 0,
                MaxAge = 1,
                TeacherId = teachers[0]
            };
            var group2 = new Group
            {
                Name = "Toddlers",
                Room = "2",
                MinAge = 1,
                MaxAge = 2,
                TeacherId = teachers[1]
            };
            var group3 = new Group
            {
                Name = "Preschool",
                Room = "3",
                MinAge = 3,
                MaxAge = 4,
                TeacherId = teachers[2]
            };

            context.Group.Add(new Group
            {
                Name = "Unassigned"
            }
            );

            context.Student.AddRange(
                new Student
                {
                    FirstName = "Debra",
                    LastName = "Garcia",
                    Birthdate = new DateTime(2016, 05, 05),
                    Status = EnrollmentStatus.Pending,
                    ParentID = parents[0],
                    Group = group1,
                    SignIns = new List<SignIn> { new SignIn() { IsSignedIn = false, Time = DateTime.Now } },
                    IsSignedIn = false
                },
                new Student
                {
                    FirstName = "Jon",
                    LastName = "Orton",
                    Birthdate = new DateTime(2017, 05, 05),
                    Status=EnrollmentStatus.Pending,
                    ParentID = parents[0],
                    Group = group2,
                    SignIns = new List<SignIn> { new SignIn() { IsSignedIn = false, Time = DateTime.Now } },
                    IsSignedIn = false
                },
                new Student
                {
                    FirstName = "Diliana",
                    LastName = "Alexieva-Bosseva",
                    Birthdate = new DateTime(2018, 05, 05),
                    Status=EnrollmentStatus.Pending,
                    ParentID = parents[0],
                    Group = group3,
                    SignIns = new List<SignIn> { new SignIn() { IsSignedIn = false, Time = DateTime.Now } },
                    IsSignedIn = false
                },
                new Student
                {
                    FirstName = "Bob",
                    LastName = "Kidd",
                    Birthdate = new DateTime(2017, 05, 05),
                    Status = EnrollmentStatus.Pending,
                    ParentID = parents[1],
                    Group = group1,
                    SignIns = new List<SignIn> { new SignIn() { IsSignedIn = false, Time = DateTime.Now } },
                    IsSignedIn = false
                },
                new Student
                {
                    FirstName = "Sally",
                    LastName = "Kidd",
                    Birthdate = new DateTime(2018, 05, 05),
                    Status = EnrollmentStatus.Pending,
                    ParentID = parents[1],
                    Group = group2,
                    SignIns = new List<SignIn> { new SignIn() { IsSignedIn = false, Time = DateTime.Now } },
                    IsSignedIn = false
                },
                new Student
                {
                    FirstName = "Dan",
                    LastName = "Kidd",
                    Birthdate = new DateTime(2018, 05, 05),
                    Status = EnrollmentStatus.Pending,
                    ParentID = parents[1],
                    Group = group3,
                    SignIns = new List<SignIn> { new SignIn() { IsSignedIn = false, Time = DateTime.Now } },
                    IsSignedIn = false
                }
            );
            context.SaveChanges();
        }
    }
}