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
        private static readonly ApplicationDbContext db = new ApplicationDbContext();
        private static readonly UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(db);
        private static readonly UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(store);
        
        //public ProjectHelper(ApplicationDbContext database)
        //{
        //    db = database;
        //}

        public void Add(string name, double budget, DateTime deadline, string creatorId)
        {
            var newProject = new Project(name, budget, deadline, creatorId);
            db.Projects.Add(newProject);

            db.SaveChanges();
        }

        public Project By(int id)
        {
            var projects = db.Projects.Find(id);

            return projects;
        }


        public void Delete(int projectId)
        {
            var project = db.Projects.Find(projectId);          
            db.Projects.Remove(project);

            db.SaveChanges();
        }


        public void Update(int projectId)
        {
            //check modify
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
        public List<Project> Filter(FilterMethods method)
        {
            List<Project> projects = db.Projects.ToList();

            switch (method)
            {
                case FilterMethods.incomplete:
                    projects = ProjectsAreNotCompleted(projects);
                    break;
            }

            return projects;
        }

        private List<Project> ProjectsAreNotCompleted(List<Project> projects)
        {
            var filteredProjects = projects.Where(p =>
                    p.Complete == false
              )
              .ToList();

            return filteredProjects;
        }
    }
}