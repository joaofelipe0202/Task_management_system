using Newtonsoft.Json;
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
        public string AssignedUserId { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public int PercentageCompleted { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime Deadline { get; set; }
        public bool Complete { get; set; }
        public Priority Priority { get; set; }
        [JsonIgnore]
        public Project Project { get; set; }
        public int ProjectId { get; set; }
        [JsonIgnore]
        public virtual ApplicationUser Manager { get; set; }
        [JsonIgnore]
        public virtual ApplicationUser AssignedUser { get; set; }
        [JsonIgnore]
        public virtual ICollection<Comment> Comments { get; set; }
        [JsonIgnore]
        public virtual ICollection<Notification> Notifications { get; set; }

        public ProjectTask()
        {
            //AssignedUser = new ApplicationUser();
            Notifications = new HashSet<Notification>();
            Comments = new HashSet<Comment>();            
        }

        public ProjectTask(string title, string contents, DateTime deadline, bool complete, Priority priority)
        {
            Title = title;
            Contents = contents;
            Deadline = deadline;
            Complete = complete;
            Priority = priority;

            AssignedUser = new ApplicationUser();
            Comments = new HashSet<Comment>();
        }
    }
}