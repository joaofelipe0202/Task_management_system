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
            ViewBag.UserId = User.Identity.GetUserId();


            return View(projectList);
        }

        private readonly ProjectHelper ph;

        private readonly TaskHelper th;
        private readonly UserManager um;
        private readonly NotificationHelper nh;

        public ProjectManagersController()
        {
            ph = new ProjectHelper(db);
            th = new TaskHelper(db);
            um = new UserManager(db);
            nh = new NotificationHelper(db);
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
            projectDetails.ProjectActualCost = project.ActualCost;

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
        public ActionResult Update(int? projectId, DateTime deadline, double budget, Priority priority,double actualCost)
        {
            var project = ph.By((int)projectId);

            ph.Update(project, deadline, budget, priority,actualCost);
            return RedirectToAction("Index");
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
                tasks = th.Filter(FilterMethods.passedDeadLine,(int)projectId);
            }
            tasks = th.Filter(FilterMethods.passedDeadLine);

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
            var projectExceed = db.Projects.Where(p => p.Budget < p.ActualCost& p.Complete).ToList();

            ViewBag.Title = "Projects That Exceed The Budget";

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
        public ActionResult CreateNewUser(string email, string password, double? dailySalary)
        {
            ViewBag.role = new SelectList(db.Roles, "Name", "Name");
            password = "NewUser123.";
            um.Create(email, dailySalary, password);
            
            return RedirectToAction("AddUserToRole");
        }
        [HttpGet]
        public ActionResult AddUserToRole()
        {
            
            ViewBag.userId = new SelectList(db.Users.ToList(), "Id", "UserName");
            
            ViewBag.Role = new SelectList(db.Roles.ToList(), "Name", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult AddUserToRole(string userId, string role)
        {
            
            ViewBag.userId = new SelectList(db.Users.ToList(), "Id", "UserName");
            
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

        [HttpGet]
        public ActionResult ShowIncompleteProjects()
        {
            List<Project> projects = ph.Filter(FilterMethods.incomplete);

            ViewBag.UserId = User.Identity.GetUserId();

            return View("Index", projects);
        }


        //POST @Url.Action("UpdateCompleteStatus")
        [HttpPost]
        public JsonResult UpdateCompleteStatusForProject(int id, bool isChecked)
        {
            var project = db.Projects.Find(id);
            if (project == null)
                return Json(new { status = 404 });

            project.Complete = isChecked;
            nh.IsComplete(project.Id, false);

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
            nh.IsComplete(task.Id, true);

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

            return Json(new { status = 200, task=task.Title });
        }

        public ActionResult Dashboard()
        {
            var projectList = db.Projects.Include("Tasks").OrderBy(p => p.Priority).ToList();
            ViewBag.UserId = User.Identity.GetUserId();

            return View( projectList);
        }

        /*
        public ActionResult AssignTaskToUser(int taskId)
        {
            ProjectTask task = db.Tasks.Find(taskId);

            ViewBag.UserId = new SelectList(db.Users.ToList(), "Id", "Email");

            return View(task);
        }
        [HttpPost]
        public ActionResult AssignTaskToUser(int taskId, string userId)
        {
            ProjectTask task = db.Tasks.Find(taskId);

            ViewBag.UserId = new SelectList(db.Users.ToList(), "Id", "Email");

            ApplicationUser user = db.Users.Find(userId);

            task.AssignedUser = user;

            db.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult DeleteTask(int? taskId)
        {
            if (taskId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var task = db.Tasks.Find(taskId);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }
        [HttpPost]
        public ActionResult DeleteTask(int taskId)
        {
            th.Delete(taskId);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult CreateNewTask(int projectId)
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateNewTask(int projectId, ProjectTask projectTask, DateTime deadline)
        {
            string creatorId = User.Identity.GetUserId();
            th.Add(projectTask.Title, projectTask.Contents, projectTask.Deadline, projectTask.Priority, projectId, creatorId);
            return RedirectToAction("Index");
        }*/
    }
}