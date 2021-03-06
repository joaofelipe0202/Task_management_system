using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Task_Managment_System.Models;

namespace Task_Managment_System.Controllers
{
    public class DevelopersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private NotificationHelper nh { get; set; }

        public DevelopersController()
        {
            nh = new NotificationHelper(db);
        }

        public ActionResult Index()
        {            
            return View();
        }

        [Authorize(Roles = "Developer, ProjectManager")]        
        public ActionResult GetAllDeveloperTasks()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            if (user == null)
                return HttpNotFound();
            
            var userName = user.UserName;
            var tasksList = db.Tasks.Where(t => t.AssignedUser.UserName == userName).OrderBy(t => t.Priority).ToList();
            ViewBag.UserName = userName;
            return View(tasksList);
        }

        public ActionResult ChangeStatusOfTask(int taskId)
        {
            ProjectTask task = db.Tasks.Find(taskId);
            if (task == null)
                return HttpNotFound();
            if (task.Complete == true)
            {
                task.Complete = false;
                task.PercentageCompleted = 0;
            }
            else
            {
                task.Complete = true;
                task.PercentageCompleted = 100;
                nh.IsComplete(task.Id, true);
            }
            db.SaveChanges();

            return RedirectToAction("GetAllDeveloperTasks");
        }

        public ActionResult UpdateTaskPercentage(int taskId, string param)
        {
            ProjectTask task = db.Tasks.Find(taskId);
            if (task == null)
                return HttpNotFound();
            if (param == "+")
            {
                if (task.PercentageCompleted < 100)
                {
                    task.PercentageCompleted += 10;
                    if (task.PercentageCompleted >= 100)
                    {
                        task.Complete = true;
                        nh.IsComplete(task.Id, true);
                    }
                }    
            }
            else
            {
                if (task.PercentageCompleted > 0)
                {
                    task.PercentageCompleted -= 10;
                    task.Complete = false;
                }
            }
            db.SaveChanges();

            return RedirectToAction("GetAllDeveloperTasks");
        }
    }
}