using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Task_Managment_System.Models;

namespace Task_Managment_System.Controllers
{
    public class TasksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TaskDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProjectTask task = db.Tasks.Where(p => p.Id == id)
                               .Include(p => p.Comments)
                               .Include(p => p.AssignedUser).First();

            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        public ActionResult CommentOnTask([Bind(Include = "Title,Content,TaskId,IsUrgent")] Comment comment)
        {
            ProjectTask db_task = db.Tasks.Where(t => t.Id == comment.TaskId).First();
            if (ModelState.IsValid)
            {                
                comment.ProjectTask = db_task;
                comment.CreatorId = User.Identity.GetUserId();                
                
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("TaskDetails", new { id = db_task.Id });
            }
            return RedirectToAction("TaskDetails", new { id = db_task.Id });
        }
    }
}