using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task_Managment_System.Models
{
    public class ProjectTask
    {
        public int Id { get; set; }
        public string ManagerId { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public int CompletedPercentage { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime Deadline { get; set; }
        public bool Complete { get; set; }
        public Priority Priority { get; set; }
        public Project Project { get; set; }
        public int ProjectId { get; set; }
        public double PercentageCompleted { get; set; }
        public virtual ApplicationUser Manager { get; set; }
        public virtual ICollection<ApplicationUser> AssignedUsers { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
      
        public ProjectTask()
        {
            AssignedUsers = new HashSet<ApplicationUser>();
        }

        public ProjectTask(string title, string contents, DateTime deadline, bool complete, Priority priority)
        {
            Title = title;
            Contents = contents;
            Deadline = deadline;
            Complete = complete;
            Priority = priority;

            AssignedUsers = new HashSet<ApplicationUser>();
            Comments = new HashSet<Comment>();
        }
    }
}