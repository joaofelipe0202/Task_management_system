using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Task_Managment_System.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string CreatorId { get; set; }
        public string Name { get; set; }
        public double Budget { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Deadline { get; set; }
        public int Percentage { get; set; }
        public double ActualCost { get; set; }
        public string Description { get; set; }
        public Priority Priority { get; set; }
        public DateTime LastBudgetUpdate { get; set; }
        public bool Complete { get; set; }
     

        public virtual ApplicationUser Creator { get; set; }
        public virtual ICollection<ApplicationUser> Members { get; set; }
        public virtual ICollection<ProjectTask> Tasks { get; set; }

        public Project()
        {
        }

        public Project(string name,string description, double budget, DateTime deadline,Priority priority, string creatorId)
        {
            Name = name;
            Description = description;
            Budget = budget;
            DateCreated = DateTime.Now;
            Deadline = deadline;
            Priority = priority;
            CreatorId = creatorId;

            Members = new HashSet<ApplicationUser>();
            Tasks = new HashSet<ProjectTask>();
        }
    }
}