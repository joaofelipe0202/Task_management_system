using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task_Managment_System.Models.ViewModels
{
    public class ProjectDetailsViewModel
    {
        public string CreatorId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public double ProjectBudget { get; set; }
        public DateTime ProjectCreatedDate { get; set; }
        public double ProjectActualCost { get; set; }
        public DateTime ProjectDeadline { get; set; }
        public ICollection<ApplicationUser> MembersAssigned { get; set; }
        public ProjectDetailsViewModel() { }
        public ProjectDetailsViewModel(Project project, List<ApplicationUser> members)
        {
            this.ProjectName = project.Name;
            this.ProjectDescription = project.Description;
            this.ProjectBudget = project.Budget;
            this.ProjectCreatedDate = project.DateCreated;
            this.ProjectActualCost = project.ActualCost;
            this.ProjectDeadline = project.Deadline;
            this.MembersAssigned = members;
        }
    }
}