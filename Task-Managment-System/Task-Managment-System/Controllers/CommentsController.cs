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
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var comments = db.Comments.Include(c => c.Creator);
            return View(comments.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.CreatorId = new SelectList(db.Users, "Id", "Email");
            ViewBag.ProjectTaskId = new SelectList(db.Tasks, "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Content,CreatorId,TaskId,Priority")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                var projectTask = db.Tasks.Where(t => t.Id == comment.TaskId).First();
                comment.ProjectTask = projectTask;                

                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CreatorId = new SelectList(db.Users, "Id", "Email", comment.CreatorId);
            ViewBag.ProjectTaskId = new SelectList(db.Tasks, "Id");
            return View(comment);
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