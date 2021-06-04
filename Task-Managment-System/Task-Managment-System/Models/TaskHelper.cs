using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.AspNet.Identity;
using System.Web;
using System.Web.Mvc;

namespace Task_Managment_System.Models
{
    public class TaskHelper
    {
        private readonly ApplicationDbContext db; 

        public TaskHelper(ApplicationDbContext database)
        {
            db = database;
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

            db.Tasks.Remove(task);
            db.SaveChanges();
        }

        public void Update(int taskId)
        {
            var task = db.Tasks.Find(taskId);
            if (task == null)
                return;

            db.Entry(task).State = EntityState.Modified;
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
                    tasks = TasksPassedDeadLine(tasks);
                    break;
                case FilterMethods.incomplete:
                    tasks = TasksAreNotCompleted(tasks);
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
                    tasks = TasksPassedDeadLine(tasks);
                    break;
                case FilterMethods.incomplete:
                    tasks = TasksAreNotCompleted(tasks);
                    break;
            }

            return tasks;
        }

        private List<ProjectTask> TasksPassedDeadLine(List<ProjectTask> tasks)
        {
            var filteredTasks = tasks.Where(t => 
                    t.Complete == false
                    && DateTime.Compare(t.Deadline, DateTime.Now) > 0
                )
                .ToList();

            return filteredTasks;
        }

        private List<ProjectTask> TasksAreNotCompleted(List<ProjectTask> tasks)
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
                    tasks = TasksOrderByPercentageComplete(tasks);
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
                    tasks = TasksOrderByPercentageComplete(tasks);
                    break;
            }

            return tasks;
        }

        private List<ProjectTask> TasksOrderByPercentageComplete(List<ProjectTask> tasks)
        {
            var orderedTasks = tasks.OrderByDescending(t => t.PercentageCompleted).ToList();

            return orderedTasks;
        }

    }
}