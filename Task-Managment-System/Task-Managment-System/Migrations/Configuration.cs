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
            if(!context.Roles.Any(r => r.Name == "ProjectManager"))
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
            if (!context.Users.Any(u => u.UserName == "Project Manager (1)"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(store);
                var passwordHasher = new PasswordHasher();
                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString().Substring(0, 10),
                    UserName = "Project Manager (1)",
                    Email = "pm1@manager.com",
                    EmailConfirmed = true,
                    PasswordHash = passwordHasher.HashPassword("pm123456"),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    PhoneNumber = "(000) 000-0000",
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false,
                    LockoutEndDateUtc = DateTime.Now,
                    LockoutEnabled = true,
                    AccessFailedCount = 0                    
                };

                userManager.Create(user);
                //userManager.AddToRole(user.Id, "ProjectManager");
            }

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
