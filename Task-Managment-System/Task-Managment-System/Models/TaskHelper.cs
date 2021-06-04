using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Task_Managment_System.Models.ViewModel;

namespace Task_Managment_System.Models
{
    public class TaskHelper
    {
        private readonly ApplicationDbContext db; 

        public TaskHelper(ApplicationDbContext database)
        {
            db = database;
        }

        public void Add(string title, string contents, DateTime deadline, Priority priority,int projectId,string managerId)
        {
            Project project = db.Projects.Find(projectId);
            ProjectTask task = new ProjectTask()
            {
                ManagerId = managerId,
                Title = title,
                Contents = contents,
                DateCreated = DateTime.Now,
                Deadline = deadline,
                Complete = false,
                ProjectId = projectId,
                PercentageCompleted = 0,
                Priority = priority
            };
            project.Tasks.Add(task);
            db.SaveChanges();
        }
        
        public void Add(ProjectTask task)
        {
            db.Tasks.Add(task);
            db.SaveChanges();
        }

        public void Delete(int taskId)
        {
            var task = db.Tasks.Find(taskId);
            if (task == null)
                return;
            var taskNotifications = db.Notifications.Where(n
                => n.Task == null ? false : n.Task.Id == taskId
            );
            if (taskNotifications.Count()>0)
                db.Notifications.RemoveRange(taskNotifications);
            db.Tasks.Remove(task);
            db.SaveChanges();
        }

        public void Update(ProjectTask task)
        {
            ProjectTask newTask = db.Tasks.Find(task.Id);
            
            if (newTask == null)
                return;

            newTask.Title = task.Title;
            newTask.Contents = task.Contents;
            newTask.Deadline = DateTime.Parse(task.Deadline.ToString());
            newTask.Complete = task.Complete;
            newTask.Priority = task.Priority;

            db.Entry(newTask).State = EntityState.Modified;
            db.SaveChanges();
        }

        public List<ProjectTask> By(int projectId)
        {
            List<ProjectTask> tasks = db.Tasks.Where(t => t.ProjectId == projectId).ToList();

            return tasks;
        }

        public List<ProjectTask> Filter(FilterMethods method)
        {
            List<ProjectTask> tasks = db.Tasks.ToList();

            switch (method)
            {
                case FilterMethods.passedDeadLine:
                    tasks = tasksPassedDeadLine(tasks);
                    break;
                case FilterMethods.incomplete:
                    tasks = tasksAreNotCompleted(tasks);
                    break;
            }

            return tasks;
        }
        public List<ProjectTask> Filter(FilterMethods method, int projectId)
        {
            List<ProjectTask> tasks = this.By(projectId);

            switch (method)
            {
                case FilterMethods.passedDeadLine:
                    tasks = tasksPassedDeadLine(tasks);
                    break;
                case FilterMethods.incomplete:
                    tasks = tasksAreNotCompleted(tasks);
                    break;
            }

            return tasks;
        }

        private List<ProjectTask> tasksPassedDeadLine(List<ProjectTask> tasks)
        {
            var filteredTasks = tasks.Where(t => 
                    !t.Complete
                    && DateTime.Compare(t.Deadline, DateTime.Now) < 0
                )
                .ToList();

            return filteredTasks;
        }

        private List<ProjectTask> tasksAreNotCompleted(List<ProjectTask> tasks)
        {
             var filteredTasks=tasks.Where(t =>
                   t.Complete == false
               )
               .ToList();

            return filteredTasks;
        }

        public List<ProjectTask> OrderBy(OrderMethods method)
        {
            List<ProjectTask> tasks = db.Tasks.ToList();

            switch (method)
            {
                case OrderMethods.percentageComplete:
                    tasks = tasksOrderByPercentageComplete(tasks);
                    break;
            }

            return tasks;
        }

        public List<ProjectTask> OrderBy(OrderMethods method, int projectId)
        {
            List<ProjectTask> tasks = this.By(projectId);

            switch (method)
            {
                case OrderMethods.percentageComplete:
                    tasks = tasksOrderByPercentageComplete(tasks);
                    break;
            }

            return tasks;
        }

        private List<ProjectTask> tasksOrderByPercentageComplete(List<ProjectTask> tasks)
        {
            var orderedTasks = tasks.OrderByDescending(t => t.PercentageCompleted).ToList();

            return orderedTasks;
        }
        //Return developers that are assigned successfully
        public ApplicationUser Assign(ProjectTask task, ApplicationUser developer)
        {
            //verify are users developers
            var store = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(store);
            ApplicationUser assignedDeveloper = null;
         
            if (userManager.IsInRole(developer.Id, "developer"))
            {
                developer.Tasks.Add(task);
                assignedDeveloper = developer;
            }

            return assignedDeveloper;
        }   
        public List<ApplicationUser> Assign(ProjectTask task, List<ApplicationUser> developers)
        {
            //verify are users developers
            var store = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(store);
            List<ApplicationUser> assignedDevelopers = new List<ApplicationUser>();

            foreach(var developer in developers)
            {
                if(userManager.IsInRole(developer.Id, "developer"))
                {
                    developer.Tasks.Add(task);
                    assignedDevelopers.Add(developer);
                }
            }

            return assignedDevelopers;
        }
    }
}