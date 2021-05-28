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
    //[Authorize(Roles = "ProjectManager")]
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
            return View("ShowProjects", projects);
        }
        [HttpGet]
        public ActionResult ShowAllTasks()
        {
            var tasks = db.Tasks.OrderByDescending(t => t.PercentageCompleted).ToList();
            ViewBag.Title = "ShowAllTasks";
            return View("ShowTasks", tasks);
        }
        [HttpGet]
        public ActionResult ShowTasksFor(int projectId)
        {
            var project = db.Projects.Include("Tasks").FirstOrDefault(p => projectId == p.Id);
            ICollection<ProjectTask> tasks = new List<ProjectTask>();

            if (project != null)
                tasks = project.Tasks;
            if (tasks == null)
                tasks = new List<ProjectTask>();

            ViewBag.Title = "ShowTasksFor " + project.Name;
            return View("ShowTasks", tasks);
        }

        [HttpGet]
        public ActionResult HideCompletedTasks(int? projectId)
        {
            if (projectId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var project = db.Projects.Find(projectId);
            if (project == null)
            {
                return HttpNotFound();
            }
            var projectTasks = db.Tasks.Where(pt => pt.ProjectId == project.Id).ToList();
            var incompleteTasks = projectTasks.Where(pt => pt.Complete == false).ToList();
            ViewBag.Title = "HideCompletedTasks";
            return View("ShowTasks", incompleteTasks);
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
            if (projectId == null)
            {
                new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var project = db.Projects.Find(projectId);
            if (project == null)
            {
                return HttpNotFound();
            }

            if (project.Complete == true)
            {
                ViewBag.TotalCost = project.ActualCost;
            }
            else
            {
                ViewBag.TotalCost = "Project is not completed yet.";
            }
            ViewBag.Title = "ShowTotalCostCompletedProjects";
            return View();
        }
        [HttpGet]
        public ActionResult AddNewProject()
        {
            ViewBag.UserId = new SelectList(db.Users.ToList(), "Id", "Email");
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
            if (projectId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var project = db.Projects.Find(projectId);
            if (project == null)
            {
                return HttpNotFound();
            }
            var members = db.Users.Where(u => u.Projects.All(p => p.Id == project.Id)).ToList();
            var tasks = db.Tasks.Where(t => t.ProjectId == project.Id).ToList();
            var projectDetails = new ProjectDetailsViewModel(project, members, tasks);

            return View(projectDetails);
        }
        [HttpGet]
        public ActionResult CreateNewUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateNewUser(string email, string password, double? dailySalary, string role)
        {
            password = "NewUser123.";
            ProjectHelper.CreateNewUser(email, password, dailySalary, role);

            return RedirectToAction("Index");
        }
        //Not idea why Put method wont work so i made it post
        //POST @Url.Action("UpdateCompleteStatus")
        [HttpPost]
        [Route("api/project/{id}")]
        public JsonResult UpdateCompleteStatusForProject(int? id, bool isChecked)
        {
            if (id == null)
            {
                return Json(new { status=404});
            }

            var project = db.Projects.Find(id);

            if (project == null)
            {
                return Json(new { status = 404 });
            }
            project.Complete = isChecked;

            db.SaveChanges();
            return Json(new { status = 200, project });
        }

        //POST @Url.Action("UpdateCompleteStatusForTask")
        [HttpPost]
        [Route("api/task/{id}")]
        public JsonResult UpdateCompleteStatusForTask(int? id, bool isChecked)
        {
            if (id == null)
            {
                return Json(new { status = 404 });
            }

            var task = db.Tasks.Find(id);

            if (task == null)
            {
                return Json(new { status = 404 });
            }

            task.Complete = isChecked;

            if(task.Complete)
            {
                task.PercentageCompleted = 100;
            }
            db.SaveChanges();
            return Json(new { status = 200, task });
        }
    }
}