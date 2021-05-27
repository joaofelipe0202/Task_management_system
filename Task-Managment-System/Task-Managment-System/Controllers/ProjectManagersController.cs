using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Task_Managment_System.Models;
using Task_Managment_System.Models.ViewModel;

namespace Task_Managment_System.Controllers
{
    [Authorize(Roles = "ProjectManager")]
    public class ProjectManagersController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: PM
        public ActionResult Index()
        {
            var projectList = db.Projects.ToList();

            return View(projectList);
        }
        [HttpGet]
        public ActionResult ShowAllProjects()
        {
            var projects = db.Projects.OrderByDescending(p => p.Percentage).ToList();
            ViewBag.Title = "ShowAllProjects";
            return View("ShowProjects",projects);
        }
        [HttpGet]
        public ActionResult ShowAllTasks(int? projectId)
        {
            if(projectId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var project = db.Projects.Find(projectId);
            if(project == null)
            {
                return HttpNotFound();
            }

            var tasks = project.Tasks.OrderByDescending(t => t.PercentageCompleted).ToList();
            ViewBag.Title = "ShowAllTasks";
            return View("ShowTasks",tasks);
        }
        [HttpGet]
        public ActionResult HideCompletedTasks(int? projectId)
        {
            if(projectId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var project = db.Projects.Find(projectId);
            if(project == null)
            {
                return HttpNotFound();
            }
            var projectTasks = db.Tasks.Where(pt => pt.ProjectId == project.Id).ToList();
            var incompleteTasks = projectTasks.Where(pt => pt.Complete == false).ToList();
            ViewBag.Title = "HideCompletedTasks";
            return View("ShowTasks",incompleteTasks);
        }
        [HttpGet]
        public ActionResult ShowProjectsThatExceedTheBudget()
        {
            var projectExceed = db.Projects.Where(p => p.Budget < p.ActualCost).ToList();
            ViewBag.Title = "ShowProjectsThatExceedTheBudget";
            return View("ShowProjects", projectExceed);
        }
        [HttpGet]
        public ActionResult ShowTotalCostCompletedProjects(int? projectId)
        {
            if(projectId == null)
            {
                new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var project = db.Projects.Find(projectId);
            if(project == null)
            {
                return HttpNotFound();
            }

            if(project.Complete == true)
            {
                ViewBag.TotalCost = project.ActualCost;
            }
            else
            {
                ViewBag.Message = "Project is not completed yet.";
            }
            ViewBag.Title = "ShowTotalCostCompletedProjects";
            return View("ShowProjects");
        }
        [HttpGet]
        public ActionResult AddNewProject()
        {
            ViewBag.UserId = new SelectList(db.Users.ToList(), "id", "UserName");
            return View();
        }
        [HttpPost]
        public ActionResult AddNewProject(string name, double budget, DateTime deadline)
        {
            string creatorId = User.Identity.GetUserId().Substring(0, 10);

            ProjectHelper.Add(name, budget, deadline, creatorId);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult ProjectDetails(int? projectId)
        {
            if(projectId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var project = db.Projects.Find(projectId);
            if(project == null)
            {
                return HttpNotFound();
            }
            var members = db.Users.Where(u => u.Projects.Any(p => p.Id == project.Id)).ToList();
            var projectDetails = new ProjectDetailsViewModel(project, members);
            return View(projectDetails);
        }
    }
}