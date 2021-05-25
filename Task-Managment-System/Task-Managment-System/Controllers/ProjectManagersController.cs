using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Task_Managment_System.Models;

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
        public ActionResult ShowAllTasks()
        {
            var tasks = db.Tasks.OrderByDescending(t => t.PercentageCompleted).ToList();
            ViewBag.Title = "ShowAllTasks";
            return View("ShowTasks",tasks);
        }
        [HttpGet]
        public ActionResult HideCompletedTasks()
        {
            var incompleteTasks = db.Tasks.Where(t => t.Complete == false).ToList();
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
        public ActionResult ShowTotalCostCompletedProjects(int? id)
        {
            if(id == null)
            {
                return HttpNotFound();
            }
            var project = db.Projects.Find(id);

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
    }
}