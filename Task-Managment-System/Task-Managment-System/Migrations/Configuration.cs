namespace Task_Managment_System.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Task_Managment_System.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Task_Managment_System.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Task_Managment_System.Models.ApplicationDbContext context)
        {
            //create the roles
            if (!context.Roles.Any(r => r.Name == "ProjectManager"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Id = Guid.NewGuid().ToString().Substring(0, 10), Name = "ProjectManager" };

                roleManager.Create(role);      
            }
            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Id = Guid.NewGuid().ToString().Substring(0, 10), Name = "Developer" };

                roleManager.Create(role);
            }

            //create the user
            //if (!context.Users.Any(u => u.UserName == "Project Manager (1)"))
            //{
            //    var store = new UserStore<ApplicationUser>(context);
            //    var userManager = new UserManager<ApplicationUser>(store);
            //    var passwordHasher = new PasswordHasher();
            //    var user = new ApplicationUser
            //    {
            //        Id = Guid.NewGuid().ToString().Substring(0, 10),
            //        UserName = "Project Manager (1)",
            //        Email = "pm1@manager.com",
            //        EmailConfirmed = true,
            //        PasswordHash = passwordHasher.HashPassword("pm123456"),
            //        SecurityStamp = Guid.NewGuid().ToString(),
            //        PhoneNumber = "(000) 000-0000",
            //        PhoneNumberConfirmed = true,
            //        TwoFactorEnabled = false,
            //        LockoutEndDateUtc = DateTime.Now,
            //        LockoutEnabled = true,
            //        AccessFailedCount = 0
            //    };

            //    userManager.Create(user);
            //    userManager.AddToRole(user.Id, "ProjectManager");
            //}

            //seed user ref: https://stackoverflow.com/questions/19280527/mvc-5-seed-users-and-roles
            SeedUser(context, "ltl@mw.com", "123456Ltl.", 130, "ProjectManager");

            var user = SeedUser(context, "Jonny@mw.com", "123456Mw.", 130, "ProjectManager");      

            SeedUser(context, "Adam@mw.com", "123456Mw.", 130, "Developer");

            SeedUser(context, "Courtney@mw.com", "123456Mw.", 130, "Developer");

            SeedUser(context, "Amanda@mw.com", "123456Mw.", 130, "Developer");

            Project project = new Project("task management", 1300, DateTime.Now.AddDays(15), user.Id);
            project.DateCreated = DateTime.Now;
            project.Description = "a task management web app";
            project.Complete = true;
            context.Projects.AddOrUpdate(p => p.Name, project);


            ProjectTask task1 = new ProjectTask()
            {
                ManagerId = user.Id,
                Title = "create git repo",
                Contents = "lalal",
                DateCreated = DateTime.Now,
                Deadline = DateTime.Now.AddDays(4),
                Complete = false,
                ProjectId = project.Id
            };
            project.Tasks.Add(task1);

            context.Tasks.AddOrUpdate(t => t.Id, task1);

            //Project project1 = new Project("Some works", 1300, DateTime.Now.AddDays(20), user.Id);
            //project1.DateCreated = DateTime.Now;
            //project1.Description = "something";


            //context.Projects.AddOrUpdate(p => p.Name, project1);


            //ProjectTask task1 = new ProjectTask()
            //{
            //    ManagerId = user.Id,
            //    Title = "Readme",
            //    Contents = "lalal",
            //    DateCreated = DateTime.Now,
            //    Deadline = DateTime.Now.AddDays(4),
            //    Complete = false,
            //    ProjectId = project.Id
            //};

            //context.Tasks.AddOrUpdate(t => t.Id, task1);

            //ProjectTask task2 = new ProjectTask()
            //{
            //    ManagerId = user.Id,
            //    Title = "Readme",
            //    Contents = "lalal",
            //    DateCreated = DateTime.Now,
            //    Deadline = DateTime.Now.AddDays(4),
            //    Complete = false,
            //    ProjectId = project.Id,
            //    Priority = Priority.Low
            //};

            //context.Tasks.AddOrUpdate(t => t.Id, task2);

            Project project2 = new Project("Open a restaurant", 1300, DateTime.Now.AddDays(10), user.Id);
            project2.DateCreated = DateTime.Now;
            project2.Description = "How to open a restaurant";

            context.Projects.AddOrUpdate(p => p.Name, project2);

            Project project3 = new Project("Today's job", 100, DateTime.Now.AddDays(3), user.Id);
            project3.DateCreated = DateTime.Now;
            project3.Description = "What did you do today";

            context.Projects.AddOrUpdate(p => p.Name, project3);





            //ProjectTask task3 = new ProjectTask()
            //{
            //    ManagerId = user.Id,
            //    Title = "Finish online courses",
            //    Contents = "asddadadasdsadsadadsad",
            //    DateCreated = DateTime.Now,
            //    Deadline = DateTime.Now.AddHours(5),
            //    Complete = false,
            //    ProjectId = project3.Id
            //};

            //context.Tasks.AddOrUpdate(t => t.Id, task3);

        }

        private ApplicationUser SeedUser(ApplicationDbContext context, string email, string password, float salary, string role)
        {
            //UserName and Email have to be identical for now otherwise it will not work. we can fix this problem later.
            if (!context.Users.Any(u => u.UserName == email))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);

                var user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    DateCreated = DateTime.Now,
                    DailySalaray = salary,
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = true,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                };

                manager.Create(user, password);

                if(role != null)
                    manager.AddToRole(user.Id, role);

                return user;
            }

            return context.Users.FirstOrDefault(user => user.UserName == email);
        }
    }
}
