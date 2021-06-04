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

            CreateRole(context, "ProjectManager");
            CreateRole(context, "Developer");

            //seed user ref: https://stackoverflow.com/questions/19280527/mvc-5-seed-users-and-roles
            var user1 = SeedUser(context, "ltl@mw.com", "123456Ltl.", 130, "ProjectManager");
            var user2 = SeedUser(context, "Jonny@mw.com", "123456Mw.", 250, "ProjectManager");
            
            var dev1 = SeedUser(context, "Adam@mw.com", "123456Mw.", 130, "Developer");
            var dev2 = SeedUser(context, "Courtney@mw.com", "123456Mw.", 130, "Developer");
            var dev3 = SeedUser(context, "Maggie@mw.com", "123456Mw.", 130, "Developer");
            var dev4 = SeedUser(context, "Amanda@mw.com", "123456Mw.", 130, "Developer");

            var project1 = SeedProject(context, "task-management", "a task-management web app", 1300, DateTime.Now.AddDays(3), Priority.Urgent, user1.Id);
            var project1Task1 = SeedTask(context, user2.Id, "create git repo", "create the git repo", DateTime.Now.AddHours(3), project1.Id, dev1.Id);
            var project1Task2 = SeedTask(context, user2.Id, "Make init tables", "make some tables to start", DateTime.Now.AddHours(3), project1.Id, dev2.Id);
            var project1Task3 = SeedTask(context, user2.Id, "Create some helper models", "some helper methods to work on", DateTime.Now.AddHours(3), project1.Id, dev3.Id);

            var project2 = SeedProject(context, "movie database", "making a movie database backend", 1000, DateTime.Now.AddDays(8), Priority.Low, user1.Id);
            var project2Task1 = SeedTask(context, user2.Id, "create table in ssms", "create the tables", DateTime.Now.AddHours(3), project2.Id, dev4.Id);
            var project2Task2 = SeedTask(context, user2.Id, "readme", "write readme", DateTime.Now.AddMinutes(30), project2.Id, dev2.Id);
            var project2Task3 = SeedTask(context, user2.Id, "testing codes", "testing tables", DateTime.Now.AddHours(5), project2.Id, dev4.Id);


            var project3 = SeedProject(context, "restaurant management system", "writing a management syetem to trak customers in a restaurant", 50000, DateTime.Now.AddDays(14), Priority.Average, user1.Id);
            var project3Task1 = SeedTask(context, user2.Id, "create table in ssms", "create the tables", DateTime.Now.AddHours(3), project2.Id, dev1.Id);
            var project3Task2 = SeedTask(context, user2.Id, "readme", "write readme", DateTime.Now.AddMinutes(30), project2.Id, dev2.Id);
            var project3Task3 = SeedTask(context, user2.Id, "something", "something", DateTime.Now.AddMinutes(-30), project2.Id, dev2.Id);

            SeedComment(context, "Nice feature implemented", "Nice feature", dev1.Id, project1Task1.Id, false);
            SeedComment(context, "Change this ASAP", "Fix ASAP!!!", dev1.Id, project1Task1.Id, true);
            SeedComment(context, "Nice", "Did not understand", dev2.Id, project2Task1.Id, true);
            SeedComment(context, "Implement this", "DANGEROUS", dev2.Id, project1Task2.Id, true);

            SeedNotification(context, user1.Id, "Finished testing controller", "Finished testing controller", NotificationType.Complete, project1Task3.Id,null);
            SeedNotification(context, dev2.Id, "Code is crashing", "Code is crashing when Update-Database", NotificationType.Urgent,null, project2.Id);
            SeedNotification(context, dev1.Id, "Need to review", "Need to review the implemented code", NotificationType.Notice, null, project2.Id);
            SeedNotification(context, dev2.Id, "Need more time to implement", "Try to review what we've done", NotificationType.Notice, null, project2.Id);
            SeedNotification(context, user1.Id, "Need more money!!", "Unfortunately we gonna need more budget to finish this out", NotificationType.Urgent, null, project1.Id);
            SeedNotification(context, dev1.Id, "Test is done", "Tested the new cod", NotificationType.Complete, project2Task3.Id, null);
            SeedNotification(context, dev4.Id, "Code is failing to run", "Code sent to the branch is not working", NotificationType.Urgent, null, project3.Id);

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

        private Project SeedProject(ApplicationDbContext context, string name, string description, double budget, DateTime deadline, Priority priority, string creatorId)
        {
            Project project = new Project(name, description, budget, deadline, priority, creatorId);

            project.DateCreated = DateTime.Now;
            project.ActualCost = 0;
            project.LastBudgetUpdate = DateTime.Now;
            project.Complete = false;

            context.Projects.AddOrUpdate(p => p.Name, project);
            context.SaveChanges();

            return project;
        }

        private ProjectTask SeedTask(ApplicationDbContext context, string managerId ,string title, string contents, DateTime deadline, int projectId, string assignedUserId)
        {
            ProjectTask task = new ProjectTask
            {
                ManagerId = managerId,
                Title = title,
                Contents = contents,
                DateCreated = DateTime.Now,
                Deadline = deadline,
                Complete = false,
                ProjectId = projectId,
                AssignedUserId = assignedUserId,
                PercentageCompleted=0
            };

            context.Tasks.AddOrUpdate(t => t.Id, task);
            context.SaveChanges();

            return task;
        }

        private Comment SeedComment(ApplicationDbContext context, string title,string contents, string creatorId, int taskId, bool isUrgent)
        {
            Comment comment = new Comment
            {
                Title = title,
                Content = contents,
                CreatorId = creatorId,
                TaskId = taskId,
                IsUrgent = isUrgent,
                DateCreated = DateTime.Now
            };

            context.Comments.AddOrUpdate(c => c.Title, comment);
            context.SaveChanges();

            return comment;
        }

        private Notification SeedNotification(ApplicationDbContext context, string userId, string title, string contents, NotificationType type, int? taskId, int? projectId)
        {
            Notification notification = new Notification
            {
                UserId = userId,
                Title = title,
                Contents = contents,
                Read = false,
                CreationDate = DateTime.Now,
                Type = type,
                TaskId = taskId,
                ProjectId=projectId
            };
            context.Notifications.AddOrUpdate(n => n.Title, notification);
            context.SaveChanges();

            return notification;
        }

        private string CreateRole(ApplicationDbContext context, string roleName)
        {
            if (!context.Roles.Any(r => r.Name == roleName))
            {
                var store = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = roleName };

                roleManager.Create(role);

                return roleName;
            }

            return null;
        }

    }
}
