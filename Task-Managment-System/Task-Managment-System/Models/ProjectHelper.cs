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

        [Authorize]
        public static void Add(string name, double budget, DateTime deadline, string creatorId)
        {
            var newProject = new Project(name, budget, deadline, creatorId);
            db.Projects.Add(newProject);
        }

        [Authorize]
        public static void Delete(int projectId)
        {
            var project = db.Projects.Find(projectId);          
            db.Projects.Remove(project);
        }

        [Authorize]
        public static void Update(int projectId)
        {
            var project = db.Projects.Find(projectId);
            db.Entry(project).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}