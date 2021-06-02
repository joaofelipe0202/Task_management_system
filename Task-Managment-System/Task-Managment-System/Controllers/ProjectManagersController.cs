using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
        RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
        // GET: PM
        public ActionResult Index()
        {
            var projectList = db.Projects.OrderBy(p => p.Priority).ToList();
            return View("Dashboard",projectList);
        }

        private readonly ProjectHelper ph;

        private readonly TaskHelper th;
        private readonly UserManager um;

        public ProjectManagersController()
        {
            ph = new ProjectHelper(db);
            th = new TaskHelper(db);
            um = new UserManager(db);
        }

        //[HttpGet]
        //public ActionResult HideCompletedTasks(int? projectId)
        //{
        //    if (projectId == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    var project = db.Projects.Include("Tasks").FirstOrDefault(p => p.Id == projectId);

        //    if (project == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    var incompleteTasks = project.Tasks.Where(pt => pt.Complete == false).ToList();
        //    ViewBag.Title = "HideCompletedTasks";

        //    return View("ShowTasks", incompleteTasks);
        //}
        //?
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
        public ActionResult CreateNewProject()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateNewProject(string name, string description, double budget, DateTime deadline, Priority priority)
        {
            string creatorId = User.Identity.GetUserId();
            ph.Add(name, description, budget, deadline, priority, creatorId);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult DeleteProject(int? projectId)
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
            return View(project);
        }
        [HttpPost, ActionName("DeleteProject")]
        public ActionResult DeleteProjectConfirmed(int projectId)
        {
            ph.Delete(projectId);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Update(int? projectId)
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
            return View(project);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update([Bind(Include ="Id, Name, Description, Complete")]Project project, DateTime deadline, Priority priority)
        {
            project.CreatorId = User.Identity.GetUserId();
     
            ph.Update(project, deadline, priority);
            return RedirectToAction("Index");
        }
        //Search projects by name
        //?
        [HttpGet]
        public ActionResult ShowProjectByName(string name)
        {
            var projects = db.Projects.Where(p => p.Name.ToLower().Contains(name.ToLower())).ToList();

            return View("ShowProjects", projects);
        }
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
        //Actions under this line are working
        public ActionResult ShowTasksThatPassedDeadLine(int? projectId)
        {
            List<ProjectTask> tasks = new List<ProjectTask>();

            if (projectId != null)
            {
                tasks = th.Filter(FilterMethods.passedDeadLine);
            }

            return View("ShowTasks", tasks);
        }

        public ActionResult ShowAllProjects()
        {
            var projects = db.Projects.ToList();

            ViewBag.Title = "ShowAllProjects";

            return View("ShowProjects", projects);
        }

        public ActionResult ShowAllTasks()
        {
            var tasks = db.Tasks.ToList();

            ViewBag.Title = "ShowAllTasks";

            return View("ShowTasks", tasks);
        }
        public ActionResult ShowProjectsThatExceedTheBudget()
        {
            var projectExceed = db.Projects.Where(p => p.Budget < p.ActualCost).ToList();

            ViewBag.Title = "ShowProjectsThatExceedTheBudget";

            return View("ShowProjects", projectExceed);
        }
        public ActionResult ShowTasksFor(int? projectId)
        {
          
            List<ProjectTask> tasks = db.Tasks.ToList();

            if (projectId != null)
            {
                string projectName = db.Projects.Find(projectId).Name;
                tasks = th.By((int)projectId);

                ViewBag.Title = "Show Tasks For " + projectName;
            }

            
            ViewData["projectId"] = projectId;

            return View("ShowTasks", tasks);
        }

        public ActionResult ShowTasksOrderByCompletion(int? projectId)
        {
            ICollection<ProjectTask> tasks = th.OrderBy(OrderMethods.percentageComplete);

            if (projectId != null)
            {
                string projectName = db.Projects.Find(projectId).Name;

                tasks = th.OrderBy(OrderMethods.percentageComplete, (int)projectId);

                ViewBag.Title = "Show Tasks For " + projectName;
            }

            ViewData["projectId"] = projectId;

            return View("showtasks", tasks);
        }
        [HttpGet]
        public ActionResult CreateNewUser()
        {
            return View();

        }
        [HttpPost]
        public ActionResult CreateNewUser(string email, string password, double? salary, string role)
        {
            ViewBag.role = new SelectList(db.Roles, "Name", "Name");
            password = "NewUser123.";
            um.Create(email, salary, password, role);

            return RedirectToAction("AddUserToRole");
        }
        [HttpGet]
        public ActionResult AddUserToRole()
        {
            //SelectList Users
            ViewBag.UserId = new SelectList(db.Users.ToList(), "Id", "Email");
            //SelectList Roles
            ViewBag.Role = new SelectList(db.Roles.ToList(), "Name", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult AddUserToRole(string userId, string role)
        {

            ViewBag.UserId = new SelectList(db.Users.ToList(), "Id", "Email");
            
            ViewBag.Role = new SelectList(db.Roles.ToList(), "Name", "Name");

            um.AddUserToRole(userId, role);

            return RedirectToAction("Index");
        }
        //GET @Url.Action("ShowIncompleteTasks")
        [HttpGet]
        public JsonResult ShowIncompleteTasks(int? projectId)
        {
            List<ProjectTask> tasks = th.Filter(FilterMethods.incomplete);

            if(projectId != null)
            {
                tasks = th.Filter(FilterMethods.incomplete, (int)projectId);
            }
            //Avoid json cycle
            var result = tasks
                .Select(t =>
                    new
                    {
                        contents = t.Contents,
                        manager = t.Manager.UserName,
                        title = t.Title,
                        priority = t.Priority.ToString(),
                        id = t.Id,
                        percentageCompleted = t.PercentageCompleted
                    }
                );

            return Json(new { status = 200, tasks=result, projectId }, JsonRequestBehavior.AllowGet);
        }

        //GET @Url.Action("ShowIncompleteProjects")
        [HttpGet]
        public ActionResult ShowIncompleteProjects()
        {
            List<Project> projects = ph.Filter(FilterMethods.incomplete);

            return View("ShowProjects", projects);
        }


        //POST @Url.Action("UpdateCompleteStatus")
        [HttpPost]
        public JsonResult UpdateCompleteStatusForProject(int id, bool isChecked)
        {
            var project = db.Projects.Find(id);
            if (project == null)
                return Json(new { status = 404 });

            project.Complete = isChecked;

            db.SaveChanges();

            return Json(new { status = 200, project=project.Name });
        }

        //POST @Url.Action("UpdateCompleteStatusForTask")
        [HttpPost]
        public JsonResult UpdateCompleteStatusForTask(int id, bool isChecked)
        {
            var task = db.Tasks.Find(id);

            if (task == null)
            {
                return Json(new { status = 404 });
            }

            task.Complete = isChecked;

            db.SaveChanges();

            if (task.Complete)
            {
                task.PercentageCompleted = 100;
            }
            else
            {
                task.PercentageCompleted = 0;
            }

            db.SaveChanges();

            return Json(new { status = 200, task });
        }

        public ActionResult Dashboard()
        {
            var projectList = db.Projects.Include("Tasks").OrderBy(p => p.Priority).ToList();
            ViewBag.TaskHelper = th;

            return View( projectList);
        }
    }
}