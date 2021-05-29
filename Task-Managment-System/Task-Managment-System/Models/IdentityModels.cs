using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Task_Managment_System.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<ProjectTask> Tasks { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public DateTime DateCreated { get; set; }
        public double? DailySalary { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public ApplicationUser()
        {
            Tasks = new HashSet<ProjectTask>();
            Projects = new HashSet<Project>();
            Notifications = new HashSet<Notification>();
            Comments = new HashSet<Comment>();
        }

        public ApplicationUser(string userName, string email, double dailySalary)
        {
            UserName = userName;
            Email = email;
            DailySalary = dailySalary;
            DateCreated = DateTime.Now;
            Tasks = new HashSet<ProjectTask>();
            Projects = new HashSet<Project>();
            Notifications = new HashSet<Notification>();
            Comments = new HashSet<Comment>();
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectTask> Tasks { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }

         public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}