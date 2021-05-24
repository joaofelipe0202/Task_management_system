using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Task_Managment_System.Models
{
    public class TaskHelper
    {
        static ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]
        public static void Add(string title, string contents, DateTime deadline, bool complete, Priority priority)
        {
            var newTask = new Task(title, contents, deadline, complete, priority);
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