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
            AutomaticMigrationsEnabled = false;
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

            //seed user ref: https://stackoverflow.com/questions/19280527/mvc-5-seed-users-and-roles
            var user1 = SeedUser(context, "ltl@mw.com", "123456Ltl.", 130, "ProjectManager");
            user1.DailySalary = 250;

            var user2 = SeedUser(context, "Jonny@mw.com", "123456Mw.", 130, "ProjectManager");
            user2.DailySalary = 250;
            
            var dev1 = SeedUser(context, "Adam@mw.com", "123456Mw.", 130, "Developer");
            dev1.DailySalary = 130;
            
            var dev2 = SeedUser(context, "Courtney@mw.com", "123456Mw.", 130, "Developer");
            dev2.DailySalary = 130;

            var dev3 = SeedUser(context, "Maggie@mw.com", "123456Mw.", 130, "Developer");
            dev3.DailySalary = 130;

            var dev4 = SeedUser(context, "Amanda@mw.com", "123456Mw.", 130, "Developer");
            dev4.DailySalary = 130;

            Project project = new Project("task management","a task management web app", 1300, DateTime.Now.AddDays(15),Priority.Urgent, user2.Id);
            project.DateCreated = DateTime.Now;
            project.Complete = true;
            project.ActualCost = 1000;
            //project.Priority = Priority.Urgent;
            
            context.Projects.AddOrUpdate(p => p.Name, project);
            context.SaveChanges();

            ProjectTask task = new ProjectTask()
            {
                ManagerId = user2.Id,
                Title = "create git repo",
                Contents = "lalal",
                DateCreated = DateTime.Now,
                Deadline = DateTime.Now.AddDays(4),
                Complete = false,
                ProjectId = project.Id,
                AssignedUserId = dev1.Id,
                PercentageCompleted = 50,
                Priority = Priority.Average
            };
            project.Tasks.Add(task);

            context.Tasks.AddOrUpdate(t => t.Title, task);
            context.SaveChanges();

            Project project1 = new Project("Some works", "something", 1300, DateTime.Now.AddDays(20), Priority.Low, user2.Id);
            project1.DateCreated = DateTime.Now;
            project1.ActualCost = 2000;
            //project1.Priority = Priority.Low;

            context.Projects.AddOrUpdate(p => p.Name, project1);


            ProjectTask task1 = new ProjectTask()
            {
                ManagerId = user2.Id,
                Title = "Readme",
                Contents = "lalal",
                DateCreated = DateTime.Now,
                Deadline = DateTime.Now.AddDays(4),
                Complete = false,
                ProjectId = project.Id,
                AssignedUserId = dev2.Id
            };

            context.Tasks.AddOrUpdate(t => t.Title, task1);

            ProjectTask task2 = new ProjectTask()
            {
                ManagerId = user2.Id,
                Title = "Readme1",
                Contents = "lalal",
                DateCreated = DateTime.Now,
                Deadline = DateTime.Now.AddDays(4),
                Complete = false,
                ProjectId = project.Id,
                Priority = Priority.Low,
                AssignedUserId = dev2.Id
            };

            context.Tasks.AddOrUpdate(t => t.Title, task2);

            Project project2 = new Project("Open a restaurant", "How to open a restaurant", 1300, DateTime.Now.AddDays(10),Priority.High, user2.Id);
            project2.DateCreated = DateTime.Now;
            project2.Description = "How to open a restaurant";
            project2.ActualCost = 1100;
            //project2.Priority = Priority.High;

            context.Projects.AddOrUpdate(p => p.Name, project2);
            context.SaveChanges();

            Project project3 = new Project("Today's job", "What did you do today", 100, DateTime.Now.AddDays(3),Priority.Low, user2.Id);
            project3.DateCreated = DateTime.Now;
            project3.Description = "What did you do today";
            project3.ActualCost = 2200;
            //project.Priority = Priority.Low;

            context.Projects.AddOrUpdate(p => p.Name, project3);
            context.SaveChanges();

            ProjectTask task3 = new ProjectTask()
            {
                ManagerId = user2.Id,
                Title = "Finish online courses",
                Contents = "asddadadasdsadsadadsad",
                DateCreated = DateTime.Now,
                Deadline = DateTime.Now.AddHours(5),
                Complete = false,
                ProjectId = project3.Id,
                AssignedUserId = dev4.Id
            };

            context.Tasks.AddOrUpdate(t => t.Title, task3);
        }

        private ApplicationUser SeedUser(ApplicationDbContext context, string email, string password, double salary, string role)
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
                    DailySalary = salary,
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
