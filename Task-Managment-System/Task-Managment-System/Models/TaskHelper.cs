using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Task_Managment_System.Models
{
    public class TaskHelper
    {
        static ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "ProjectManager")]
        public static void Add(string title, string contents, DateTime deadline, bool complete, Priority priority)
        {
            var newTask = new ProjectTask(title, contents, deadline, complete, priority);
            db.Tasks.Add(newTask);
            db.SaveChanges();
        }

        [Authorize(Roles = "ProjectManager")]
        public static void Delete(int taskId)
        {
            var task = db.Tasks.Find(taskId);
            if (task == null)
                return;

            db.Tasks.Remove(task);
            db.SaveChanges();
        }

        [Authorize(Roles = "ProjectManager")]
        public static void Update(int taskId)
        {
            var task = db.Tasks.Find(taskId);
            if (task == null)
                return;

            db.Entry(task).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}