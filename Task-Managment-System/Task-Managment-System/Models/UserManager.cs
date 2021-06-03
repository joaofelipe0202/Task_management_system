using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Task_Managment_System.Models.ViewModel;

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
        public  bool AddUserToRole(string userId, string roleName)
        {
            if (CheckUserHasRole(userId, roleName))
                return false;
            else
            {
                userManager.AddToRole(userId, roleName);
                return true;
            }
        }

        [Authorize]
        public bool CheckUserHasRole(string userId, string role)
        {
            var result = userManager.IsInRole(userId, role);
            return result;
        }
        public  void Create(string email, double? dailySalary, string password)
        {
            if(!db.Users.Any(u => u.Email == email))
            {
                var user = new ApplicationUser(email, dailySalary);
                user.Email = email;
                user.DailySalary = dailySalary;

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