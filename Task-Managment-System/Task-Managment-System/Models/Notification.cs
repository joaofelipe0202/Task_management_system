using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task_Managment_System.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int? TaskId { get; set; }
        public int? ProjectId { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public bool Read { get; set; }
        public DateTime CreationDate { get; set; }
        public NotificationType Type { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ProjectTask Task { get; set; }
        public virtual Project Project { get; set; }

        public Notification()
        {

        }

        public Notification(string userId, string title, string contents, NotificationType type, int? taskId, int? projectId)
        {
            UserId = userId;
            Title = title;
            Contents = contents;
            Read = false;
            CreationDate = DateTime.Now;
            Type = type;
            TaskId = taskId;
            ProjectId = projectId;
        }
    }
}