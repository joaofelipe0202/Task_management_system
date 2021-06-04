using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Task_Managment_System.Models.ViewModel;

namespace Task_Managment_System.Models
{
    public class ProjectHelper
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        
        public ProjectHelper(ApplicationDbContext database)
        {
            db = database;
        }

        public void Add(string name, string description, double budget, DateTime deadline, Priority priority, string creatorId)
        {
            var newProject = new Project(name, description, budget, deadline, priority, creatorId);
            
            db.Projects.Add(newProject);

            try
            {
                db.SaveChanges();
            }
            catch(DbEntityValidationException e)
            {
                Console.WriteLine(e);
            }
            catch (DbUpdateException e)
            {
                var exception = HandleDbUpdateException(e);
                throw exception;
            }
        }

        public Project By(int id)
        {
            var projects = db.Projects.Find(id);

            return projects;
        }


        public void Delete(int projectId)
        {
            var project = db.Projects.Find(projectId);
            if (project == null)
                return;
            var projectNotifications = db.Notifications.Where(n
                => n.Project==null?false:n.Project.Id==projectId
            );
            if(projectNotifications.Count()>0)
                db.Notifications.RemoveRange(projectNotifications);
            db.Projects.Remove(project);
            db.SaveChanges();
        }
        public void Update(Project project, DateTime deadline, double budget, Priority priority, int percentage)
        {
            //check modify
            var projectToBeUpdated = db.Projects.FirstOrDefault(p => p.Id == project.Id);
            if (projectToBeUpdated == null)
                return;

            projectToBeUpdated.Deadline = deadline;
            projectToBeUpdated.Budget = budget;
            projectToBeUpdated.ActualCost = project.ActualCost;
            projectToBeUpdated.Priority = priority;
            projectToBeUpdated.Percentage = percentage;
            
            //if(budget == 0)
            //{
            //    projectToBeUpdated.Budget = project.Budget;
            //}
            //else
            //{
            //    projectToBeUpdated.Budget = budget;
            //}
            //if(priority == 0)
            //{
            //    projectToBeUpdated.Priority = project.Priority;
            //}
            //else
            //{
            //    projectToBeUpdated.Priority = priority;
            //}
            //if (percentage == 0)
            //    projectToBeUpdated.Percentage = project.Percentage;
            //else
            //    projectToBeUpdated.Percentage = percentage;
            //project.DateCreated = DateTime.Now;
            //project.Deadline = deadline;
            //project.Budget = budget;
            //project.Priority = priority;

            db.Entry(projectToBeUpdated).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
            }
        }
        public List<Project> Filter(FilterMethods method)
        {
            List<Project> projects = db.Projects.ToList();

            switch (method)
            {
                case FilterMethods.incomplete:
                    projects = ProjectsAreNotCompleted(projects);
                    break;
                case FilterMethods.passedDeadLine:
                    projects = ProjectsAreNotCompleted(projects);
                    break;
            }

            return projects;
        }

        private List<Project> ProjectsAreNotCompleted(List<Project> projects)
        {
            var filteredProjects = projects.Where(p =>
                    p.Complete == false
              )
              .ToList();

            return filteredProjects;
        }
        public ProjectDetailsViewModel Details(int projectId)
        {
            var project = db.Projects.Find(projectId);

            if(project == null)
                return null;
            var tasks = db.Tasks.Where(t => t.ProjectId == project.Id).ToList();

            var projectDetails = new ProjectDetailsViewModel(project, tasks);
            return projectDetails;
        }
        private List<Project> ProjectsPassedDeadline(List<Project> projects)
        {
            var filteredProjects = projects.Where(p =>
                    p.Complete == false
                    &&DateTime.Compare(DateTime.Now,p.Deadline)>0
              )
              .ToList();

            return filteredProjects;
        }

        private Exception HandleDbUpdateException(DbUpdateException dbu)
        {
            var builder = new StringBuilder("A DbUpdateException was caught while saving changes. ");

            try
            {
                foreach (var result in dbu.Entries)
                {
                    builder.AppendFormat("Type: {0} was part of the problem. ", result.Entity.GetType().Name);
                }
            }
            catch (Exception e)
            {
                builder.Append("Error parsing DbUpdateException: " + e.ToString());
            }

            string message = builder.ToString();
            return new Exception(message, dbu);
        }
        public ProjectDetailsViewModel ShowTasksNotCompleteAndOverdue(int projectId)
        {
            var project = db.Projects.Find(projectId);
            if (project == null)
                return null;

            var projectTasks = project.Tasks.ToList();

            var tasks = projectTasks.Where(t => t.Complete == false && DateTime.Now.CompareTo(t.Deadline) > 0).ToList();
            var projectDetails = new ProjectDetailsViewModel(project, tasks);

            return projectDetails;
        }
    }
}