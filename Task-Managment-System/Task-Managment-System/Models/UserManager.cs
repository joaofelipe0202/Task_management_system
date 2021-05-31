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
    public class UserManager
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private readonly RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
        private readonly UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

        public UserManager(ApplicationDbContext database)
        {
            db = database;
        }

        [Authorize]
        public  List<string> GetAllRoles(string userName)
        {
            if (userName == null)
                return null;

            var user = db.Users.First(u => u.UserName == userName);

            if (user == null)
                return null;

            return userManager.GetRoles(user.Id).ToList();
        }

        [Authorize(Roles = "ProjectManager")]
        public  bool AddUserToRole(string userName, string roleName)
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
        public  bool CheckUserHasRole(string userName, string roleName)
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
        public  void Create(string email, double? dailySalaray, string password )
        {
            if(!db.Users.Any(u => u.Email == email))
            {
                var user = new ApplicationUser(email, dailySalaray);
                userManager.Create(user, password);
                db.SaveChanges();
            }
            
        }

        [Authorize(Roles = "ProjectManager")]
        public  void Delete(string userId)
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
        public  void Update(string userId)
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