using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Task_Managment_System.Models
{
    public class ProjectHelper
    {
        static ApplicationDbContext db = new ApplicationDbContext();
        static UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(db);
        static UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(store);

        [Authorize(Roles="ProjectManager")]
        public static void Add(string name, double budget, DateTime deadline, string creatorId)
        {
            var newProject = new Project(name, budget, deadline, creatorId);
            db.Projects.Add(newProject);
        }

        [Authorize(Roles = "ProjectManager")]
        public static void Delete(int projectId)
        {
            var project = db.Projects.Find(projectId);          
            db.Projects.Remove(project);
        }

        [Authorize(Roles = "ProjectManager")]
        public static void Update(int projectId)
        {
            var project = db.Projects.Find(projectId);
            db.Entry(project).State = EntityState.Modified;
            db.SaveChanges();
        }
        [Authorize(Roles = "ProjectManager")]
        public static void CreateNewUser(string email, string password, double? salary)
        {
            if(!db.Users.Any(u => u.UserName == email))
            {
                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
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
            }
        }
        public static bool CheckIfUserInRole(string userId, string role)
        {
            var result = manager.IsInRole(userId, role);
            return result;
        }
        public static bool AddUserToRole(string userId, string role)
        {
            var user = db.Users.Find(userId);

            if (CheckIfUserInRole(userId, role))
                return false;
            else
            {
                manager.AddToRole(userId, role);
                return true;
            }  
        }
    }
}