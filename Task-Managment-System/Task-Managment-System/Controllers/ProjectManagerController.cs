using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Task_Managment_System.Models;

namespace Task_Managment_System.Controllers
{
    [Authorize(Roles = "ProjectManager")]
    public class ProjectManagerController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: PM
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ShowAllProjects()
        {
            var projects = db.Projects.OrderByDescending(p => p.PercentageCompleted).ToList();
            return View(projects);
        }
        [HttpGet]
        public ActionResult ShowAllTasks()
        {
            var tasks = db.Tasks.OrderByDescending(t => t.PercentageCompleted).ToList();
            return View(tasks);
        }
        [HttpGet]
        public ActionResult HideCompletedTasks()
        {
            var incompleteTasks = db.Tasks.Where(t => t.Complete == false).ToList();

            return View(incompleteTasks);
        }
        [HttpGet]
        public ActionResult ShowProjectsThatExceedTheBudget()
        {
            var projectExceed = db.Projects.Where(p => p.Budget < p.ActualCost).ToList();

            return View(projectExceed);
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
            return View();
        }
    }
}