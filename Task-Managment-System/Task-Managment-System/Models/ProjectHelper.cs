using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Task_Managment_System.Models
{
    public class ProjectHelper
    {
        private readonly ApplicationDbContext db;
        public ProjectHelper(ApplicationDbContext database)
        {
            db = database;
        }

        public void Add(string name, double budget, DateTime deadline, string creatorId)
        {
            var newProject = new Project(name, budget, deadline, creatorId);
            db.Projects.Add(newProject);

            db.SaveChanges();
        }

        public Project By(int id)
        {
            var projects = db.Projects.Find(id);

            return projects;
        }


        public void Delete(int projectId)
        {
            var project = db.Projects.Find(projectId);          
            db.Projects.Remove(project);

            db.SaveChanges();
        }


        public void Update(int projectId)
        {
            //check modify
            var project = db.Projects.Find(projectId);
            db.Entry(project).State = EntityState.Modified;

            db.SaveChanges();
        }

        public List<Project> Filter(FilterMethods method)
        {
            List<Project> projects = db.Projects.ToList();

            switch (method)
            {
                case FilterMethods.incomplete:
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

    }
}