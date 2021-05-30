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

        public ActionResult CompletedTask(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProjectTask task = db.Tasks.Where(p => p.Id == id)
                               .Include(p => p.Comments)
                               .Include(p => p.AssignedUsers).First();

            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        public ActionResult CommentOnTask([Bind(Include = "Title,Content")] Comment comment, ProjectTask task)
        {
            if (ModelState.IsValid)
            {
                comment.CreatorId = User.Identity.GetUserId();
                comment.ProjectTaskId = task.Id;

                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("completedTask", new { id = task.Id });
            }
            return RedirectToAction("completedTask", new { id = task.Id });
        }
    }
}