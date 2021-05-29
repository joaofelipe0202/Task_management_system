﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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

        [Authorize(Roles = "ProjectManager")]
        public ActionResult ShowDevelopers()
        {         
            var developers = db.Users.Where(u => 
                u.Roles.Any(r => r.RoleId == "8c3bf9bf-2")).ToList();           
            return View(developers);
        }

        [Authorize(Roles = "Developer, ProjectManager")]        
        public ActionResult GetAllDeveloperTasks()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            if (user == null)
                return HttpNotFound();

            var userName = user.UserName;
            var tasksList = db.Tasks.Where(t => t.AssignedUser.UserName == userName).ToList();
            return View(tasksList);
        }

        public ActionResult ChangeStatusOfTask(int taskId)
        {
            ProjectTask task = db.Tasks.Find(taskId);
            if (task == null)
                return HttpNotFound();
            if (task.Complete == true)
            {
                task.Complete = false;
            }
            else
            {
                task.Complete = true;
            }

            db.SaveChanges();

            return RedirectToAction("GetAllDeveloperTasks");
        }
    }
}