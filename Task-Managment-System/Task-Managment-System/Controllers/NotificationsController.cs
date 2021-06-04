using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Task_Managment_System.Models;

namespace Task_Managment_System.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly ApplicationDbContext db ;
        private readonly NotificationHelper nh;
        private readonly TaskHelper th;
        private readonly ProjectHelper ph;
        private readonly UserManager um;

        public NotificationsController()
        {
            db = new ApplicationDbContext();
            nh = new NotificationHelper(db);
            th = new TaskHelper(db);
            ph = new ProjectHelper(db);
            um = new UserManager(db);
        }

        // GET: Notifications
        public ActionResult Index()
        {
            var notifications = db.Notifications.Include(n => n.Project).Include(n => n.Task).Include(n => n.User);
            return View(notifications.ToList());
        }

        // GET: Notifications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notification notification = db.Notifications.Find(id);
            if (notification == null)
            {
                return HttpNotFound();
            }
            return View(notification);
        }
        [Authorize(Roles = "ProjectManager")]
        // GET: Notifications/Create
        public ActionResult Create()
        {
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            ViewBag.TaskId = new SelectList(db.Tasks, "Id", "Title");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email");
            return View();
        }
        [Authorize(Roles = "ProjectManager")]
        // POST: Notifications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,TaskId,ProjectId,Title,Contents,Read,CreationDate,Type")] Notification notification)
        {
            if (ModelState.IsValid)
            {
                db.Notifications.Add(notification);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "CreatorId", notification.ProjectId);
            ViewBag.TaskId = new SelectList(db.Tasks, "Id", "ManagerId", notification.TaskId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", notification.UserId);
            return View(notification);
        }
        [Authorize(Roles = "ProjectManager")]
        // GET: Notifications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notification notification = db.Notifications.Find(id);
            if (notification == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "CreatorId", notification.ProjectId);
            ViewBag.TaskId = new SelectList(db.Tasks, "Id", "ManagerId", notification.TaskId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", notification.UserId);
            return View(notification);
        }
        [Authorize(Roles = "ProjectManager")]
        // POST: Notifications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,TaskId,ProjectId,Title,Contents,Read,CreationDate,Type")] Notification notification)
        {
            if (ModelState.IsValid)
            {
                db.Entry(notification).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "CreatorId", notification.ProjectId);
            ViewBag.TaskId = new SelectList(db.Tasks, "Id", "ManagerId", notification.TaskId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", notification.UserId);
            return View(notification);
        }

        // GET: Notifications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notification notification = db.Notifications.Find(id);
            if (notification == null)
            {
                return HttpNotFound();
            }
            return View(notification);
        }

        // POST: Notifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Notification notification = db.Notifications.Find(id);
            db.Notifications.Remove(notification);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult GetNumberOfUnOpenedNotifications(string userId)
        {
            int numberUnopened = nh.GetNumUnopened(userId);

            return Json(new { status=200, numberUnopened }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowNotificationsBy(string userId)
        {
            IEnumerable<Notification> notifications = nh.GetBy(userId).ToList();

            return View("Index", notifications);
        }


        {
            List<Project> overDueProjects = ph.Filter(FilterMethods.passedDeadLine);
            
            foreach(var project in overDueProjects)
                nh.Create(User.Identity.GetUserId(), "project passed deadline", project.Name, NotificationType.Overdue);

            db.SaveChanges();
        }

        {
            List<Project> overDueProjects = ph.Filter(FilterMethods.passedDeadLine);
            
            foreach(var project in overDueProjects)
                nh.Create(User.Identity.GetUserId(), "project passed deadline", project.Name, NotificationType.Overdue);

            db.SaveChanges();
        }

        {
            List<Project> overDueProjects = ph.Filter(FilterMethods.passedDeadLine);
            
            foreach(var project in overDueProjects)
                nh.Create(User.Identity.GetUserId(), "project passed deadline", project.Name, NotificationType.Overdue);

            db.SaveChanges();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
