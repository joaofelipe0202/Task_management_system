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
            SeedUser(context, "ltl@mw.com", "123456Ltl.", 130, "Developer");

            var user = SeedUser(context, "Jonny@mw.com", "123456Mw.", 130, "ProjectManager");      

            SeedUser(context, "Adam@mw.com", "123456Mw.", 130, "Developer");

            SeedUser(context, "Courtney@mw.com", "123456Mw.", 130, "Developer");

            SeedUser(context, "Amanda@mw.com", "123456Mw.", 130, "Developer");

            Project project = new Project("task management", 1300, DateTime.Now, user.Id);

            ProjectTask task = new ProjectTask()
            {
                ManagerId = user.Id,
                Title = "create git repo",
                Contents = "lalal",
                DateCreated = DateTime.Now,
                Complete = false
            };
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
