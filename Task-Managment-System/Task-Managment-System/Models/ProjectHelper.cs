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

        [Authorize(Roles="ProjectManager")]
        public static void Add(string name, double budget, DateTime deadline, string creatorId)
        {
            var newProject = new Project(name, budget, deadline, creatorId);
            db.Projects.Add(newProject);
            db.SaveChanges();
        }

        [Authorize(Roles = "ProjectManager")]
        public static void Delete(int projectId)
        {
            var project = db.Projects.Find(projectId);
            if (project == null)
                return;

            db.Projects.Remove(project);
            db.SaveChanges();
        }

        [Authorize(Roles = "ProjectManager")]
        public static void Update(int projectId)
        {
            var project = db.Projects.Find(projectId);
            if (project == null)
                return;

            db.Entry(project).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}