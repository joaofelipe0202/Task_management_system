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
    public static class UserManager
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        private static RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
        private static UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

        [Authorize]
        public static List<string> GetAllRoles(string userName)
        {
            if (userName == null)
                return null;

            var user = db.Users.First(u => u.UserName == userName);

            if (user == null)
                return null;

            return userManager.GetRoles(user.Id).ToList();
        }

        [Authorize(Roles = "ProjectManager")]
        public static bool AddUserToRole(string userName, string roleName)
        {
            if (userName == null || roleName == null)
                return false;

            var user = db.Users.First(u => u.UserName == userName);
            var role = roleManager.FindByName(roleName);

            if (user == null || role == null)
                return false;

            userManager.AddToRole(user.Id, role.Name);
            return true;
        }

        [Authorize]
        public static bool CheckUserHasRole(string userName, string roleName)
        {
            if (userName == null || roleName == null)
                return false;

            var user = db.Users.First(u => u.UserName == userName);
            var role = roleManager.FindByName(roleName);

            if (user == null || role == null)
                return false;

            return userManager.IsInRole(user.Id, role.Name); ;
        }

        [Authorize(Roles="ProjectManager")]
        public static void Create(string userName, string email, double dailySalaray, string password )
        {
            var user = new ApplicationUser(userName, email, dailySalaray);
            userManager.Create(user, password);
            db.SaveChanges();
        }

        [Authorize(Roles = "ProjectManager")]
        public static void Delete(string userId)
        {
            if (userId == null)
                return;

            var user = db.Users.Find(userId);

            if (user == null)
                return;
            
            db.Users.Remove(user);
            db.SaveChanges();
        }

        [Authorize(Roles = "ProjectManager")]
        public static void Update(string userId)
        {
            if (userId == null)
                return;

            var user = db.Users.Find(userId);

            if (user == null)
                return;

            db.Entry(user).State = EntityState.Modified;
        }
    }
}