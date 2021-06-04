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

            var user2 = SeedUser(context, "Jonny@mw.com", "123456Mw.", 130, "ProjectManager");

            var dev1 = SeedUser(context, "Adam@mw.com", "123456Mw.", 130, "Developer");

            var dev2 = SeedUser(context, "Courtney@mw.com", "123456Mw.", 130, "Developer");

            var dev3 = SeedUser(context, "Maggie@mw.com", "123456Mw.", 130, "Developer");

            var dev4 = SeedUser(context, "Amanda@mw.com", "123456Mw.", 130, "Developer");

            Project project = new Project("task management", 1300, DateTime.Now.AddDays(15), user2.Id);
            project.DateCreated = DateTime.Now;
            project.Description = "a task management web app";
            project.Complete = true;
            project.ActualCost = 1000;
            project.Priority = Priority.Urgent;
            project.LastBudgetUpdate = DateTime.Now;

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
                AssignedUserId = user2.Id,
                PercentageCompleted = 50,
                Priority = Priority.Average
            };
            project.Tasks.Add(task);

            context.Tasks.AddOrUpdate(t => t.Title, task);
            context.SaveChanges();

            Project project1 = new Project("Some works", 1300, DateTime.Now.AddDays(20), user2.Id);
            project1.DateCreated = DateTime.Now;
            project1.Description = "something";
            project1.ActualCost = 2000;
            project1.Priority = Priority.Low;
            project1.LastBudgetUpdate = DateTime.Now;

            context.Projects.AddOrUpdate(p => p.Name, project1);


            ProjectTask task1 = new ProjectTask()
            {
                ManagerId = user2.Id,
                Title = "Readme",
                Contents = "lalal",
                DateCreated = DateTime.Now,
                Deadline = DateTime.Now.AddDays(4),
                Complete = false,
                ProjectId = project.Id
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
                Priority = Priority.Low
            };

            context.Tasks.AddOrUpdate(t => t.Title, task2);

            Project project2 = new Project("Open a restaurant", 1300, DateTime.Now.AddDays(10), user2.Id);
            project2.DateCreated = DateTime.Now;
            project2.Description = "How to open a restaurant";
            project2.ActualCost = 1100;
            project2.Priority = Priority.High;
            project2.LastBudgetUpdate = DateTime.Now;

            context.Projects.AddOrUpdate(p => p.Name, project2);
            context.SaveChanges();

            Project project3 = new Project("Today's job", 100, DateTime.Now.AddDays(3), user2.Id);
            project3.DateCreated = DateTime.Now;
            project3.Description = "What did you do today";
            project3.ActualCost = 2200;
            project.Priority = Priority.Low;
            project3.LastBudgetUpdate = DateTime.Now;

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
                ProjectId = project3.Id
            };

            context.Tasks.AddOrUpdate(t => t.Title, task3);

            ProjectTask task4 = new ProjectTask
            {
                ManagerId = user1.Id,
                Title = "Implement new feature",
                Contents = "asddadadasdsadsadadsad",
                DateCreated = DateTime.Now,
                Deadline = DateTime.Now.AddHours(10),
                Complete = false,
                ProjectId = project2.Id,
                AssignedUserId = "df89c449-187d-49fa-bfc6-1980ee94957d"
            };
            context.Tasks.AddOrUpdate(t => t.Title, task4);

            ProjectTask task5 = new ProjectTask
            {
                ManagerId = user1.Id,
                Title = "Testing Project Controller",
                Contents = "asddadadasdsadsadadsad",
                DateCreated = DateTime.Now,
                Deadline = DateTime.Now.AddHours(2),
                Complete = false,
                ProjectId = project1.Id,
                AssignedUserId = "7986eb62-55d3-4b95-841e-3a881eca1fd4"
            };
            context.Tasks.AddOrUpdate(t => t.Title, task5);

            ProjectTask task6 = new ProjectTask
            {
                ManagerId = user1.Id,
                Title = "Testing Task Helper",
                Contents = "asddadadasdsadsadadsad",
                DateCreated = DateTime.Now,
                Deadline = DateTime.Now.AddHours(5),
                Complete = false,
                ProjectId = project3.Id,
                AssignedUserId = "a8bcccc5-da7c-4ae2-bce0-65c111c1dd5c"
            };
            context.Tasks.AddOrUpdate(t => t.Title, task6);
            Comment comment1 = new Comment
            {
                Title = "Nice feature implemented",
                Content = "Nice feature",
                CreatorId = "f067a25c-685e-4747-9143-d33f680d8129",
                TaskId = 27,
                IsUrgent = false,
                DateCreated = DateTime.Now
            };
            context.Comments.AddOrUpdate(c => c.Title, comment1);

            Comment comment2 = new Comment
            {
                Title = "Change this ASAP",
                Content = "Fix ASAP!!!",
                CreatorId = "df89c449-187d-49fa-bfc6-1980ee94957d",
                TaskId = 23,
                IsUrgent = true,
                DateCreated = DateTime.Now
            };
            context.Comments.AddOrUpdate(c => c.Title, comment2);

            Comment comment3 = new Comment
            {
                Title = "Nice",
                Content = "Did not understand",
                CreatorId = "a8bcccc5-da7c-4ae2-bce0-65c111c1dd5c",
                TaskId = 24,
                IsUrgent = false,
                DateCreated = DateTime.Now
            };
            context.Comments.AddOrUpdate(c => c.Title, comment3);

            Comment comment4 = new Comment
            {
                Title = "Implement this",
                Content = "DANGEROUS",
                CreatorId = "7986eb62-55d3-4b95-841e-3a881eca1fd4",
                TaskId = 27,
                IsUrgent = true,
                DateCreated = DateTime.Now
            };
            context.Comments.AddOrUpdate(c => c.Title, comment4);

            Notification notification1 = new Notification
            {
                UserId = dev1.Id,
                Title = "Finished testing controller",
                Contents = "Just finished testing the controller",
                Read = false,
                CreationDate = DateTime.Now,
                Type = NotificationType.Complete,
                TaskId = 30,
                ProjectId = project.Id
            };
            context.Notifications.AddOrUpdate(n => n.Title, notification1);

            Notification notification2 = new Notification
            {
                UserId = dev1.Id,
                Title = "Code is crashing",
                Contents = "Code is crashing when Update-Database",
                Read = false,
                CreationDate = DateTime.Now,
                Type = NotificationType.Urgent,
                TaskId = task4.Id,
                ProjectId = project1.Id
            };
            context.Notifications.AddOrUpdate(n => n.Title, notification2);

            Notification notification3 = new Notification
            {
                UserId = dev3.Id,
                Title = "Need to review",
                Contents = "Need to review the implemented code",
                Read = true,
                CreationDate = DateTime.Now,
                Type = NotificationType.Notice,
                TaskId = task3.Id,
                ProjectId = project2.Id
            };
            context.Notifications.AddOrUpdate(n => n.Title, notification3);

            Notification notification4 = new Notification
            {
                UserId = dev2.Id,
                Title = "Need more time to implement",
                Contents = "Try to review what we've done",
                Read = true,
                CreationDate = DateTime.Now,
                Type = NotificationType.Overdue,
                TaskId = task2.Id,
                ProjectId = project3.Id
            };
            context.Notifications.AddOrUpdate(n => n.Title, notification4);

            Notification notification5 = new Notification
            {
                UserId = dev4.Id,
                Title = "Need more money!!",
                Contents = "Unfortunately we gonna need more budget to finish this out",
                Read = true,
                CreationDate = DateTime.Now,
                Type = NotificationType.Overbudget,
                TaskId = task6.Id,
                ProjectId = project2.Id
            };
            context.Notifications.AddOrUpdate(n => n.Title, notification5);

            Notification notification6 = new Notification
            {
                UserId = dev1.Id,
                Title = "Test is done",
                Contents = "Tested the new code",
                Read = false,
                CreationDate = DateTime.Now,
                Type = NotificationType.Complete,
                TaskId = task1.Id,
                ProjectId = project.Id
            };
            context.Notifications.AddOrUpdate(n => n.Title, notification6);

            Notification notification7 = new Notification
            {
                UserId = dev4.Id,
                Title = "Code is failing to run",
                Contents = "Code sent to the branch is not working",
                Read = true,
                CreationDate = DateTime.Now,
                Type = NotificationType.Urgent,
                TaskId = task4.Id,
                ProjectId = project3.Id
            };
            context.Notifications.AddOrUpdate(n => n.Title, notification7);
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
