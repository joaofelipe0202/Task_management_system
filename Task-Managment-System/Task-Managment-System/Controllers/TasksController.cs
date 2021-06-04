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
        private readonly TaskHelper th;
        private NotificationHelper nh { get; set; }

        public TasksController()
        {
            th = new TaskHelper(db);
            nh = new NotificationHelper(db);
        }

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


            var project = db.Projects.FirstOrDefault(p => p.Id == task.ProjectId).Name;
            ViewBag.ProjectName = project;

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
                if (comment.IsUrgent)
                {
                    nh.UrgentNote(comment.TaskId, comment.Title, comment.Content);
                }
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("TaskDetails", new { id = db_task.Id });
            }
            return RedirectToAction("TaskDetails", new { id = db_task.Id });
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "ProjectManager")]
        public ActionResult Create(ProjectTask task)
        {
            if (ModelState.IsValid)
            {
                Project project = db.Projects.Find(1); //Project is returning 0

                task.Manager.Id = User.Identity.GetUserId();
                task.Complete = false;
                task.DateCreated = DateTime.Now;
                task.Deadline = DateTime.Parse(task.Deadline.ToString());
                task.PercentageCompleted = 0;
                task.Project = project;
                task.ProjectId = project.Id;
                //add to the db
                th.Add(task);

                return RedirectToAction("Index", "Home");
            }

            return View(task);
        }

        [Authorize(Roles = "ProjectManager")]
        public ActionResult Delete(int task)
        {
            if (ModelState.IsValid)
            {
                th.Delete(task);

                return RedirectToAction("Index", "Home");
            }

            return View(task);
        }

        [HttpGet]
        public ActionResult Edit(int taskId)
        {
            ProjectTask task = db.Tasks.Find(taskId);
            return View(task);
        }

       [HttpPost]
        public ActionResult Edit([Bind(Include = "Id, Title, Contents, Deadline, Complete, Priority")] ProjectTask task)
        {
            if (ModelState.IsValid)
            {
                th.Update(task.Id);
                return RedirectToAction("Index", "Home");
            }
            return View(task);
        }
    }
}