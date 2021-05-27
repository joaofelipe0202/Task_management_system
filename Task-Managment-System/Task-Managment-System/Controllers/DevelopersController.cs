using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Task_Managment_System.Models;

namespace Task_Managment_System.Controllers
{
    public class DevelopersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();        

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllDeveloperTasks()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var userName = user.UserName;
            var tasksList = db.Tasks.Where(t => t.AssignedUsers.Any(au => au.UserName == userName)).ToList();
            return View(tasksList);
        }
    }
}