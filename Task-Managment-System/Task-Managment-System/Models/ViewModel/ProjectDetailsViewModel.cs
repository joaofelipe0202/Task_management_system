using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task_Managment_System.Models.ViewModel
{
    public class ProjectDetailsViewModel
    {
        public string CreatorId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public double ProjectBudget { get; set; }
        public double ProjectActualCost { get; set; }
        public DateTime ProjectCreatedDate { get; set; }
        public bool Complete { get; set; }
        public virtual ICollection<ApplicationUser> ProjectMembers { get; set; }
        public virtual ICollection<ProjectTask> ProjectTasks { get; set; }
        public ProjectDetailsViewModel() { }
        public ProjectDetailsViewModel(Project project, List<ApplicationUser> members, List<ProjectTask> projectTasks)
        {
            this.CreatorId = project.CreatorId;
            this.ProjectName = project.Name;
            this.ProjectDescription = project.Description;
            this.ProjectBudget = project.Budget;
            this.ProjectCreatedDate = project.DateCreated;
            this.ProjectMembers = members;
            this.ProjectTasks = projectTasks;
        }
        
    }
}