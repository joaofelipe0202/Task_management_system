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
            ProjectTask newTask = new ProjectTask();
            var dbValues = db.Projects.ToList();
            var dbDevelopers = db.Users.ToList();

            var projectsDropdownList = new SelectList(dbValues.Select(item => new SelectListItem
            {
                Text = item.Name,
                Value = item.Id.ToString()
            }).ToList(), "Value", "Text");

            var developersDropDownList = new SelectList(dbDevelopers.Select(item => new SelectListItem
            {
                Text = item.UserName,
                Value = item.Id.ToString()
            }).ToList(), "Value", "Text");

            var taskViewModel = new Models.ViewModel.TaskViewModel()
            {
                ProjectTask = newTask,
                ProjectList = projectsDropdownList,
                DevelopersList = developersDropDownList
            };
            return View(taskViewModel);
        }

        [HttpPost]
        public ActionResult Create(Models.ViewModel.TaskViewModel task)
        {
            if (ModelState.IsValid)
            {
                Project project = db.Projects.Find(task.ProjectId);
                ApplicationUser developer = db.Users.Find(task.DeveloperId);

                task.ProjectTask.ManagerId = User.Identity.GetUserId();
                task.ProjectTask.Complete = false;
                task.ProjectTask.DateCreated = DateTime.Now;
                task.ProjectTask.Deadline = DateTime.Parse(task.ProjectTask.Deadline.ToString());
                task.ProjectTask.PercentageCompleted = 0;
                task.ProjectTask.Project = project;
                task.ProjectTask.ProjectId = project.Id;
                task.ProjectTask.AssignedUser = developer;
                task.ProjectTask.AssignedUserId = developer.Id;
                //add to the db
                th.Add(task.ProjectTask);

                return RedirectToAction("GetAllDeveloperTasks", "Developers");
            }

            return View(task);
        }

        public ActionResult Delete(int task)
        {
            if (ModelState.IsValid)
            {
                th.Delete(task);

                return RedirectToAction("GetAllDeveloperTasks", "Developers");
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